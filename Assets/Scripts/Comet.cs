using System.Linq;
using UnityEngine;

public class Comet : MonoBehaviour
{
    public RectTransform planetRect;
    public float cometScreenRadius = 20f;
    public float bulletScreenRadius = 10f;

    void Update()
    {
        Vector2 cometScreen = Camera.main.WorldToScreenPoint(transform.position);
        if (RectTransformUtility.RectangleContainsScreenPoint(planetRect, cometScreen, null))    
        {
            Destroy(gameObject);
            return;
        }

        foreach (var bullet in FindObjectsOfType<Bullet>())
        {
            Vector2 bulletScreen;
            var bulletScreenPosition = bullet.GetComponent<RectTransform>();
            if (bulletScreenPosition != null)
                bulletScreen = bulletScreenPosition.position;
            
            else
                bulletScreen = Camera.main.WorldToScreenPoint(bullet.transform.position);

            if ((cometScreen - bulletScreen).magnitude
                  < cometScreenRadius + bulletScreenRadius)
            {
                Destroy(bullet.gameObject);
                Destroy(gameObject);
                return;
            }
        }
    }
}