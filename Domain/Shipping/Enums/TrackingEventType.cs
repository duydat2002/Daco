namespace Daco.Domain.Shipping.Enums
{
    public enum TrackingEventType
    {
        OrderPlaced       = 0,  //Đơn hàng được tạo
        OrderConfirmed    = 1,  //Shop xác nhận
        Preparing         = 2,  //Đang chuẩn bị hàng
        ReadyForPickup    = 3,  //Sẵn sàng lấy hàng
        PickedUp          = 4,  //Đã lấy hàng
        InTransit         = 5,  //Đang vận chuyển
        ArrivedAtHub      = 6,  //Đến hub/kho
        OutForDelivery    = 7,  //Đang giao hàng
        DeliveryAttempted = 8,  //Giao không thành công
        Delivered         = 9,  //Đã giao
        ReturnedToSender  = 10, //Trả về người gửi
        Cancelled         = 11, //Hủy
        Exception         = 12  //Ngoại lệ/Sự cố
    }
}
