using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class PlanetController : MonoBehaviour
{
    public List<GameObject> planets;
    public List<Transform> positions;
    public TextMeshProUGUI planetNameText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            RotateRight();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            RotateLeft();
        else if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");
    }

    void RotateRight()
    {
        GameObject last = planets[2];
        planets.RemoveAt(2);
        planets.Insert(0, last);
        UpdatePlanetPositions();
    }

    void RotateLeft()
    {
        GameObject first = planets[0];
        planets.RemoveAt(0);
        planets.Add(first);
        UpdatePlanetPositions();
    }

    void UpdatePlanetPositions()
    {
        for (int i = 0; i < planets.Count; i++)
            planets[i].transform.position = positions[i].position;
        planetNameText.text = planets[0].name;
    }
}
