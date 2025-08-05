using UnityEngine;

public class LeaderboardEntryUIController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI[] Text;

    public void SetData(int rank, string playerName, int score)
    {
        Text[0].text = "#" + rank;
        Text[1].text = playerName;
        Text[2].text = score.ToString();
    }
}
