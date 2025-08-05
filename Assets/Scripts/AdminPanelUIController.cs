using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class AdminPanelUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject searchUITemplet;
    [SerializeField]
    private Transform searchScrollView;
    [SerializeField]
    private TMP_InputField searchbar;

    [SerializeField]
    private TMP_InputField mealName;
    [SerializeField]
    private TMP_InputField mealPrice;
    [SerializeField]
    private TMP_InputField mealImgLink;
    [SerializeField]
    private TMP_InputField mealDiscription;
    [SerializeField]
    private TMP_InputField mealCategory;
    [SerializeField]
    private GameObject EditPanel;

    private Meals currentMeal;

    public static event Action OnMealDataChanged;

    private void Start()
    {
        if (!SessionManager.Singleton.IsCurrentUserAdmin())
            Destroy(this.gameObject);
        searchbar.onValueChanged.AddListener(OnSearchValueChanged);
    }

    private void OnSearchValueChanged(string searchString)
    {
        foreach (Transform child in searchScrollView)
        {
            Destroy(child.gameObject);
        }

        List<Meals> searchResults = DatabaseManager.Singleton.SearchMealsByName(searchString);

        foreach (Meals meal in searchResults)
        {
            GameObject newItem = Instantiate(searchUITemplet, searchScrollView);
            newItem.GetComponent<SearchItemController>().LoadSearchUI(meal);
        }
    }
    public void OnSaveChangesClicked()
    {
        bool flag = float.TryParse(mealPrice.text, out float price);
        if(!flag)
        {
            Debug.Log("enter a valid price");
        }
        if (mealName.text == "")
        {
            Debug.Log("there Must be a meal name");
            flag = false;
        }
        if (mealPrice.text == "")
        {
            Debug.Log("there Must be a meal price");
            flag = false;
        }
        if (mealCategory.text == "")
        {
            Debug.Log("there Must be a meal category");
            flag = false;
        }
        if (flag)
        {
            if (currentMeal == null)
            { 
                DatabaseManager.Singleton.AddMeal(mealName.text, price, mealCategory.text, true, mealImgLink.text, mealDiscription.text);
                ClearAllInputs();
            }
            else
            {
                currentMeal.MealName = mealName.text;
                currentMeal.Price = price;
                currentMeal.Description = mealDiscription.text;
                currentMeal.ImagePath = mealImgLink.text;

                DatabaseManager.Singleton.UpdateMeal(currentMeal, mealCategory.text);
            }
            
            OnMealDataChanged?.Invoke();
        }


    }
    public void ShowPanelForNewMeal()
    {
        currentMeal = null;

        ClearAllInputs();

        EditPanel.SetActive(true);
    }

    public void ShowPanelForEdit(Meals mealToEdit)
    {
        currentMeal = mealToEdit;

        mealName.text = mealToEdit.MealName;
        mealPrice.text = mealToEdit.Price.ToString("F2");
        mealDiscription.text = mealToEdit.Description;
        mealImgLink.text = mealToEdit.ImagePath;
        mealCategory.text = DatabaseManager.Singleton.GetCategoryName(mealToEdit.CategoryID);

        EditPanel.SetActive(true);
    }
    private void ClearAllInputs()
    {
        mealName.text = "";
        mealPrice.text = "";
        mealImgLink.text = "";
        mealDiscription.text = "";
        mealCategory.text = "";
    }
}
