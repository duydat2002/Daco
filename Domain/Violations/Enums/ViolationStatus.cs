namespace Daco.Domain.Violations.Enums
{
    public enum ViolationStatus
    {
        Pending   = 0, //Chờ xử lý
        Reviewing = 1, //Đang xem xét
        Confirmed = 2, //Xác nhận vi phạm
        Rejected  = 3, //Không vi phạm
        Resolved  = 4, //Đã xử lý xong
    }
}
