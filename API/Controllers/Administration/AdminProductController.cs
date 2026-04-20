
namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/products")]
    public class AdminProductController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId:guid}/approve")]
        [RequirePermission(AdminPermissions.Products.Approve)]
        public async Task<ActionResult<ResponseDTO>> ApproveProduct(
            Guid productId,
            [FromBody] ApproveProductCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { 
                ProductId = productId,
                ApprovedBy = CurrentAdminId 
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{productId:guid}/suspend")]
        [RequirePermission(AdminPermissions.Products.Suspend)]
        public async Task<ActionResult<ResponseDTO>> SuspendProduct(
            Guid productId,
            [FromBody] SuspendProductCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                ProductId   = productId,
                SuspendedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{productId:guid}/unsuspend")]
        [RequirePermission(AdminPermissions.Products.UnSuspend)]
        public async Task<ActionResult<ResponseDTO>> UnsuspendProduct(
            Guid productId,
            [FromBody] UnSuspendProductCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                ProductId     = productId,
                UnSuspendedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{productId:guid}/remove")]
        [RequirePermission(AdminPermissions.Products.Remove)]
        public async Task<ActionResult<ResponseDTO>> RemoveProduct(
            Guid productId,
            [FromBody] RemoveProductCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                ProductId = productId,
                RemovedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
