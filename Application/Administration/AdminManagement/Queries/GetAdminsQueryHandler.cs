namespace Daco.Application.Administration.AdminManagement.Queries
{
    public class GetAdminsQueryHandler : IRequestHandler<GetAdminsQuery, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ILogger<GetAdminsQueryHandler> _logger;

        public GetAdminsQueryHandler(
            IAdminUserRepository adminRepository,
            ILogger<GetAdminsQueryHandler> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting admins list");

            var result = await _adminRepository.GetAdminsAsync(request, cancellationToken);

            return ResponseDTO.Success(result);
        }
    }
}
