namespace Daco.Application.Administration.AdminManagement.Queries
{
    public class GetAdminByIdQueryHandler : IRequestHandler<GetAdminByIdQuery, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ILogger<GetAdminByIdQueryHandler> _logger;

        public GetAdminByIdQueryHandler(
            IAdminUserRepository adminUserRepository, 
            ILogger<GetAdminByIdQueryHandler> logger)
        {
            _adminRepository = adminUserRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetAdminByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting admin detail {AdminId}", request.AdminId);

            var admin = await _adminRepository.GetAdminDetailAsync(
                request.AdminId, cancellationToken);

            if (admin is null)
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.NotFound, "Admin not found");

            return ResponseDTO.Success(admin);
        }
    }
}
