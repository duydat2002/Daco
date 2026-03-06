namespace Daco.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDTO>> Register(
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("register/google")]
        public async Task<ActionResult<ResponseDTO>> RegisterWitGoogle(
            [FromBody] RegisterWithGoogleCommand command,
            CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("resend-otp")]
        public async Task<ActionResult<ResponseDTO>> VerifyEmail(
            [FromBody] ResendOtpCommand command,
            CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ResponseDTO>> VerifyEmail(
            [FromBody] VerifyEmailCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                DeviceType = DeviceDetector.Detect(HttpContext.Request.Headers.UserAgent.ToString())
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
