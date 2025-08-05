using UnityEngine;

public class FoodDestoryer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GoodFood") || collision.CompareTag("BadFood"))
        {
            Destroy(collision.gameObject);
        }
    }
}
