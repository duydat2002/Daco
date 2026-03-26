namespace Daco.Application.Administration.UserManagement.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(
            IAdminUserRepository adminRepository,
            ILogger<GetUserByIdQueryHandler> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting user detail {UserId}", request.UserId);

            var user = await _adminRepository.GetUserDetailAsync(request.UserId, cancellationToken);

            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            return ResponseDTO.Success(user);
        }
    }
}
