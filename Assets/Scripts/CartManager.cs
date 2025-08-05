using System.Collections.Generic;
using UnityEngine;
using System;

public class CartManager : MonoBehaviour
{
    public static CartManager Singleton { get; private set; }

    Dictionary<Meals,int> cartItems;
    float discount = 0.0f;

    public static event Action<Meals, int> OnCartChanged;
    public static event Action OnCartCleared;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            cartItems = new Dictionary<Meals, int>();
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        UpdateDiscount();
        if(discount > 0.0f)
            MainMenuUIManager.Singleton.ShowDiscountBanner();
    }
    public void AddToCart(Meals meal, int quantity)
    {
        if (cartItems.ContainsKey(meal))
        {
            cartItems[meal] += quantity;
        }
        else
        {
            cartItems[meal] = quantity;
        }
        OnCartChanged?.Invoke(meal, cartItems[meal]);
    }
    public void RemoveFromCart(Meals meal, int quantity)
    {
        if (cartItems.ContainsKey(meal))
        {
            cartItems[meal] -= quantity;
            int newQuantity = cartItems[meal];
            if (newQuantity <= 0)
            {
                cartItems.Remove(meal);
            }
            OnCartChanged?.Invoke(meal, newQuantity);
        }
    }
    public void RemoveFromCart(Meals meal)
    {
        if (cartItems.ContainsKey(meal))
        {
            cartItems.Remove(meal);
        }
        OnCartChanged?.Invoke(meal, 0);
    }
    public float GetCartTotalPrice()
    {
        float sum = 0;
        foreach (var item in cartItems) 
        {
            sum += item.Key.Price * item.Value;
        }
        
        return sum * (1 - discount);
    }
    private void UpdateDiscount()
    {
        if (DatabaseManager.Singleton.IsUserEligableForDiscount(SessionManager.Singleton.GetCurrntUserID()) > -1)
            discount = 0.1f;
        else
            discount = 0.0f;
    }
    public void MakeOrder()
    {
        int userID = SessionManager.Singleton.GetCurrntUserID();
        DatabaseManager.Singleton.AddOrder(userID, cartItems, GetCartTotalPrice(), discount);
        discount = 0.0f;
        ClearCart();
    }
    private void ClearCart()
    {
        cartItems.Clear();
        OnCartCleared?.Invoke();
    }
    public int GetItemQuantity(Meals item)
    {
        return cartItems[item];
    }


}
