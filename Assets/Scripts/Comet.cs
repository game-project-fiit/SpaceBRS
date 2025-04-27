using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Comet : MonoBehaviour
{
    public RectTransform planetRect;
    public float cometScreenRadius = 20f;
    public float bulletScreenRadius = 10f;
    public TextMeshProUGUI cometText;

    private readonly Dictionary<string, int> cometPoints = new()
    {
        { "комп практика", 2 },
        { "коллок по матану", 4 },
        { "тысячи", 1 },
        { "кр по алгему", 4 },
        { "python task", 3 },
        { "дедлайн по ятп", 2 }, 
        { "экзамен по матану", 5},
        { "экзамен по алгему", 5},
        { "нтк по философии", 1},
        { "зачёт по питону", 4},
    };

    private void Start()
    {
        cometText = GetComponentInChildren<TextMeshProUGUI>();
        if (cometText != null)
        {
            cometText.text = GetRandomText();
        }
    }

    private void Update()
    {
        Vector2 cometScreen = Camera.main.WorldToScreenPoint(transform.position);
        if (RectTransformUtility.RectangleContainsScreenPoint(planetRect, cometScreen, null))
        {
            Destroy(gameObject);
            return;
        }

        if (cometText != null)
        {
            var offsetX = 20f; 
            cometText.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(offsetX, 0, 0);
        }

        foreach (var bullet in FindObjectsOfType<Bullet>())
        {
            Vector2 bulletScreen;
            var bulletScreenPosition = bullet.GetComponent<RectTransform>();
            if (bulletScreenPosition != null)
                bulletScreen = bulletScreenPosition.position;
            else
                bulletScreen = Camera.main.WorldToScreenPoint(bullet.transform.position);

            if ((cometScreen - bulletScreen).magnitude < (cometScreenRadius + bulletScreenRadius))
            {
                var cometTextValue = cometText.text;
                if (cometPoints.TryGetValue(cometTextValue, out var points))
                {
                    ScoreManager.Instance.IncreaseScore(points);
                    NotificationManager.Instance.ShowNotification(cometTextValue, points);
                }

                Destroy(bullet.gameObject);
                Destroy(gameObject);
                return;
            }
        }
    }


    private static string GetRandomText()
    {
        string[] texts =
            { "комп практика", "коллок по матану", "тысячи", "кр по алгему", "python task", "дедлайн по ятп", "экзамен по матану",  "экзамен по алгему", "нтк по философии", "зачёт по питону"};
        return texts[Random.Range(0, texts.Length)];
    }
}
