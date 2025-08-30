using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class OrdersManager : MonoBehaviour
{
    [SerializeField]
    private List<Orders> newOrders;
    [SerializeField] 
    private List<Orders> viewdOrders;
    [SerializeField]
    private GameObject ordersContainer;
    [SerializeField]
    private GameObject orderUITemplet;
    [SerializeField]
    private AudioClip newOrderSound;
    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        if (!SessionManager.Singleton.IsCurrentUserAdmin())
        {
            Destroy(this);
        }
        viewdOrders = DatabaseManager.Singleton.GetAllOrdersByStatus(OrderStatus.Viewed);

        StartCoroutine(CheckForNewOrders());
    }

    private IEnumerator CheckForNewOrders()
    {
        while (true)
        {
            newOrders = DatabaseManager.Singleton.GetAllOrdersByStatus(OrderStatus.New);
            if (newOrders.Count > 0)
            {
                audioSource.PlayOneShot(newOrderSound);
            }
            yield return new WaitForSeconds(3.0f);
        }
    }
    public void ShowOrders()
    {
        if (ordersContainer.transform.childCount > 0)
        {
            foreach (Transform child in ordersContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (var order in newOrders)
        {
            PopulateOrderDropdown(order);
            DatabaseManager.Singleton.UpdateOrderStatus(order.OrderID, OrderStatus.Viewed);
        }
        foreach (var order in viewdOrders)
        {
            PopulateOrderDropdown(order);
        }
    }

    private void PopulateOrderDropdown(Orders order)
    {
        GameObject orderItem = Instantiate(orderUITemplet, ordersContainer.transform);

        Dictionary<string, int> map = new Dictionary<string, int>();
        List<string> list = new List<string>();
        map = DatabaseManager.Singleton.GetOrderItems(order.OrderID);
        list.Add($"Order N{order.OrderID}");
        foreach (var meal in map)
        {
            list.Add($"x {meal.Value}  {meal.Key}");
        }

        TMP_Dropdown dropdown = orderItem.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(list);

        orderItem.GetComponent<OrderUIController>().orderId = order.OrderID;
    }
}
