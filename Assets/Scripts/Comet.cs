using UnityEngine;

public class Comet : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject comet;
    public string targetTag = "Planet";
    public string bulletTag = "Bullet"; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Destroy(comet);
        }
        else if (collision.gameObject.CompareTag(bulletTag))
        {
            Destroy(comet);
        }
    }
}