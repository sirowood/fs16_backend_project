namespace Shopify.Service.src.Shared;

public class CustomException : Exception
{
  public int StatusCode { get; set; }

  public CustomException(int statusCode, string message) : base(message)
  {
    StatusCode = statusCode;
  }

  public static CustomException EmailIsNotAvailable()
  {
    return new CustomException(400, "Email is not available.");
  }

  public static CustomException NotFound(string msg = "Not found.")
  {
    return new CustomException(404, msg);
  }

  public static CustomException LoginFailed()
  {
    return new CustomException(400, "Invalid Email or Password.");
  }

  public static CustomException WrongPassword()
  {
    return new CustomException(401, "Wrong original password.");
  }

  public static CustomException NotAllowed()
  {
    return new CustomException(405, "Not allowed.");
  }
}