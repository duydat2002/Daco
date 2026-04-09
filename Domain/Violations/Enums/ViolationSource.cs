namespace Daco.Domain.Violations.Enums
{
    public enum ViolationSource
    {
        UserReport    = 0, //Người dùng báo cáo
        AutoDetection = 1, //Hệ thống tự phát hiện
        AdminReview   = 2, //Admin review thủ công
        SystemScan    = 3, //Quét tự động định kỳ
    }
}
