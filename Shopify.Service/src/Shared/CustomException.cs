namespace Shopify.Service.src.Shared;

public class CustomException : Exception
{
  public int StatusCode { get; set; }

  public CustomException(int statusCode, string message) : base(message)
  {
    StatusCode = statusCode;
  }

  public static CustomException NotAvailable(string message)
  {
    return new CustomException(400, message);
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

  public static CustomException NotAllowed(string message = "Not allowed.")
  {
    return new CustomException(405, message);
  }

  public static CustomException EmptyOrderDetails()
  {
    return new CustomException(400, "Empty products in the order.");
  }

  public static CustomException InvalidQuantity()
  {
    return new CustomException(400, "Quantity must be greater than 0.");
  }

  public static CustomException InvalidPrice()
  {
    return new CustomException(400, "Price at purchase must be greater than 0.");
  }
}