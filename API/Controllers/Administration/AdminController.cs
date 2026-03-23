namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/admins")]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = AdminRoles.SuperAdmin)]
        public async Task<ActionResult<ResponseDTO>> Create(
            [FromBody] CreateAdminCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                CreatedByAdminId = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{adminId}/status")]
        [Authorize(Roles = AdminRoles.SuperAdmin)]
        public async Task<ActionResult<ResponseDTO>> UpdateStatus(
            Guid adminId,
            [FromBody] UpdateAdminStatusCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                AdminId = adminId,
                UpdatedByAdminId = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{adminId}/roles")]
        [Authorize(Roles = AdminRoles.SuperAdmin)]
        public async Task<ActionResult<ResponseDTO>> AssignRole(
            Guid adminId,
            [FromBody] AssignAdminRoleCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                AdminId = adminId,
                AssignedByAdminId = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{adminId}/roles/{roleId}")]
        [Authorize(Roles = AdminRoles.SuperAdmin)]
        public async Task<ActionResult<ResponseDTO>> RevokeRole(
            Guid adminId,
            Guid roleId,
            CancellationToken cancellationToken)
        {
            var command = new RevokeAdminRoleCommand
            {
                AdminId = adminId,
                RoleId = roleId,
                RevokedByAdminId = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
