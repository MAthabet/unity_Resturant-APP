using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
public static class Validator
{
    public static bool IsEmailValid(string email)
    {
       return Regex.IsMatch(email,  @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$");
    }
    public static bool IsPasswordValid(string password)
    {
        if (password.Length < 8 || password.Length > 32)
            return false;
        return true;
    }
    public static bool IsCharLetter(char charToValidate)
    {
        if (charToValidate < 64 || charToValidate > 122)
        {
            return false;
        }
        else if (charToValidate > 90 && charToValidate < 97)
        {
            return false;
        }
        return true;
    }
    public static bool IsCharInPhoneValid(char charToValidate)
    {
        if (!char.IsDigit(charToValidate) && charToValidate != '+')
                return false;
        return true;
    }
}

public static class PasswordHasher
{
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 20;
    private const int ITERATIONS = 1000;

    public static string HashPassword(string password)
    {
        byte[] salt = new byte[SALT_SIZE];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
        byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

        byte[] hashSlat = new byte[SALT_SIZE + HASH_SIZE];
        Array.Copy(salt, 0, hashSlat, 0, SALT_SIZE);
        Array.Copy(hash, 0, hashSlat, SALT_SIZE, HASH_SIZE);

        return Convert.ToBase64String(hashSlat);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] hashSlat = Convert.FromBase64String(hashedPassword);

        byte[] salt = new byte[SALT_SIZE];
        Array.Copy(hashSlat, 0, salt, 0, SALT_SIZE);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
        byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

        for (int i = 0; i < HASH_SIZE; i++)
        {
            if (hashSlat[i + SALT_SIZE] != hash[i])
            {
                return false; 
            }
        }
        return true;
    }
}
