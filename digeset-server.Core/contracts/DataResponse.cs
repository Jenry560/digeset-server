namespace Solurecwebapi.Reponse
{
    public class DataResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }



        public DataResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public DataResponse(bool isSuccess, string message, T result)
        {
            IsSuccess = isSuccess;
            Message = message;
            Result = result;
        }

        public DataResponse() => IsSuccess = false;
    }
}
