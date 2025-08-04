using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartItemController : MonoBehaviour
{
    [SerializeField]
    RawImage img;
    [SerializeField]
    TMP_Text mealName;
    [SerializeField]
    TMP_Text totalPriceText;
    [SerializeField]
    TMP_Text quantityText;

    private Meals item;

    public void Init(Meals meal)
    {
        item = meal;
        UpdateQuantity(CartManager.Singleton.GetItemQuantity(meal));
        img.texture = MealImageData.GetMealImage(meal);
    }
    public void DeletItemFromCart()
    {
        CartManager.Singleton.RemoveFromCart(item);
    }
    public void UpdateQuantity(int newQuantity)
    {
        quantityText.text = newQuantity.ToString();
        UpdateTotalPrice(newQuantity);
    }

    private void UpdateTotalPrice(int newQuantity)
    {
        totalPriceText.text = $"${(item.Price * newQuantity):F2}";
    }
}
