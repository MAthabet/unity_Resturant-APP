using TMPro;
using UnityEngine;

public class SearchItemController : MonoBehaviour
{
    [SerializeField]
    TMP_Text mealNameText;

    private Meals MealToEdit;

    public void LoadSearchUI(Meals meal)
    {
        MealToEdit = meal;
        mealNameText.text = MealToEdit.MealName;
    }

    public void OpenEditMenu()
    {
        MainMenuUIManager.Singleton.OpenEditMealMenu(MealToEdit);
    }

}
