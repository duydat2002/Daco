namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/sellers")]
    public class AdminSellerController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminSellerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{sellerId:guid}/approve")]
        [RequirePermission(AdminPermissions.Sellers.Approve)]
        public async Task<ActionResult<ResponseDTO>> ApproveSeller(
            Guid sellerId,
            [FromBody] ApproveSellerCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { 
                SellerId = sellerId,
                ApprovedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{sellerId:guid}/reject")]
        [RequirePermission(AdminPermissions.Sellers.Reject)]
        public async Task<ActionResult<ResponseDTO>> RejectSeller(
            Guid sellerId,
            [FromBody] RejectSellerCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                SellerId = sellerId,
                RejectedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{sellerId:guid}/suspend")]
        [RequirePermission(AdminPermissions.Sellers.Suspend)]
        public async Task<ActionResult<ResponseDTO>> SuspendSeller(
            Guid sellerId,
            [FromBody] SuspendSellerCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                SellerId = sellerId,
                SuspendedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{sellerId:guid}/reinstate")]
        [RequirePermission(AdminPermissions.Sellers.Reinstate)]
        public async Task<ActionResult<ResponseDTO>> ReinstateSeller(
            Guid sellerId,
            [FromBody] ReinstateSellerCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                SellerId = sellerId,
                ReinstatedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
