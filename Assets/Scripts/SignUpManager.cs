using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;


public class SignUpManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField fn_IF;
    [SerializeField]
    TMP_InputField ln_IF;
    [SerializeField]
    TMP_InputField phone_IF;
    [SerializeField]
    TMP_InputField email_IF;
    [SerializeField]
    TMP_InputField password_IF;

    public void Start()
    {
        fn_IF.onValidateInput += delegate (string input, int charIndex, char addedChar) { return OnValidateName(addedChar); };
        ln_IF.onValidateInput += delegate (string input, int charIndex, char addedChar) { return OnValidateName(addedChar); };
        phone_IF.onValidateInput += delegate (string input, int charIndex, char addedChar) { return OnValidatePhone(addedChar); };
    }

    private char OnValidateName(char charToValidate)
    {
        if(!Validator.IsCharLetter(charToValidate))
            charToValidate = '\0';
        return charToValidate;
    }
    private char OnValidatePhone(char charToValidate)
    {
        if (!Validator.IsCharInPhoneValid(charToValidate))
        {
            charToValidate = '\0';
        }
        return charToValidate;
    }
    public void OnSignUpClicked()
    {
        if (ValidateData())
        {
            string hashedPassword = PasswordHasher.HashPassword(password_IF.text);
            DatabaseManager.Singleton.AddUser(email_IF.text, phone_IF.text, fn_IF.text, ln_IF.text, hashedPassword);
            StartUpPageUIManager.Singleton.ShowLoginCanvas();
        }
    }

    private bool ValidateData()
    {
        if (phone_IF.text.Length < 7)
        {
            Debug.Log("enter valid phone");
            return false;
        }
        if (!Validator.IsPasswordValid(password_IF.text))
        {
            Debug.Log("enter password between  8 and 32 characters");
            return false;
        }
        if (fn_IF.text.Length < 1)
        {
            Debug.Log("enter valid name");
            return false;
        }
        if (!Validator.IsEmailValid(email_IF.text))
        {
            Debug.Log("enter valid email");
            return false;
        }
        return true;
    }
}
