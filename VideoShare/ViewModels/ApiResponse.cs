namespace VideoShare.ViewModels
{
  public class ApiResponse
  {
    public ApiResponse()
    {
      
    }

    public ApiResponse(int statusCode, string title = null, string message = null, object result = null)
    {
      StatusCode = statusCode;
      Title = title ?? GetDefaultTitle(statusCode);
      Message = message ?? GetDefaultMessage(statusCode);
      Result = result;

      if (statusCode == 200 || statusCode == 201)
      {
        IsSuccess = true;
      }
      else
      {
        IsSuccess = false;
      }
    }

    public int StatusCode { get; set; }
    public string Title { get; set; }
    public string Message { get; set; } 
    public object Result { get; set; }
    public bool IsSuccess { get; set; }
    private string GetDefaultTitle(int statusCode)
    {
      return statusCode switch
      {
        200 => "Success",
        201 => "Created",
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        500 => "Internal Server Error",
        _ => null
      };
    }
    private string GetDefaultMessage(int statusCode)
    {
      return statusCode switch
      {
        200 => "The request has succeeded.",
        201 => "The request has been fulfilled and resulted in a new resource being created.",
        400 => "The server could not understand the request due to invalid syntax.",
        401 => "The client must authenticate itself to get the requested response.",
        403 => "The client does not have access rights to the content.",
        404 => "The server can not find the requested resource.",
        500 => "The server has encountered a situation it doesn't know how to handle.",
        _ => null
      };
    }
  }
}
