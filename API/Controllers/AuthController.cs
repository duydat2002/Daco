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
            var result = await _mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }

        [HttpPost("register/google")]
        public async Task<ActionResult<ResponseDTO>> RegisterWitGoogle(
            [FromBody] RegisterWithGoogleCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
