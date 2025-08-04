using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class DashboardController : MonoBehaviour
{
    [SerializeField]
    GameObject[] mainmenuPanels;
    public void DisplayPanel(int targetPanel)
    {
        int n = mainmenuPanels.Length;
        for (int i = 0; i < n; i++)
        {
            if (i == targetPanel)
            {
                mainmenuPanels[i].SetActive(true);
                continue;
            }
            mainmenuPanels[i].SetActive(false);
        }
    }

    public void OnLogoutButtonClicked()
    {
        SessionManager.Singleton.ClearCurrentUser();

        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
