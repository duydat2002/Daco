namespace Daco.Domain.Sellers.Enums
{
    public enum PenaltyType
    {
        Warning           = 0, //Cảnh báo
        Points            = 1, //Trừ điểm
        Product_removal   = 2, //Gỡ sản phẩm
        Temporary_suspend = 3, //Tạm khóa
        Permanent_ban     = 4, //Cấm vĩnh viễn
        Fine              = 5, //Phạt tiền
    }
}
