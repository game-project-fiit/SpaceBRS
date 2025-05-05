using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 5f; 
    public Image resultImage; // Изображение для результата
    public CometSpawner cometSpawner; 
    public Slider timeSlider; 

    public Image[] resultImages; // Массив изображений для разных результатов

    private void Start()
    {
        timeSlider.maxValue = gameDuration; 
        timeSlider.value = gameDuration; 
        
        // Скрываем все изображения результата при старте
        foreach (var img in resultImages)
        {
            img.gameObject.SetActive(false);
        }
    }

    public void StartGame() 
    {
        StartCoroutine(GameTimer());
        if (cometSpawner != null)
        {
            cometSpawner.StartSpawning(); 
        }
    }

    private IEnumerator GameTimer()
    {
        var elapsedTime = 0f; 
        while (elapsedTime < gameDuration)
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
        EndGame();
    }

    private void EndGame()
    {
        var finalScore = ScoreManager.Instance.GetScore();
        
        // Получаем индекс изображения результата
        int resultIndex = finalScore switch
        {
            < 40 => 0, // Индекс для "Game Over"
            >= 40 and < 60 => 1, // Индекс для "Удовлетворительно"
            >= 60 and < 80 => 2, // Индекс для "Хорошо"
            _ => 3 // Индекс для "Отлично"
        };

        // Скрываем все изображения перед активацией нужного
        foreach (var img in resultImages)
        {
            img.gameObject.SetActive(false);
        }

        // Активируем нужное изображение
        resultImages[resultIndex].gameObject.SetActive(true);
        
        if (cometSpawner != null)
        {
            cometSpawner.StopSpawning();
        }
        
        StartCoroutine(QuitGameAfterDelay(5f));
    }

    private void EndGameWithAutomaticWin()
    {
        // Скрываем все изображения перед активацией нужного
        foreach (var img in resultImages)
        {
            img.gameObject.SetActive(false);
        }

        // Активируем изображение для "Автомат"
        resultImages[4].gameObject.SetActive(true);
        
        if (cometSpawner != null)
        {
            cometSpawner.StopSpawning();
        }
        
        StartCoroutine(QuitGameAfterDelay(5f));
    }

    private IEnumerator QuitGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }
}
