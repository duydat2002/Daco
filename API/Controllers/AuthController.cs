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

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ResponseDTO>> RefreshToken(
            [FromBody] RefreshTokenCommand command,
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

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> Login(
            [FromBody] LoginCommand command,
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

        [HttpPost("login/google")]
        public async Task<ActionResult<ResponseDTO>> RegisterWitGoogle(
            [FromBody] LoginWithGoogleCommand command,
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ResponseDTO>> Logout(
            [FromBody] LogoutCommand command,
            CancellationToken cancellationToken)
        {
            var rawToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            command = command with
            {
                UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                RawToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "")
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPost("link/google")]
        public async Task<ActionResult<ResponseDTO>> LinkGoogleAccount(
            [FromBody] LinkGoogleAccountCommand command,
            CancellationToken cancellationToken)
        {
            var rawToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            command = command with
            {
                UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
