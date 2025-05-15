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
        { 1, new List<string> { "Lego island" } },
        { 2, new List<string> { "Board game" } },
        { 3, new List<string> { "ВВМ Ulearn" } },
        { 4, new List<string> { "первый дедлайн по ятп" } },
        { 5, new List<string> { "ввм кр 1" } },
        { 6, new List<string> { "ввм кр 2" } },
    };

    private readonly Dictionary<string, int>  taskScores = new()
    {
        { "Lego island", 1 },
        { "Board game", 2 },
        { "ВВМ Ulearn", 3 },
        { "первый дедлайн по ятп", 4 },
        { "ввм кр 1", 5 },
        { "ввм кр 2", 6 },
    };

    private void Start()
    {
        transform.localScale = Vector3.one * size;

        cometText = GetComponentInChildren<TextMeshProUGUI>();
        if (cometText != null)
        {
            var randomScore = Random.Range(1, 7); 
            cometText.text = randomScore.ToString();

            cometText.fontSize = 45;
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
            if (ScoreManager.Instance.decreaseScore)
            {
                var cometValue = int.Parse(cometText.text);
                if (cometPoints.TryGetValue(cometValue, out var tasks))
                {
                    var randomTask = tasks[Random.Range(0, tasks.Count)];
                    ScoreManager.Instance.ChangeScore(taskScores[randomTask], false);
                    NotificationManager.Instance.ShowNotification(randomTask, taskScores[randomTask], false);
                }
            }
            return;
        }

        if (cometText != null)
        {
            var offsetX = (transform.position.x < 0) ? 122f : 52f; 
            var offsetY = -32f; 

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
                    ScoreManager.Instance.ChangeScore(taskScores[randomTask], true);
                    NotificationManager.Instance.ShowNotification(randomTask, taskScores[randomTask], true);
                }

                Destroy(bullet.gameObject);
                Destroy(gameObject);
                return;
            }
        }
    }
}
