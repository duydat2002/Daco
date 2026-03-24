namespace Daco.Application.Administration.AdminManagement.Commands
{
    public class RevokeAdminRoleCommandHandler : IRequestHandler<RevokeAdminRoleCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IPermissionCacheService _permissionCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RevokeAdminRoleCommandHandler> _logger;

        public RevokeAdminRoleCommandHandler(
            IAdminUserRepository adminRepository,
            IPermissionCacheService permissionCache,
            IUnitOfWork unitOfWork,
            ILogger<RevokeAdminRoleCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _permissionCache = permissionCache;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RevokeAdminRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
        "Revoking role {RoleId} from admin {AdminId} by {RevokedBy}",
        request.RoleId, request.AdminId, request.RevokedByAdminId);

            if (request.AdminId == request.RevokedByAdminId)
                return ResponseDTO.Failure(ErrorCodes.Admin.CannotUpdateSelf, "Cannot revoke your own role");

            var admin = await _adminRepository.GetByIdAsync(request.AdminId, cancellationToken);
            if (admin is null)
                return ResponseDTO.Failure(ErrorCodes.Admin.NotFound, "Admin not found");

            var role = await _adminRepository.GetRoleByIdAsync(request.RoleId, cancellationToken);
            if (role is null)
                return ResponseDTO.Failure(ErrorCodes.Admin.RoleNotFound, "Role not found");

            if (role.RoleCode == AdminRoles.SuperAdmin)
                return ResponseDTO.Failure(ErrorCodes.Admin.CannotAssignSuperAdmin, "Cannot revoke super admin role");

            var currentRoles = await _adminRepository.GetRolesAsync(admin.Id, cancellationToken);
            if (currentRoles.Count <= 1)
                return ResponseDTO.Failure(ErrorCodes.Admin.MustHaveAtLeastOneRole, "Admin must have at least one role");

            var assignment = await _adminRepository.GetRoleAssignmentAsync(admin.Id, role.Id);
            if (assignment is null)
                return ResponseDTO.Failure(ErrorCodes.Admin.RoleNotAssigned, "Role is not assigned");

            assignment.Revoke();

            _permissionCache.InvalidateCache(request.AdminId);

            _unitOfWork.TrackEntity(admin);

            _logger.LogInformation("Role {RoleCode} revoked from admin {AdminId}", role.RoleCode, admin.Id);

            return ResponseDTO.Success(new
            {
                admin.Id,
                RoleId = role.Id,
                role.RoleCode
            }, "Role revoked successfully");
        }
    }
}
