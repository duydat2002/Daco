namespace Daco.Application.Administration.AdminManagement
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

            try
            {
                admin.AssignRole(role.Id, request.AssignedByAdminId, request.ExpiresAt);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDTO.Failure(ErrorCodes.Admin.RoleAlreadyAssigned, ex.Message);
            }

            await _adminRepository.UpdateAsync(admin, cancellationToken);

            _logger.LogInformation(
                "Role {RoleCode} assigned to admin {AdminId}",
                role.RoleCode, admin.Id);

            return ResponseDTO.Success(new
            {
                admin.Id,
                RoleId = role.Id,
                RoleCode = role.RoleCode,
                RoleName = role.RoleName,
                request.ExpiresAt
            }, "Role assigned successfully");
        }
    }
}
