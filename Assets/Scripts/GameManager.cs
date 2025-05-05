using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 5f;
    public CometSpawner cometSpawner;
    public Slider timeSlider;
    public Image[] resultImages;
    public TextMeshProUGUI tapEnterText;
    public AudioClip gameOverSound;
    public AudioClip victorySound;
    private AudioSource audioSource;
    private bool isGameActive = false;
    private bool isGameOver = false; // Новый флаг для отслеживания состояния окончания игры

    private void Start()
    {
        timeSlider.maxValue = gameDuration;
        timeSlider.value = gameDuration;

        foreach (var img in resultImages)
        {
            img.gameObject.SetActive(false);
        }

        tapEnterText.gameObject.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartGame()
    {
        isGameActive = true;
        isGameOver = false; // Сбрасываем флаг при старте игры
        StartCoroutine(GameTimer());
        if (cometSpawner != null)
        {
            cometSpawner.StartSpawning();
        }
    }

    private IEnumerator GameTimer()
    {
        var elapsedTime = 0f;
        while (elapsedTime < gameDuration && isGameActive)
        {
            elapsedTime += Time.deltaTime;
            timeSlider.value = gameDuration - elapsedTime;
            if (ScoreManager.Instance.GetScore() >= 100)
            {
                EndGameWithAutomaticWin();
                yield break;
            }

            yield return null;
        }

        if (isGameActive)
        {
            EndGame();
        }
    }

    private void Update()
    {
        if (isGameActive && Input.GetKeyDown(KeyCode.Return))
        {
            EndGame();
        }

        // Проверка нажатия Enter для выхода из игры только если игра окончена
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            ExitGame();
        }
    }

    private void EndGame()
    {
        isGameActive = false;
        isGameOver = true; // Устанавливаем флаг окончания игры
        var finalScore = ScoreManager.Instance.GetScore();
        var resultIndex = finalScore switch
        {
            < 40 => 0,
            >= 40 and < 60 => 1,
            >= 60 and < 80 => 2,
            _ => 3
        };

        foreach (var img in resultImages)
        {
            img.gameObject.SetActive(false);
        }

        resultImages[resultIndex].gameObject.SetActive(true);
        tapEnterText.gameObject.SetActive(true);
        tapEnterText.text = "Tap Enter"; // Устанавливаем текст

        if (finalScore < 40)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
        else
        {
            audioSource.PlayOneShot(victorySound);
        }

        if (cometSpawner != null)
        {
            cometSpawner.StopSpawning();
        }
    }

    private void EndGameWithAutomaticWin()
    {
        isGameActive = false;
        isGameOver = true; // Устанавливаем флаг окончания игры
        foreach (var img in resultImages)
        {
            img.gameObject.SetActive(false);
        }

        resultImages[4].gameObject.SetActive(true);
        tapEnterText.gameObject.SetActive(true);
        tapEnterText.text = "Tap Enter";
        audioSource.PlayOneShot(victorySound);
        if (cometSpawner != null)
        {
            cometSpawner.StopSpawning();
        }
    }

    private void ExitGame()
    {
        // Завершение игры
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Остановка игры в редакторе
#endif
    }
}
