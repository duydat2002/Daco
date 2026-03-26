namespace Daco.Application.Administration.AdminManagement.Commands
{
    public class UpdateAdminStatusCommandHandler : IRequestHandler<UpdateAdminStatusCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IPermissionCacheService _permissionCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAdminStatusCommandHandler> _logger;

        public UpdateAdminStatusCommandHandler(
            IAdminUserRepository adminRepository,
            IPermissionCacheService permissionCache,
            IUnitOfWork unitOfWork,
            ILogger<UpdateAdminStatusCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _permissionCache = permissionCache;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateAdminStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Updating admin {AdminId} status to {Status} by {UpdatedBy}",
                request.AdminId, request.Status, request.UpdatedByAdminId);

            if (request.AdminId == request.UpdatedByAdminId)
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.CannotUpdateSelf, "Cannot update your own status");

            var admin = await _adminRepository.GetByIdAsync(request.AdminId, cancellationToken);
            if (admin is null)
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.NotFound, "Admin not found");

            var adminRoles = await _adminRepository.GetRolesAsync(admin.Id, cancellationToken);
            if (adminRoles.Contains(AdminRoles.SuperAdmin))
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.CannotUpdateSuperAdmin, "Cannot update super admin status");

            switch (request.Status)
            {
                case "active":
                    admin.Activate();
                    break;

                case "inactive":
                    admin.Deactivate();
                    break;

                case "suspended":
                    admin.Suspend(request.Reason!);
                    break;
            }

            await _adminRepository.UpdateAsync(admin, cancellationToken);
            if (request.Status != "active")
                _permissionCache.InvalidateCache(request.AdminId);
            _unitOfWork.TrackEntity(admin);

            _logger.LogInformation(
                "Admin {AdminId} status updated to {Status}",
                request.AdminId, request.Status);

            return ResponseDTO.Success(new
            {
                admin.Id,
                request.Status
            }, "Admin status updated successfully");
        }
    }
}
