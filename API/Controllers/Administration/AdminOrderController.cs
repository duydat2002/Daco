namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminOrderController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{orderId:guid}/cancel")]
        [RequirePermission(AdminPermissions.Orders.Cancel)]
        public async Task<ActionResult<ResponseDTO>> Cancel(
            Guid orderId,
            [FromBody] AdminCancelOrderCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                OrderId = orderId,
                AdminId = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{orderId:guid}/refund")]
        [RequirePermission(AdminPermissions.Orders.Refund)]
        public async Task<ActionResult<ResponseDTO>> Refund(
            Guid orderId,
            [FromBody] AdminRefundOrderCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                OrderId = orderId,
                AdminId = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
