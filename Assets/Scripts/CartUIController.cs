using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CartUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject CartUITemplet;
    [SerializeField]
    private Transform CartScrollView;
    [SerializeField]
    private TMP_Text cartTotalPriceText;

    private Dictionary<Meals, GameObject> itemUIMap = new Dictionary<Meals, GameObject>();

    private void Start()
    {
        CartManager.OnCartChanged += HandleItemChange;
        CartManager.OnCartCleared += RemoveAllItemsFromCart;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        CartManager.OnCartChanged -= HandleItemChange;
        CartManager.OnCartCleared -= RemoveAllItemsFromCart;
    }

    private void HandleItemChange(Meals meal, int newQuantity)
    {
        if (newQuantity <= 0)
        {
            if (itemUIMap.ContainsKey(meal))
            {
                RemoveItemFromCart(meal);
            }
        }
        else
        {
            if (itemUIMap.ContainsKey(meal))
            {
                itemUIMap[meal].GetComponent<CartItemController>().UpdateQuantity(newQuantity);
            }
            else
            {
                AddNewItemToCart(meal);
            }
        }
        UpdateTotalPriceText();
    }

    private void RemoveItemFromCart(Meals meal)
    {
        Destroy(itemUIMap[meal]);
        itemUIMap.Remove(meal);
    }
    private void RemoveAllItemsFromCart()
    {
        foreach(var meal in itemUIMap)
        {
            Destroy(itemUIMap[meal.Key]);
        }
        itemUIMap.Clear();
        UpdateTotalPriceText();
    }

    private void AddNewItemToCart(Meals meal)
    {
        Debug.Log("adding to ui");
        GameObject newItem = Instantiate(CartUITemplet, CartScrollView);
        newItem.GetComponent<CartItemController>().Init(meal);
        itemUIMap[meal] = newItem;
    }
    private void UpdateTotalPriceText()
    {
        cartTotalPriceText.text = $"Total: ${CartManager.Singleton.GetCartTotalPrice():F2}";
    }
}
