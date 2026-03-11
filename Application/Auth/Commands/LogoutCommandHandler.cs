using Microsoft.Extensions.Logging;

namespace Daco.Application.Auth.Commands
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ResponseDTO>
    {
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(
            ILoginSessionRepository sessionRepository,
            IJwtService jwtService,
            ILogger<LogoutCommandHandler> logger)
        {
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logout request for user {UserId}, AllDevices={LogoutAllDevices}",
                request.UserId, request.LogoutAllDevices);

            if (request.LogoutAllDevices)
            {
                var allSessions = await _sessionRepository.GetAllActiveByUserIdAsync(
                    request.UserId!.Value, cancellationToken);

                foreach (var s in allSessions)
                    s.Revoke("user_logout");

                _logger.LogInformation("Revoked {Count} sessions for user {UserId}",
                    allSessions.Count, request.UserId);
            }
            else
            {
                var tokenHash = _jwtService.HashToken(request.RawToken!);
                var session = await _sessionRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

                if (session is null || session.UserId != request.UserId)
                {
                    _logger.LogWarning("Logout: session not found or mismatch for user {UserId}", request.UserId);
                    return ResponseDTO.Success(null, "Logged out successfully");
                }

                session.Revoke("user_logout");
            }

            return ResponseDTO.Success(null, "Logged out successfully");
        }
    }
}
