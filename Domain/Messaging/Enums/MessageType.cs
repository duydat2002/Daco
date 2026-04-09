namespace Daco.Domain.Messaging.Enums
{
    public enum MessageType
    {
        Text    = 0, //Text thường
        Image   = 1, //Hình ảnh
        Product = 2, //Card sản phẩm
        Order   = 3, //Card đơn hàng
        Voucher = 4, //Mã giảm giá
        System  = 5, //Tin nhắn hệ thống
    }
}
