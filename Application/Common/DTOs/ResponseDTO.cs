namespace Daco.Application.Common.DTOs
{
    public class ResponseDTO
    {
        public bool    IsSuccess { get; set; }
        public string  Code      { get; set; }
        public string? Message   { get; set; } = string.Empty;
        public object? Data      { get; set; }

        public ResponseDTO() { }

        public static ResponseDTO Success(object? data = null, string? message = null)
        => new()
        {
            IsSuccess = true,
            Code = "SUCCESS",
            Data = data,
            Message = message
        };

        public static ResponseDTO Failure(string code, string message)
            => new()
            {
                IsSuccess = false,
                Code = code,
                Message = message
            };

        public bool IsFailure => !IsSuccess;
    }
}
