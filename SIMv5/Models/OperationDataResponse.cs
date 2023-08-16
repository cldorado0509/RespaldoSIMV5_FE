
namespace SIM.Models
{
    public class OperationDataResponse<T> : OperationResponse
    {
        public T Result { get; set; }
    }
}