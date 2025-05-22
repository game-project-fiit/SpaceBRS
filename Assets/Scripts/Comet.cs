using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comet : MonoBehaviour
{
    public RectTransform planetRect;
    public TextMeshProUGUI cometText;
    public float cometScreenRadius = 20f;
    public float bulletScreenRadius = 10f;
    public float size = 0.1f;

    private Dictionary<int, List<string>> currentCometPoints = new();
    private Dictionary<string, int> currentTaskScores = new();

    private readonly Dictionary<int, List<string>> cometPointsVvm = new()
    {
        { 1, new List<string> { "Lego island" } },
        { 2, new List<string> { "Board game" } },
        { 3, new List<string> { "ВВМ Ulearn" } },
        { 4, new List<string> { "первый дедлайн по ятп" } },
        { 5, new List<string> { "ввм кр 1" } },
        { 6, new List<string> { "ввм кр 2" } },
    };

    private readonly Dictionary<string, int> taskScoresVvm = new()
    {
        { "Lego island", 1 },
        { "Board game", 2 },
        { "ВВМ Ulearn", 3 },
        { "первый дедлайн по ятп", 4 },
        { "ввм кр 1", 5 },
        { "ввм кр 2", 6 },
    };

    private readonly Dictionary<int, List<string>> cometPointsTerm1 = new()
    {
        { 1, new List<string> { "тысячи" } },
        { 2, new List<string> { "комп практика", "нтк по английскому", "нтк по орг" } },
        { 3, new List<string> { "дедлайн по ятп", "сдача дз по Nand2Tetris" } },
        { 4, new List<string> { "коллок по матану" } },
        { 5, new List<string> { "первая кр по алгему", "вторая кр по алгему" } },
        { 6, new List<string> { "экзамен по алгему", "экзамен по матану", "экзамен по ятп" } },
    };

    private readonly Dictionary<string, int> taskScoresTerm1 = new()
    {
        { "тысячи", 1 },
        { "комп практика", 2 },
        { "нтк по английскому", 2 },
        { "нтк по орг", 2 },
        { "дедлайн по ятп", 3 },
        { "сдача дз по Nand2Tetris", 3 },
        { "коллок по матану", 4 },
        { "первая кр по алгему", 5 },
        { "вторая кр по алгему", 5 },
        { "экзамен по алгему", 6 },
        { "экзамен по матану", 6 },
        { "экзамен по ятп", 6 },
    };

    private readonly Dictionary<int, List<string>> cometPointsTerm2 = new()
    {
        { 1, new List<string> { "тысячи" } },
        { 2, new List<string> { "комп практика", "нтк по английскому", "нтк по философии" } },
        { 3, new List<string> { "дедлайн по ятп", "сдача дз по Nand2Tetris", "python task" } },
        { 4, new List<string> { "первый коллок по матану", "зачет по питону" } },
        { 5, new List<string> { "второй коллок по матану", "первая кр по алгему", "вторая кр по алгему", "ДКР" } },
        { 6, new List<string> { "экзамен по алгему", "экзамен по матану", "экзамен по ятп" } },
    };

    private readonly Dictionary<string, int> taskScoresTerm2 = new()
    {
        { "тысячи", 1 },
        { "комп практика", 2 },
        { "нтк по английскому", 2 },
        { "нтк по философии", 2 },
        { "дедлайн по ятп", 3 },
        { "сдача дз по Nand2Tetris", 3 },
        { "python task", 3 },
        { "первый коллок по матану", 4 },
        { "зачет по питону", 4 },
        { "первая кр по алгему", 5 },
        { "вторая кр по алгему", 5 },
        { "второй коллок по матану", 5 },
        { "ДКР", 5 },
        { "экзамен по алгему", 6 },
        { "экзамен по матану", 6 },
        { "экзамен по ятп", 6 },
    };

    private void Start()
    {
        transform.localScale = Vector3.one * size;

        cometText = GetComponentInChildren<TextMeshProUGUI>();
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "VVMLevel")
        {
            currentCometPoints = cometPointsVvm;
            currentTaskScores = taskScoresVvm;
        }
        
        if (sceneName == "Term1Level")
        {
            currentCometPoints = cometPointsTerm1;
            currentTaskScores = taskScoresTerm1;
        }
        
        if (sceneName == "Term2Level")
        {
            currentCometPoints = cometPointsTerm2;
            currentTaskScores = taskScoresTerm2;
        }
        
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
                if (currentCometPoints.TryGetValue(cometValue, out var tasks))
                {
                    var randomTask = tasks[Random.Range(0, tasks.Count)];
                    ScoreManager.Instance.ChangeScore(currentTaskScores[randomTask], false);
                    NotificationManager.Instance.ShowNotification(randomTask, currentTaskScores[randomTask], false);
                }
            }

            return;
        }

        if (cometText != null)
        {
            var offsetX = (transform.position.x < 0) ? 122f : 52f;
            var offsetY = -32f;

            cometText.transform.position =
                Camera.main.WorldToScreenPoint(transform.position) + new Vector3(offsetX, offsetY, 0);
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
                if (currentCometPoints.TryGetValue(cometValue, out var tasks))
                {
                    var randomTask = tasks[Random.Range(0, tasks.Count)];
                    ScoreManager.Instance.ChangeScore(currentTaskScores[randomTask], true);
                    NotificationManager.Instance.ShowNotification(randomTask, currentTaskScores[randomTask], true);
                }

                Destroy(bullet.gameObject);
                Destroy(gameObject);
                return;
            }
        }
    }
}