namespace GeneralStockMarket.CoreLib.Response
{
    public class Response<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public Error ErrorData { get; set; }

        public static Response<T> Success(T data, int statusCode) => new Response<T>()
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccessful = true
        };

        public static Response<T> Success(int statusCode) => new Response<T>()
        {
            Data = default,
            StatusCode = statusCode,
            IsSuccessful = true
        };

        public static Response<T> Fail(int statusCode, bool isShow, string path, params string[] errors) => new Response<T>()
        {
            StatusCode = statusCode,
            IsSuccessful = false,
            Data = default,
            ErrorData = Error.SendError(path, isShow, errors)
        };

        public static Response<T> Fail<D>(Response<D> response) => new Response<T>()
        {
            StatusCode = response.StatusCode,
            IsSuccessful = response.IsSuccessful,
            Data = default,
            ErrorData =response.ErrorData
        };
    }
}
