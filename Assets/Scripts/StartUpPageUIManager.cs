using UnityEngine;

public class StartUpPageUIManager : MonoBehaviour
{
    public static StartUpPageUIManager Singleton { get; private set; }

    [SerializeField]
    private GameObject loginCanvas;
    [SerializeField]
    private GameObject signUpCanvas;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            ShowLoginCanvas();
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowLoginCanvas()
    {
        loginCanvas.SetActive(true);
        signUpCanvas.SetActive(false);
    }
    public void ShowSignUpCanvas()
    {
        loginCanvas.SetActive(false);
        signUpCanvas.SetActive(true);
    }

    public void OnLoginClicked()
    {
        LoginManager.Singleton.ValidateLogin();
    }
    public void OnRegisterClicked()
    {
        ShowSignUpCanvas();
    }

}
