namespace Daco.Domain.Returns.Enums
{
    public enum ReturnStatus
    {
        Pending   = 0, //Chờ duyệt
        Approved  = 1, //Đã duyệt
        Rejected  = 2, //Từ chối
        Shipping  = 3, //Đang gửi hàng về
        Received  = 4, //Shop đã nhận hàng
        Refunded  = 5, //Đã hoàn tiền
        Completed = 6, //Hoàn tất
    }
}
