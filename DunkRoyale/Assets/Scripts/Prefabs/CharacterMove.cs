using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
