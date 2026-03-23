namespace Daco.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Profile
        [Authorize]
        [HttpPut("profile")]
        public async Task<ActionResult<ResponseDTO>> UpdateProfile(
            [FromBody] UpdateUserProfileCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPut("avatar")]
        public async Task<ActionResult<ResponseDTO>> UpdateAvatar(
            IFormFile avatar,
            CancellationToken cancellationToken)
        {
            var command = new UpdateAvatarCommand
            {
                UserId = CurrentUserId,
                FileStream = avatar.OpenReadStream(),
                FileName = avatar.FileName,
                ContentType = avatar.ContentType,
                FileSize = avatar.Length
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPut("username")]
        public async Task<ActionResult<ResponseDTO>> UpdateUsername(
            [FromBody] UpdateUsernameCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPut("email")]
        public async Task<ActionResult<ResponseDTO>> UpdateEmail(
            [FromBody] UpdateEmailCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPut("phone")]
        public async Task<ActionResult<ResponseDTO>> UpdatePhone(
            [FromBody] UpdatePhoneCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion

        #region Address
        [HttpPost("addresses")]
        public async Task<ActionResult<ResponseDTO>> AddAddress(
            [FromBody] AddAddressCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPut("addresses/{addressId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateAddress(
            Guid addressId,
            [FromBody] UpdateAddressCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId,
                AddressId = addressId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPatch("addresses/{addressId:guid}/default")]
        public async Task<ActionResult<ResponseDTO>> SetDefaultAddress(
            Guid addressId,
            CancellationToken cancellationToken)
        {
            var command = new SetDefaultAddressCommand
            {
                UserId = CurrentUserId,
                AddressId = addressId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpDelete("addresses/{addressId:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteAddress(
            Guid addressId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteAddressCommand
            {
                UserId = CurrentUserId,
                AddressId = addressId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion

        #region Bank account
        [Authorize]
        [HttpPost("bank-accounts")]
        public async Task<ActionResult<ResponseDTO>> AddBankAccount(
            [FromBody] AddBankAccountCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPut("bank-accounts/{bankAccountId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateBankAccount(
            Guid bankAccountId,
            [FromBody] UpdateBankAccountCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId,
                BankAccountId = bankAccountId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPatch("bank-accounts/{bankAccountId:guid}/default")]
        public async Task<ActionResult<ResponseDTO>> SetDefaultBankAccount(
            Guid bankAccountId,
            CancellationToken cancellationToken)
        {
            var command = new SetDefaultBankAccountCommand
            {
                UserId = CurrentUserId,
                BankAccountId = bankAccountId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpDelete("bank-accounts/{bankAccountId:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteBankAccount(
            Guid bankAccountId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteBankAccountCommand
            {
                UserId = CurrentUserId,
                BankAccountId = bankAccountId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion

        #region Account Status
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("{userId:guid}/suspend")]
        public async Task<ActionResult<ResponseDTO>> SuspendUser(
            Guid userId,
            [FromBody] SuspendUserCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = userId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion
    }
}
