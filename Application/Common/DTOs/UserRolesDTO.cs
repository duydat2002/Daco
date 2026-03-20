namespace Daco.Application.Common.DTOs
{
    public class UserRolesDTO
    {
        [ColumnMapping("is_seller")]
        public bool IsSeller { get; set; }
        [ColumnMapping("is_admin")]
        public bool IsAdmin  { get; set; }
    }
}
