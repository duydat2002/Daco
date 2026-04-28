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

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("me/addresses")]
        public async Task<ActionResult<ResponseDTO>> AddAddress(
            [FromBody] AddShopAddressCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = CurrentUserId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
