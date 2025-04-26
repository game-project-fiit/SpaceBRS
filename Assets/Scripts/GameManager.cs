using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 15f; 
    public TextMeshProUGUI resultText; 
    public TextMeshProUGUI descriptionText; 
    public CometSpawner cometSpawner; 

    private void Start()
    {
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration); 
        EndGame();
    }

    private void EndGame()
    {
        var finalScore = ScoreManager.Instance.GetScore();
        
        var resultMessage = finalScore switch
        {
            < 40 => "Game Over",
            >= 40 and < 60 => "Удовлетворительно",
            >= 60 and < 80 => "Хорошо",
            _ => "Отлично"
        };
        
        resultText.text = resultMessage;
        resultText.gameObject.SetActive(true);
        
        descriptionText.text = GetDescription(finalScore);
        descriptionText.gameObject.SetActive(true); 

        if (cometSpawner != null)
        {
            cometSpawner.StopSpawning();
        }
        
        StartCoroutine(QuitGameAfterDelay(2f));
    }

    private static string GetDescription(int score)
    {
        return score switch
        {
            < 40 => "Ты отчислен! Можешь попробовать перепоступить в следующем году или пойти в УГИ, IT - не твоё",
            >= 40 and < 60 => "Ну хоть закрылся! Но стипендии тебе не видать, поздравляю! Ещё и из общаги выселят. Больше не говори на лекциях, что тебе очев",
            >= 60 and < 80 => "Минус повышка, но ты на верном пути! Съешь сэндвич из автомата и не грусти:)",
            _ => "ОГО! Ты даже матан сдал на отлично? Что ж, наше почтение!"
        };
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
