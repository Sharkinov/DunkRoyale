using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speed = 2f;
    public bool isEnemy = false;
    private bool isActive = true;
    private Rigidbody2D rb;

    [Header("Court Bounds")]
    public float minX = -2.2f;
    public float maxX = 2.2f;
    public float minY = -1.7f;
    public float maxY = 6.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isActive) return;

        if (isEnemy)
            rb.linearVelocity = Vector2.down * speed;
        else
            rb.linearVelocity = Vector2.up * speed;

        // Clamp inside court
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    public void Stop()
    {
        isActive = false;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Resume()
    {
        isActive = true;
        if (rb == null) rb = GetComponent<Rigidbody2D>(); 
        rb.bodyType = RigidbodyType2D.Dynamic;
}
}