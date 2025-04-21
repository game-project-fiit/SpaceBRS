using UnityEngine;

public class Comet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            Destroy(gameObject); 
        }
        
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject); 
            Destroy(collision.gameObject); 
        }
    }
}
