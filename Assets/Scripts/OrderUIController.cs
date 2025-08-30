using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUIController : MonoBehaviour
{
    public int orderId;
    private TMP_Dropdown  orderDD;

    private void Start()
    {
        orderDD = GetComponent<TMP_Dropdown>();
    }
    public void OnOrderRemoved()
    {
        Destroy(gameObject);
        DatabaseManager.Singleton.UpdateOrderStatus(orderId, OrderStatus.Completed);
    }
    public void OnValueChanged()
    {
        orderDD.SetValueWithoutNotify(0);
        DatabaseManager.Singleton.UpdateOrderStatus(orderId, OrderStatus.Viewed);
    }
}
