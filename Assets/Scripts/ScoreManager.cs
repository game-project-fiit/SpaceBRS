// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class ScoreManager : MonoBehaviour
// {
//     public Text scoreText; // Ссылка на UI элемент для отображения очков
//     private int score = 100; // Начальное количество очков
//
//     void Start()
//     {
//         UpdateScoreText(); // Обновляем текст при старте
//     }
//
//     public void DecreaseScore(int amount)
//     {
//         score -= amount; // Уменьшаем счет
//         UpdateScoreText(); // Обновляем текст счета
//     }
//
// void UpdateScoreText()
// {
//     if (scoreText != null)
//         scoreText.text = "Score: " + score.ToString();
// }
//     
//     public void DecreaseScoreAfterTime(float time, int amount)
//     {
//         StartCoroutine(DecreaseScoreCoroutine(time, amount));
//     }
//
//     private IEnumerator DecreaseScoreCoroutine(float time, int amount)
//     {
//         yield return new WaitForSeconds(time); // Ждем указанное время
//         DecreaseScore(amount); // Уменьшаем счет
//     }
// }

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Синглтон
    private int score = 0;
    private AudioSource audioSource;
    public AudioClip scoreSound;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Awake()
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

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    public void IncreaseScore(int amount)
    {
        score += amount; 
        UpdateScoreText();
        PlaySound(scoreSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
    
    public int GetScore()
    {
        return score; 
    }

}