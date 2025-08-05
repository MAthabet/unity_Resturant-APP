using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    Image[] hearts;
    [SerializeField]
    Sprite EmptyHeartSprite;
    [SerializeField]
    Transform GameOverPanel;

    public static GameUIManager Singelton { get; private set; }
    private void Awake()
    {
        if (Singelton == null)
        {
            Singelton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateScore(long score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateHealth(int health)
    {
        if(health >= 1)
        {
            hearts[health].sprite = EmptyHeartSprite;
        }
        else
        {
            hearts[0].sprite = EmptyHeartSprite;
            ShowGameOverPanel();
        }

    }
    public void OnRetryClicked()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnExitClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void AddScoreToDB()
    {
        int score = int.Parse(scoreText.text);
        int userID = SessionManager.Singleton.GetCurrntUserID();
        DatabaseManager.Singleton.AddScore(score, userID);
    }
    public void ShowGameOverPanel()
    {
        AddScoreToDB();
        GameOverPanel.gameObject.SetActive(true);
    }
}
