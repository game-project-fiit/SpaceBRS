using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
	public static NotificationManager instance;
	public GameObject notificationTextPrefab;
	public Transform notificationContainer;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

	public void ShowNotification(string cometTextValue, int score, bool positive)
		=> CreateNotification(
			$"Вы {(positive ? "сдали" : "не сдали")} " +
			$"{cometTextValue} {(positive ? "+" : "-")}{score}!");

	private void CreateNotification(string message)
	{
		if (notificationContainer.childCount >= 3)
			Destroy(notificationContainer.GetChild(0).gameObject);

		var newNotification = Instantiate(notificationTextPrefab, notificationContainer);
		var textComponent = newNotification.GetComponent<TextMeshProUGUI>();
		textComponent.text = message;

		for (var i = 0; i < notificationContainer.childCount; i++)
		{
			var rectTransform = notificationContainer.GetChild(i).GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new(0, -i * 7);
		}
	}
}