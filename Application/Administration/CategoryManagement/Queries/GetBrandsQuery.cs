namespace Daco.Application.Administration.CategoryManagement.Queries
{
    public record GetBrandsQuery : IRequest<ResponseDTO>
    {
        public string? Search   { get; init; }  
        public bool?   IsActive { get; init; }
        public int     Page     { get; init; } = 1;
        public int     PageSize { get; init; } = 20;
    }
}
