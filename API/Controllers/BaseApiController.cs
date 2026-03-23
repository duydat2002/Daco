namespace Daco.API.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<ResponseDTO> HandleResult(ResponseDTO result)
        {
            if (result.IsFailure)
                return BadRequest(ResponseDTO.Failure(result.Code, result.Message, result.Data));

            return Ok(ResponseDTO.Success(result.Data, result.Message));
        }
        protected Guid CurrentUserId
            => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        protected Guid CurrentAdminId
            => Guid.Parse(User.FindFirstValue("admin_id")!);

        protected string CurrentIpAddress
            => HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        protected string CurrentUserAgent
            => HttpContext.Request.Headers.UserAgent.ToString();

        protected string? CurrentDeviceType
            => DeviceDetector.Detect(HttpContext.Request.Headers.UserAgent.ToString());

        protected string? CurrentRawToken
        {
            get
            {
                var authHeader = HttpContext.Request.Headers.Authorization.ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    return null;

                return authHeader["Bearer ".Length..].Trim();
            }
        }
    }
}
