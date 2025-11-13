namespace Mosaic.Shared;

public class Response<T>
{
    public bool WasSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; }

    public static Response<T> Success(T data)
    {
        return new Response<T>
        {
            WasSuccessful = true,
            Data = data,
        };
    }

    public static Response<T> Success(string message, T data)
    {
        return new Response<T>
        {
            WasSuccessful = true,
            Data = data,
            Message = message
        };
    }

    public static Response<T> Failure(string message)
    {
        return new Response<T>
        {
            WasSuccessful = false,
            Message = message,
        };
    }
}
