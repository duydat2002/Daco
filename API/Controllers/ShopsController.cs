namespace Daco.API.Controllers
{
    [ApiController]
    [Route("api/shops")]
    public class ShopsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ShopsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Onboarding
        [Authorize(Roles = UserRoles.Buyer)]
        [HttpPost("onboard")]
        public async Task<ActionResult<ResponseDTO>> Onboard(CancellationToken cancellationToken)
        {
            var command = new OnboardSellerCommand { UserId = CurrentUserId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("kyc")]
        public async Task<ActionResult<ResponseDTO>> SubmitKyc(
            [FromBody] SubmitSellerKycCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = CurrentUserId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion

        #region Profile
        [Authorize(Roles = UserRoles.Seller)]
        [HttpPut("me/basic-info")]
        public async Task<ActionResult<ResponseDTO>> UpdateBasicInfo(
            [FromBody] UpdateShopBasicInfoCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = CurrentUserId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("me/logo")]
        public async Task<ActionResult<ResponseDTO>> UpdateLogo(
            IFormFile logo,
            CancellationToken cancellationToken)
        {
            var command = new UpdateShopLogoCommand
            {
                UserId = CurrentUserId,
                FileStream = logo.OpenReadStream(),
                FileName = logo.FileName,
                ContentType = logo.ContentType,
                FileSize = logo.Length
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion

        #region Addresses
        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("me/addresses")]
        public async Task<ActionResult<ResponseDTO>> AddAddress(
            [FromBody] AddShopAddressCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = CurrentUserId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPut("me/addresses/{addressId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateAddress(
            Guid addressId,
            [FromBody] UpdateShopAddressCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId,
                AddressId = addressId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPatch("me/addresses/{addressId:guid}/default")]
        public async Task<ActionResult<ResponseDTO>> SetDefaultAddress(
            Guid addressId,
            CancellationToken cancellationToken)
        {
            var command = new SetDefaultShopAddressCommand
            {
                UserId = CurrentUserId,
                AddressId = addressId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpDelete("me/addresses/{addressId:guid}")]
        public async Task<ActionResult<ResponseDTO>> RemoveAddress(
            Guid addressId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveShopAddressCommand
            {
                UserId = CurrentUserId,
                AddressId = addressId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion

        #region Products
        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("me/products")]
        public async Task<ActionResult<ResponseDTO>> CreateProduct(
            [FromBody] CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = CurrentUserId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPut("me/products/{productId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateProduct(
            Guid productId,
            [FromBody] UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId,
                ProductId = productId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
        #endregion
    }
}
