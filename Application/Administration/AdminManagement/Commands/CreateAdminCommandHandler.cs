namespace Daco.Application.Administration.AdminManagement.Commands
{
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminUserRepository _adminRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAdminCommandHandler> _logger;

        public CreateAdminCommandHandler(
            IUserRepository userRepository,
            IAdminUserRepository adminRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            ILogger<CreateAdminCommandHandler> logger)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating admin {Username} by admin {CreatedBy}",
                request.Username, request.CreatedByAdminId);

            var existingUser = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
            if (existingUser is not null)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.UserAlreadyExists, "Email already exists");

            var existingAdmin = await _adminRepository.GetByEmployeeCodeAsync(request.EmployeeCode, cancellationToken);
            if (existingAdmin is not null)
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.EmployeeCodeExists, "Employee code already exists");

            var roles = await _adminRepository.GetRolesByIdsAsync(request.RoleIds, cancellationToken);
            if (roles.Count != request.RoleIds.Count)
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.RoleNotFound, "One or more roles not found");

            if (roles.Any(r => r.RoleCode == AdminRoles.SuperAdmin))
                return ResponseDTO.Failure(ErrorCodes.AdminErrors.CannotAssignSuperAdmin, "Cannot assign super admin role");

            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var user = User.CreateWithEmail(request.Username, request.Email, passwordHash);

            user.VerifyEmail();

            await _userRepository.AddAsync(user, cancellationToken);

            var admin = AdminUser.Create(
                userId: user.Id,
                assignedBy: request.CreatedByAdminId,
                employeeCode: request.EmployeeCode,
                department: request.Department,
                position: request.Position,
                workEmail: request.WorkEmail,
                workPhone: request.WorkPhone,
                notes: request.Notes);

            foreach (var role in roles)
                admin.AssignRole(role.Id, request.CreatedByAdminId);

            await _adminRepository.AddAsync(admin, cancellationToken);

            _unitOfWork.TrackEntity(user);
            _unitOfWork.TrackEntity(admin);

            _logger.LogInformation(
                "Admin created successfully: {AdminId}, User: {UserId}",
                admin.Id, user.Id);

            return ResponseDTO.Success(new
            {
                admin.Id,
                UserId = user.Id,
                Username = user.Username.Value,
                Email = user.Email?.Value,
                admin.EmployeeCode,
                admin.Department,
                admin.Position,
                Roles = roles.Select(r => r.RoleCode).ToList()
            }, "Admin created successfully");
        }
    }
}
