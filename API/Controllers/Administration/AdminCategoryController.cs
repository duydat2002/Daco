namespace Daco.API.Controllers.Administration
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminCategoryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [RequirePermission(AdminPermissions.Categories.Create)]
        public async Task<ActionResult<ResponseDTO>> Create(
            [FromBody] CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { CreatedBy = CurrentAdminId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{categoryId:guid}")]
        [RequirePermission(AdminPermissions.Categories.Update)]
        public async Task<ActionResult<ResponseDTO>> Update(
            Guid categoryId,
            [FromBody] UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command = command with {
                CategoryId = categoryId,
                UpdatedBy = CurrentAdminId 
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{categoryId:guid}")]
        [RequirePermission(AdminPermissions.Categories.Delete)]
        public async Task<ActionResult<ResponseDTO>> Delete(
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryCommand
            {
                CategoryId = categoryId,
                DeletedBy = CurrentAdminId
            };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        // Attributes
        [HttpPost("{categoryId:guid}/attributes")]
        [RequirePermission(AdminPermissions.Categories.Update)]
        public async Task<ActionResult<ResponseDTO>> CreateAttribute(
            Guid categoryId,
            [FromBody] CreateCategoryAttributeCommand command,
            CancellationToken cancellationToken)
        {
            command = command with
            {
                CategoryId = categoryId,
                CreatedBy = CurrentAdminId
            };

            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{categoryId:guid}/attributes/{attributeId:guid}")]
        [RequirePermission(AdminPermissions.Categories.Update)]
        public async Task<ActionResult<ResponseDTO>> UpdateAttribute(
        Guid categoryId,
        Guid attributeId,
        [FromBody] UpdateCategoryAttributeCommand command,
        CancellationToken cancellationToken)
        {
            command = command with { AttributeId = attributeId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }

        // Attribute Values
        [HttpPost("{categoryId:guid}/attributes/{attributeId:guid}/values")]
        [RequirePermission(AdminPermissions.Categories.Update)]
        public async Task<ActionResult<ResponseDTO>> AddAttributeValues(
            Guid categoryId,
            Guid attributeId,
            [FromBody] AddAttributeValueCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { AttributeId = attributeId };
            return HandleResult(await _mediator.Send(command, cancellationToken));
        }
    }
}
