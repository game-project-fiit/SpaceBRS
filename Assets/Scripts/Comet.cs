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

    private readonly Dictionary<int, List<string>> cometPoints = new()
    {
        { 1, new List<string> { "тысячи", "нтк по философии" } },
        { 2, new List<string> { "комп практика" } },
        { 3, new List<string> { "python task", "дедлайн по ятп" } },
        { 4, new List<string> { "кр по алгему", "зачёт по питону" } },
        { 5, new List<string> { "коллок по матану" } },
        { 6, new List<string> { "экзамен по матану", "экзамен по алгему" } },
    };

    private readonly Dictionary<string, int> taskScores = new()
    {
        { "тысячи", 1 },
        { "нтк по философии", 1 },
        { "комп практика", 2 },
        { "python task", 3 },
        { "дедлайн по ятп", 3 },
        { "кр по алгему", 4 },
        { "зачёт по питону", 4 },
        { "коллок по матану", 5 },
        { "экзамен по матану", 6 },
        { "экзамен по алгему", 6 },
    };

    private void Start()
    {
        cometText = GetComponentInChildren<TextMeshProUGUI>();
        if (cometText != null)
        {
            int randomScore = Random.Range(1, 7); // Случайное число от 1 до 6
            cometText.text = randomScore.ToString();
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
                int cometValue = int.Parse(cometText.text);
                if (cometPoints.TryGetValue(cometValue, out var tasks))
                {
                    string randomTask = tasks[Random.Range(0, tasks.Count)];
                    ScoreManager.Instance.IncreaseScore(taskScores[randomTask]);
                    NotificationManager.Instance.ShowNotification(randomTask, taskScores[randomTask]);
                }

                Destroy(bullet.gameObject);
                Destroy(gameObject);
                return;
            }
        }
    }
}
