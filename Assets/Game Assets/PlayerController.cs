using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    int health = 3;
    long score = 0;
    bool isDead = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isDead)
        {
            return;
        }
        Move();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if(moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput)*-3, 3, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("BadFood"))
            {
                score += 10;
                GameUIManager.Singelton.UpdateScore(score);
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.CompareTag("GoodFood"))
            {
                health--;
                if (health <= 0)
                {
                    isDead = true;
                }
                GameUIManager.Singelton.UpdateHealth(health);
                Destroy(collision.gameObject);
            }
        }
    }
}
