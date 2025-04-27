using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    public GameObject notificationTextPrefab;
    public Transform notificationContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowNotification(string cometTextValue, int points)
    {
        var message = cometTextValue switch
        {
            "комп практика" => $"Вы сдали комп практику! +{points}",
            "коллок по матану" => $"Вы сдали коллок по матану! +{points}",
            "тысячи" => $"Вы сдали тысячи! +{points}",
            "кр по алгему" => $"Вы написали кр по алгему! +{points}",
            "python task" => $"Вы сделали python task! +{points}",
            "дедлайн по ятп" => $"Вы сделали дедлайн по ятп! +{points}",
            "экзамен по матану" => $"Вы сдали экзамен по матану! +{points}",
            "экзамен по алгему" => $"Вы сдали экзамен по алгему! +{points}",
            "нтк по философии" => $"Вы написали нтк по философии! +{points}",
            "зачёт по питону" => $"Вы сдали зачёт по питону! +{points}",
            _ => ""
        };

        CreateNotification(message);
    }

    private void CreateNotification(string message)
    {
        if (notificationContainer.childCount >= 3)
        {
            Destroy(notificationContainer.GetChild(0).gameObject);
        }
        
        var newNotification = Instantiate(notificationTextPrefab, notificationContainer);
        
        var textComponent = newNotification.GetComponent<TextMeshProUGUI>();
        textComponent.text = message;
        
        for (var i = 0; i < notificationContainer.childCount; i++)
        {
            var rectTransform = notificationContainer.GetChild(i).GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * 7); 
        }
    }
}
