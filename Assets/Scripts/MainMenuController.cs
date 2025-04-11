using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections; 

public class MainMenuController : MonoBehaviour
{
    public Button[] menuButtons; 
    public TextMeshProUGUI[] menuTexts; 
    public AudioClip moveSound;
    public AudioClip selectSound; 
    public GameObject optionsPanel; 
    private AudioSource audioSource;
    private int selectedIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateMenu();
    }

    void Update()
    {
        if (menuButtons.Length == 0) return; 

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            PlaySound(moveSound);
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            PlaySound(moveSound);
            UpdateMenu();
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlaySound(selectSound);
            SelectOption();
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    void UpdateMenu()
    {
        for (var i = 0; i < menuButtons.Length; i++)
        {
            if (i == selectedIndex)
            {
                menuTexts[i].text = ">" + menuTexts[i].text.TrimStart('>');
                menuButtons[i].Select(); 
            }
            else
            {
                menuTexts[i].text = menuTexts[i].text.TrimStart('>');
            }
        }
    }

    void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Start Game");
                break;
            case 1:
                Debug.Log("Options");
                optionsPanel.SetActive(true); 
                break;
            case 2:
                Debug.Log("Quit Game");
                PlaySound(selectSound); 
                StartCoroutine(QuitGame()); 
                break;
        }
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(selectSound.length/2);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
