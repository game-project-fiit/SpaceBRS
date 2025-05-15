using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class PlanetController : MonoBehaviour
{
	public List<GameObject> planets;
	public List<Transform> positions;
	public TextMeshProUGUI planetNameText;

	private readonly Dictionary<string, string> levelScenesByNames = new()
	{
		{ "ВВМ", "VVMLevel" },
		{ "Term 1", "Term1Level" },
		{ "Term 2", "Term2Level" }
	};

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
			RotateRight();
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			RotateLeft();
		else if (Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Main Menu");
		else if (Input.GetKeyDown(KeyCode.Return))
			LoadSelectedPlanetLevel();
	}

	private void LoadSelectedPlanetLevel()
	{
		var level = planets[0].name;
		if (level == "ВВМ")
		{
			SceneManager.LoadScene("VVMLevel");
		}

		if (level == "Term 1")
		{
			SceneManager.LoadScene("Term1Level");
		}
		
		
		if (level == "Term 2")
		{
			SceneManager.LoadScene("Term2Level");
		}
	}

	private void RotateRight()
	{
		var last = planets[2];
		planets.RemoveAt(2);
		planets.Insert(0, last);
		UpdatePlanetPositions();
	}

	private void RotateLeft()
	{
		var first = planets[0];
		planets.RemoveAt(0);
		planets.Add(first);
		UpdatePlanetPositions();
	}

	private void UpdatePlanetPositions()
	{
		for (var i = 0; i < planets.Count; i++)
			planets[i].transform.position = positions[i].position;

		planetNameText.text = planets[0].name;
	}
}