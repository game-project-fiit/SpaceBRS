using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI[] menuItems;
    private int selectedIndex = 0;

    private void Start()
    {
        UpdateMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)
        || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)
        || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuItems.Length;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectOption();
        }
    }

    private void UpdateMenu()
    {
        for (var i = 0; i < menuItems.Length; i++)
        {
            if (i == selectedIndex)
                menuItems[i].text = "> " + menuItems[i].text.Replace("> ", "");
            else
                menuItems[i].text = menuItems[i].text.Replace("> ", "");
        }
    }

    private void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Start Game");
                break;
            case 1:
                Debug.Log("Options");
                break;
            case 2:
                Debug.Log("Quit Game");
                Application.Quit();
                break;
        }
    }
}