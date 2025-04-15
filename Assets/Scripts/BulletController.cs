using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }
}
