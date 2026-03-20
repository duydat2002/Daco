namespace Daco.API.Controllers
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
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                DeviceType = DeviceDetector.Detect(HttpContext.Request.Headers.UserAgent.ToString())
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
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                DeviceType = DeviceDetector.Detect(HttpContext.Request.Headers.UserAgent.ToString())
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ResponseDTO>> Logout(
            [FromBody] AdminLogoutCommand command,
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
    }
}
