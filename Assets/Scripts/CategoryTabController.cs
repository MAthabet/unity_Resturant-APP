using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CategoryTabController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text catNameText;

    private int catID = -1;

    private Button button;
    public void Setup(string catName, int iD)
    {
        catNameText.text = catName;
        button = GetComponent<Button>();
        catID = iD;
        button.onClick.AddListener(() => OnCategorySelected(iD));
    }

    private void OnCategorySelected(int categoryID)
    {
        MainMenuUIManager.Singleton.DisplayCategoryPanel(catID);
    }
}
