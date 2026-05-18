using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speed = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = Vector2.up * speed;
    }
}