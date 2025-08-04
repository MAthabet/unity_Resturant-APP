using TMPro;
using UnityEngine;

public class SearchItemController : MonoBehaviour
{
    [SerializeField]
    TMP_Text mealNameText;

    private int MealID;

    public void LoadSearchUI(int mealID, string mealName)
    {
        MealID = mealID;
        mealNameText.text = mealName;
    }
}
