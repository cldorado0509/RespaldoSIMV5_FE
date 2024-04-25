namespace SIM.Models
{
    public class ResponseMicroServicio
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null;
        public string MessageError { get; set; } = null;
        public object Result { get; set; } = null;
        public object Error { get; set; } = null;

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
    }
}