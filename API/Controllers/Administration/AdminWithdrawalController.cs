namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/withdrawal")]
    public class AdminWithdrawalController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminWithdrawalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{withdrawalId:guid}/approve")]
        [RequirePermission(AdminPermissions.Withdrawals.Approve)]
        public async Task<ActionResult<ResponseDTO>> Approve(
            Guid withdrawalId,
            [FromBody] ApproveWithdrawalCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                WithdrawalId = withdrawalId,
                ApprovedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{withdrawalId:guid}/reject")]
        [RequirePermission(AdminPermissions.Withdrawals.Reject)]
        public async Task<ActionResult<ResponseDTO>> Reject(
            Guid withdrawalId,
            [FromBody] RejectWithdrawalCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                WithdrawalId = withdrawalId,
                RejectedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{withdrawalId:guid}/complete")]
        [RequirePermission(AdminPermissions.Withdrawals.Approve)]
        public async Task<ActionResult<ResponseDTO>> Complete(
            Guid withdrawalId,
            [FromBody] CompleteWithdrawalCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                WithdrawalId = withdrawalId,
                CompletedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
