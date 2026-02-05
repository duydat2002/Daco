namespace Daco.API.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<ResponseDTO> HandleResult(ResponseDTO result)
        {
            if (result.IsFailure)
                return BadRequest(ResponseDTO.Failure(result.Code, result.Message));

            return Ok(ResponseDTO.Success(result.Data, result.Message));
        }
    }
}
