namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/brands")]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminBrandController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminBrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [RequirePermission(AdminPermissions.Brands.Create)]
        public async Task<ActionResult<ResponseDTO>> Create(
            [FromBody] CreateBrandCommand command,
            CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{brandId:guid}")]
        [RequirePermission(AdminPermissions.Brands.Update)]
        public async Task<ActionResult<ResponseDTO>> Update(
            Guid brandId,
            [FromBody] UpdateBrandCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { BrandId = brandId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{brandId:guid}")]
        [RequirePermission(AdminPermissions.Brands.Delete)]
        public async Task<ActionResult<ResponseDTO>> Delete(
            Guid brandId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteBrandCommand { BrandId = brandId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{brandId:guid}/categories")]
        [RequirePermission(AdminPermissions.Brands.Update)]
        public async Task<ActionResult<ResponseDTO>> AssignCategories(
            Guid brandId,
            [FromBody] AssignBrandToCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { BrandId = brandId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{brandId:guid}/categories")]
        [RequirePermission(AdminPermissions.Brands.Update)]
        public async Task<ActionResult<ResponseDTO>> UnassignCategories(
            Guid brandId,
            [FromBody] UnassignBrandFromCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { BrandId = brandId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
