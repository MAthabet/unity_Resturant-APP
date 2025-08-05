using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
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
    [SerializeField]
    private RectTransform[] panels;
    [SerializeField]
    AdminPanelUIController adminPanelUIController;

    private Dictionary<int, GameObject> categoryPanels = new Dictionary<int, GameObject>();
    

    private int currentCatID = -1;
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
        //DisplayMainMenu();
    }

    private void DisplayMainMenu()
    {
        int n = panels.Length;
        for (int i = 1; i < n-1; i++)
        {
            panels[i].gameObject.SetActive(false);
        }

        panels[n - 1].gameObject.SetActive(true);
        panels[0].gameObject.SetActive(true);
    }

    void BuildUI()
    {
        List<MealCategories> allCategories = DatabaseManager.Singleton.GetAllCategories();
        List<Meals> allMeals = DatabaseManager.Singleton.GetAllMeals();

        foreach (var category in allCategories)
        {
            GameObject newPanel = Instantiate(MealContentTemplet, MealScrollView);
            newPanel.name = $"{category.CategoryName} Panel";
            int tempId = category.CategoryID;
            categoryPanels[tempId] = newPanel;
            

            GameObject newTab = Instantiate(CategoryButtonTemplet, CategoriesNamesPanel);
            newTab.GetComponent<CategoryTabController>().Setup(category.CategoryName, tempId);

            var mealsInCategory = allMeals.Where(m => m.CategoryID == tempId);
            foreach (var meal in mealsInCategory)
            {
                GameObject mealItem = Instantiate(MealUITemplet, newPanel.transform);
                mealItem.GetComponent<MealItemController>().LoadMealUI(meal);
            }

            newPanel.SetActive(false);
        }

        if (allCategories.Count > 0)
        {
            int fisrtCatID = allCategories[0].CategoryID;
            DisplayCategoryPanel(fisrtCatID);
        }
    }
    public void DisplayCategoryPanel(int catID)
    {
        if (currentCatID == catID)
            return;

        categoryPanels[catID].SetActive(true);
        if(currentCatID > 0)
            categoryPanels[currentCatID].SetActive(false);
        currentCatID = catID;
    }
    public void StartImageLoad(MealItemController targetController, string imagePath)
    {
        StartCoroutine(LoadImageCoroutine(targetController, imagePath));
    }

    private IEnumerator LoadImageCoroutine(MealItemController targetController, string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || targetController == null)
        {
            yield break;
        }
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture texture = DownloadHandlerTexture.GetContent(uwr);
                targetController.SetImage(texture);
            }
        }
    }

    public void OpenEditMealMenu(Meals meal)
    {
        adminPanelUIController.ShowPanelForEdit(meal);
    }
}
