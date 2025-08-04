using UnityEngine;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine.UI;
using System;

public class CategoryTabController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text catNameText;
    public int catID = -1;

    private Button button;
    public void Setup(string catName, int iD)
    {
        catNameText.text = catName;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnCategorySelected(iD));
    }

    private void OnCategorySelected(int categoryID)
    {
        MainMenuUIManager.Singleton.DisplayCategoryPanel(catID);
    }
}
