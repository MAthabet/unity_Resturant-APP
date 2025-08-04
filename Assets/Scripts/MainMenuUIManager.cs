using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Singleton { get; private set; }

    [SerializeField]
    private GameObject CategoryButtonTemplet;
    [SerializeField]
    private GameObject MealContentTemplet;
    [SerializeField]
    private GameObject MealUITemplet;

    [SerializeField]
    private Transform CategoriesNamesPanel;
    [SerializeField]
    private Transform MealScrollView;

    private Dictionary<int, GameObject> categoryPanels = new Dictionary<int, GameObject>();
    private Transform[] panels;

    private int currentCatID;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        BuildUI();
        panels = gameObject.GetComponentsInChildren<Transform>();
        int n = panels.Length;
        for (int i = 0; i < n; i++)
        {
            panels[i].gameObject.SetActive(false);
        }
        
        panels[n-1].gameObject.SetActive(true);
        panels[0].gameObject.SetActive(true);
    }
    void BuildUI()
    {
        List<MealCategories> allCategories = DatabaseManager.Singleton.GetAllCategories();
        List<Meals> allMeals = DatabaseManager.Singleton.GetAllMeals();

        foreach (var category in allCategories)
        {
            GameObject newPanel = Instantiate(MealContentTemplet, MealScrollView);
            newPanel.name = "{category.CategoryName} Panel";
            int tempId = category.CategoryID;
            categoryPanels[tempId] = newPanel;
            newPanel.SetActive(false);

            GameObject newTab = Instantiate(CategoryButtonTemplet, CategoriesNamesPanel);
            newTab.GetComponent<CategoryTabController>().Setup(category.CategoryName, tempId);

            var mealsInCategory = allMeals.Where(m => m.CategoryID == tempId);
            foreach (var meal in mealsInCategory)
            {
                GameObject mealItem = Instantiate(MealUITemplet, newPanel.transform);
                mealItem.GetComponent<MealItemController>().LoadMealUI(meal);
            }

            
        }

        if (allCategories.Count > 0)
        {
            int fisrtCatID = allCategories[0].CategoryID;
            DisplayCategoryPanel(fisrtCatID);
            currentCatID = fisrtCatID;
        }
    }
    public void DisplayCategoryPanel(int catID)
    {
        categoryPanels[catID].SetActive(true);
        categoryPanels[currentCatID].SetActive(false);
        currentCatID = catID;
    }
}
