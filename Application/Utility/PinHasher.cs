using System.Security.Cryptography;

namespace Application.Utility;

public static class PinHasher
{
    public static int HashPin(int pin)
    {
        byte[] bytes = BitConverter.GetBytes(pin);

        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(bytes);

        byte[] intBytes = hash.Take(4).ToArray();

        int hashedPin = BitConverter.ToInt32(intBytes, 0);
        return hashedPin;
    }
}