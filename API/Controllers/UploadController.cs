namespace Daco.API.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("image")]
        public async Task<ActionResult<ResponseDTO>> UploadImage(
            IFormFile image,
            [FromForm] string type,
            CancellationToken cancellationToken)
        {
            var command = new UploadImageCommand
            {
                UserId = CurrentUserId,
                Type = type,
                FileStream = image.OpenReadStream(),
                FileName = image.FileName,
                ContentType = image.ContentType,
                FileSize = image.Length
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("video")]
        public async Task<ActionResult<ResponseDTO>> UploadVideo(
            IFormFile video,
            [FromForm] string type,
            CancellationToken cancellationToken)
        {
            var command = new UploadVideoCommand
            {
                UserId = CurrentUserId,
                Type = type,
                FileStream = video.OpenReadStream(),
                FileName = video.FileName,
                ContentType = video.ContentType,
                FileSize = video.Length
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
