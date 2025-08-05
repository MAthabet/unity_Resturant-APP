using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    GameObject leaderboardItemPrefab;
    [SerializeField]
    Transform contentPanel;
    [SerializeField]
    int leaderboardSize = 3;

    void OnEnable()
    {
        LoadAndDisplayLeaderboard();
    }

    private void LoadAndDisplayLeaderboard()
    {
        List<GameScores> topScores = DatabaseManager.Singleton.GetLeaderboard(leaderboardSize);
        int rank = 1;
        foreach (var scoreEntry in topScores)
        {
            Users user = DatabaseManager.Singleton.GetUser(scoreEntry.UserID);

            GameObject newItem = Instantiate(leaderboardItemPrefab, contentPanel);

            newItem.GetComponent<LeaderboardEntryUIController>().SetData(rank, $"{user.FirstName} {user.LastName}", scoreEntry.Score);
            
            rank++;
        }
    }
}
