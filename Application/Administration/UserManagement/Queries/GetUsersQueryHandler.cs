namespace Daco.Application.Administration.UserManagement.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        public GetUsersQueryHandler(
            IAdminUserRepository adminRepository,
            ILogger<GetUsersQueryHandler> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting users list");

            //var result = await _adminRepository.GetAdminsAsync(request, cancellationToken);

            return ResponseDTO.Success(null);
        }
    }
}
