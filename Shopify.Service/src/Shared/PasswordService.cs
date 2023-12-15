using System.Text;
using System.Security.Cryptography;

namespace Shopify.Service.src.Shared;

public class PasswordService
{
  public static void HashPassword(string originalPassword, out string salt, out string hashedPassword)
  {
    using var hmac = new HMACSHA256();
    salt = Convert.ToBase64String(hmac.Key);
    hashedPassword = Convert
      .ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(originalPassword)))
      .Replace("-", "");
  }

  public static bool VerifyPassword(string originalPassword, string hashedPassword, string salt)
  {
    using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
    var computedHash = Convert
      .ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(originalPassword)))
      .Replace("-", "");
    return computedHash == hashedPassword;
  }
}