namespace Daco.Application.Auth.Queries
{
    public class GetActiveSessionsQueryHandler : IRequestHandler<GetActiveSessionsQuery, ResponseDTO>
    {
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<GetActiveSessionsQueryHandler> _logger;

        public GetActiveSessionsQueryHandler(
            ILoginSessionRepository sessionRepository,
            IJwtService jwtService,
            ILogger<GetActiveSessionsQueryHandler> logger)
        {
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetActiveSessionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting active sessions for user {UserId}", request.UserId);

            var sessions = await _sessionRepository.GetAllActiveByUserIdAsync(
                request.UserId!.Value, cancellationToken);

            var tokenHash = _jwtService.HashToken(request.CurrentToken);

            var result = sessions
                .Where(s => !s.IsExpired())
                .OrderByDescending(s => s.LastActivityAt)
                .Select(s => new
                {
                    s.Id,
                    s.LoginProvider,
                    s.DeviceType,
                    s.IpAddress,
                    s.UserAgent,
                    s.LoginAt,
                    s.LastActivityAt,
                    s.ExpiresAt,
                    IsCurrent = s.Token == tokenHash
                });

            return ResponseDTO.Success(result);
        }
    }
}
