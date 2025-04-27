using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; 
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