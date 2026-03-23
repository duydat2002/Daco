namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/auth")]
    public class AdminAuthController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> Login(
            [FromBody] AdminLoginCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                IpAddress = CurrentIpAddress,
                UserAgent = CurrentUserAgent,
                DeviceType = CurrentDeviceType
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("verify-otp")]
        public async Task<ActionResult<ResponseDTO>> VerifyOtp(
            [FromBody] AdminVerifyOtpCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                IpAddress = CurrentIpAddress,
                UserAgent = CurrentUserAgent,
                DeviceType = CurrentDeviceType
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ResponseDTO>> Logout(
            [FromBody] AdminLogoutCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                UserId = CurrentUserId,
                RawToken = CurrentRawToken
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
