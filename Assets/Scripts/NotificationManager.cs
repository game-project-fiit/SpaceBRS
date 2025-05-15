using System.Collections;
using System.Collections.Generic;
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

    public void ShowNotification(string cometTextValue, int score, bool positive)
    {
        var message = string.Format("Вы {0} {1} {2}{3}!",
            positive ? "сдали" : "не сдали",
            cometTextValue,
            positive ? "+" : "-",
            score);

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
