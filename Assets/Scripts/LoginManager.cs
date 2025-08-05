using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Singleton { get; private set; }

    [SerializeField]
    TMP_InputField email_IF;
    [SerializeField]
    TMP_InputField password_IF;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void ValidateLogin()
    {
        if (!Validator.IsEmailValid(email_IF.text) || !Validator.IsPasswordValid(password_IF.text))
        {
            Debug.Log("please enter valid email and password");
            return;
        }
        Users user = DatabaseManager.Singleton.GetUser(email_IF.text);
        if (user == null)
        {
            Debug.Log("Invalid email or password");
            return;
        }
        else if (!PasswordHasher.VerifyPassword(password_IF.text, user.Password))
        {
            Debug.Log("Invalid email or password");
            return;
        }
        else
        {
            Debug.Log("Login successful");
            SessionManager.Singleton.SetCurrentUser(user);
            SceneManager.LoadScene("MainMenu");
        }
    }

}
