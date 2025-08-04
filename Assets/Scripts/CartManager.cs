using System.Collections.Generic;
using UnityEngine;
using System;

public class CartManager : MonoBehaviour
{
    public static CartManager Singleton { get; private set; }

    Dictionary<Meals,int> cartItems;
    float discount = 0.0f;

    public static event Action<Meals, int> OnCartChanged;

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
        cartItems = new Dictionary<Meals, int>();
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
    public void UpdateDiscount(float val)
    {
        discount = val;
    }
    public void MakeOrder()
    {
        int userID = SessionManager.Singleton.GetCurrntUserID();

        DatabaseManager.Singleton.AddOrder(userID, cartItems, GetCartTotalPrice());
    }

    public int GetItemQuantity(Meals item)
    {
        return cartItems[item];
    }


}
