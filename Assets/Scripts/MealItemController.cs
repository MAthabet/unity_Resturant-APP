using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MealItemController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text mealName;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private TMP_Text price;
    [SerializeField]
    private RawImage img;
    [SerializeField]
    private TMP_InputField quantity_IF;

    private Meals linkedMeal;
    private void Start()
    {
        quantity_IF.onValidateInput += delegate (string input, int charIndex, char addedChar) { return OnValidateQuantity(addedChar); };
    }
    public void LoadMealUI(Meals meal)
    {
        if (meal == null)
        {
            Debug.LogError("Meal is null");
            return;
        }

        mealName.text = meal.MealName;
        description.text = meal.Description;
        price.text = $"${meal.Price:F2}";
        linkedMeal = meal;
        MainMenuUIManager.Singleton.StartImageLoad(this, linkedMeal.ImagePath);
    }
    //private System.Collections.IEnumerator LoadImage(string imagePath)
    //{
    //    if (string.IsNullOrEmpty(imagePath))
    //    {
    //        yield break;
    //    }
    //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imagePath))
    //    {
    //        yield return uwr.SendWebRequest();
    //        if (uwr.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(uwr.error);
    //        }
    //        else
    //        {
    //            Texture texture = DownloadHandlerTexture.GetContent(uwr);
    //            MealImageData.SetMealImage(linkedMeal, texture);
    //            img.texture = texture;
    //        }
    //    }
    //}
    public void ChangeQuantity(int n)
    {
        if (int.TryParse(quantity_IF.text, out int currentQuantity) )
        {
            currentQuantity += n;
            if (currentQuantity < 0) currentQuantity = 0;
            quantity_IF.text = currentQuantity.ToString();
        }
        else if(quantity_IF.text == "")
        {
            currentQuantity = 0;
            currentQuantity += n;
            if (currentQuantity < 0) currentQuantity = 0;
            quantity_IF.text = currentQuantity.ToString();
        }
    }
    private char OnValidateQuantity(char charToValidate)
    {
        if (!char.IsDigit(charToValidate))
        {
            if(charToValidate != '\b')
                charToValidate = '\0';
        }
        else if (charToValidate == '0' && quantity_IF.text.Length == 0)
        {
            charToValidate = '\0';
        }
        else if(quantity_IF.text.Length > 3)
        {
            charToValidate = '\0';
        }
        return charToValidate;
    }

    public void OnIFSelect()
    {
        if (quantity_IF.text == "0")
        {
            quantity_IF.text = "";
        }
    }
    public void SetImage(Texture image)
    {
        MealImageData.SetMealImage(linkedMeal, image);
        img.texture = image;
    }
    public void OnAddToCartClicked()
    {
        if(int.TryParse(quantity_IF.text, out int quantity) && quantity > 0)
        {
            CartManager.Singleton.AddToCart(linkedMeal, quantity);
        }
    }
}
