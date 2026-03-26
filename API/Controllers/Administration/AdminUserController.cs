namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{userId:guid}/suspend")]
        [RequirePermission(AdminPermissions.Users.Suspend)]
        public async Task<ActionResult<ResponseDTO>> SuspendUser(
            Guid userId,
            [FromBody] SuspendUserCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = userId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{userId:guid}/ban")]
        [RequirePermission(AdminPermissions.Users.Ban)]
        public async Task<ActionResult<ResponseDTO>> BanUser(
            Guid userId,
            [FromBody] BanUserCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = userId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{userId:guid}/activate")]
        [RequirePermission(AdminPermissions.Users.Activate)]
        public async Task<ActionResult<ResponseDTO>> ActiveUser(
            Guid userId,
            [FromBody] ActivateUserCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = userId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpGet]
        [RequirePermission(AdminPermissions.Users.View)]
        public async Task<ActionResult<ResponseDTO>> GetUsers(
            [FromQuery] GetUsersQuery query,
            CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(query, cancellationToken));
        }    

        [HttpGet("{userId:guid}")]
        [RequirePermission(AdminPermissions.Users.View)]
        public async Task<ActionResult<ResponseDTO>> GetUserById(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(new GetUserByIdQuery { UserId = userId }, cancellationToken));
        }

        [HttpGet("{userId:guid}/orders")]
        [RequirePermission(AdminPermissions.Users.View)]
        public async Task<ActionResult<ResponseDTO>> GetUserOrders(
            Guid userId,
            [FromQuery] GetUserOrdersQuery query,
            CancellationToken cancellationToken)
        {
            query = query with { UserId = userId };
            return HandleResult(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("{userId:guid}/transactions")]
        [RequirePermission(AdminPermissions.Users.View)]
        public async Task<ActionResult<ResponseDTO>> GetUserTransactions(
            Guid userId,
            [FromQuery] GetUserTransactionsQuery query,
            CancellationToken cancellationToken)
        {
            query = query with { UserId = userId };
            return HandleResult(await _mediator.Send(query, cancellationToken));
        }
    }
}
