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

    private readonly Dictionary<string, int> cometPoints = new Dictionary<string, int>
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

    public TextMeshProUGUI notificationText;

    private void Start()
    {
        cometText = GetComponentInChildren<TextMeshProUGUI>();
        if (cometText != null)
        {
            cometText.text = GetRandomText();
        }

        notificationText.gameObject.SetActive(false);
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
            cometText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
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
                    ShowNotification(cometTextValue, points);
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
    
    private void ShowNotification(string cometTextValue, int points)
    {
        var message = cometTextValue switch
        {
            "комп практика" => $"Вы сдали комп практику! +{points}",
            "коллок по матану" => $"Вы сдали коллок по матану! +{points}",
            "тысячи" => $"Вы сдали тысячи! +{points}",
            "кр по алгему" => $"Вы написали кр по алгему! +{points}",
            "python task" => $"Вы сделали python task! +{points}",
            "дедлайн по ятп" => $"Вы сделали дедлайн по ятп! +{points}",
            "зачёт по питону" => $"Вы сдали зачёт по питону! +{points}", 
            "экзамен по матану" => $"Вы сдали экзамен по матану! +{points}",
            "экзамен по алгему" => $"Вы сдали экзамен по алгему! +{points}",
            "нтк по философии" => $"Вы написали нтк по философии! +{points}",
            _ => ""
        };

        StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        Debug.Log($"Displaying notification: {message}");
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        var rectTransform = notificationText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(3f);
        notificationText.gameObject.SetActive(false);
    }
}