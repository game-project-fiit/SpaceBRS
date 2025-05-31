using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class PlanetController : MonoBehaviour
{
	public List<GameObject> planets;
	public List<Transform> positions;
	public TextMeshProUGUI planetNameText;
	public AudioClip rotateClip;
	public AudioClip clickClip;


	private readonly Dictionary<string, string> levelScenesByNames = new()
	{
		{ "ВВМ", "VVMLevel" },
		{ "1 семестр", "Term1Level" },
		{ "2 семестр", "Term2Level" }
	};

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			RotateRight();
			AudioManager.Instance.PlaySVX(rotateClip);
		}

		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			RotateLeft();
			AudioManager.Instance.PlaySVX(rotateClip);
		}

		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			AudioManager.Instance.PlaySVX(clickClip);
			SceneManager.LoadScene("Main Menu");
		}

		else if (Input.GetKeyDown(KeyCode.Return))
		{
			AudioManager.Instance.PlaySVX(clickClip);
			LoadSelectedPlanetLevel();
		}
	}

	private void LoadSelectedPlanetLevel()
	{
		var level = planets[0].name;
		if (levelScenesByNames.TryGetValue(level, out var sceneName))
			SceneManager.LoadScene(sceneName);
		else
			Debug.LogError($"No scene found for level: {level}");
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