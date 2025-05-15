using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Comet : MonoBehaviour
{
    public RectTransform planetRect;
    public TextMeshProUGUI cometText;
    public float cometScreenRadius = 20f;
    public float bulletScreenRadius = 10f;
    public float size = 0.1f;

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
        transform.localScale = Vector3.one * size;

        cometText = GetComponentInChildren<TextMeshProUGUI>();
        if (cometText != null)
        {
            var randomScore = Random.Range(1, 7); 
            cometText.text = randomScore.ToString();

            cometText.fontSize = 48;
            cometText.color = Color.black;

            var material = cometText.fontMaterial;
            material.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
            material.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.13f);
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
            var offsetX = (transform.position.x < 0) ? 127f : 45f; 
            var offsetY = -35f; 

            cometText.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(offsetX, offsetY, 0);
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
                var cometValue = int.Parse(cometText.text);
                if (cometPoints.TryGetValue(cometValue, out var tasks))
                {
                    var randomTask = tasks[Random.Range(0, tasks.Count)];
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
