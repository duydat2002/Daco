namespace Daco.Application.Administration.AdminManagement.Commands
{
    public class AssignAdminRoleCommandHandler : IRequestHandler<AssignAdminRoleCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignAdminRoleCommandHandler> _logger;

        public AssignAdminRoleCommandHandler(
            IAdminUserRepository adminRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignAdminRoleCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AssignAdminRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Assigning role {RoleId} to admin {AdminId} by {AssignedBy}",
                request.RoleId, request.AdminId, request.AssignedByAdminId);

            var admin = await _adminRepository.GetByIdAsync(request.AdminId, cancellationToken);
            if (admin is null)
                return ResponseDTO.Failure(ErrorCodes.Admin.NotFound, "Admin not found");

            var role = await _adminRepository.GetRoleByIdAsync(request.RoleId, cancellationToken);
            if (role is null)
                return ResponseDTO.Failure(ErrorCodes.Admin.RoleNotFound, "Role not found");

            if (role.RoleCode == AdminRoles.SuperAdmin)
                return ResponseDTO.Failure(ErrorCodes.Admin.CannotAssignSuperAdmin, "Cannot assign super admin role");

            var existing = await _adminRepository.GetRoleAssignmentAsync(request.AdminId, role.Id, cancellationToken);
            if (existing is not null)
            {
                if (existing.IsActive)
                    return ResponseDTO.Failure(ErrorCodes.Admin.RoleAlreadyAssigned, "Role is already assigned");

                existing.Reactivate(request.AssignedByAdminId, request.ExpiresAt);
            }
            else
            {
                var assignment = AdminRoleAssignment.Create(
                    admin.Id,
                    role.Id,
                    request.AssignedByAdminId,
                    request.ExpiresAt);

                await _adminRepository.AddRoleAssignmentAsync(assignment, cancellationToken);
            }
            _unitOfWork.TrackEntity(admin);

            _logger.LogInformation("Role {RoleCode} assigned to admin {AdminId}", role.RoleCode, admin.Id);

            return ResponseDTO.Success(new
            {
                admin.Id,
                RoleId = role.Id,
                role.RoleCode,
                role.RoleName,
                request.ExpiresAt
            }, "Role assigned successfully");
        }
    }
}
