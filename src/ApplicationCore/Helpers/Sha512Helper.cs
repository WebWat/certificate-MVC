using System;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationCore.Helpers;

public static class Sha512Helper
{
    public static string GetRandomValue()
    {
        var sha512 = SHA512.Create();
        var hash = new StringBuilder();
        byte[] array = sha512.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

        foreach (byte @byte in array)
        {
            hash.Append(@byte.ToString("x2"));
        }

        return hash.ToString();
    }
}
