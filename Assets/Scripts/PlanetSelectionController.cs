using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using NUnit.Framework.Internal;

public class PlanetController : MonoBehaviour
{
	public List<GameObject> planets;
	public List<Transform> positions;
	public TextMeshProUGUI planetNameText;
	public TextMeshProUGUI bestScoreText;
	public TextMeshProUGUI unavailablePlanetText;
	public AudioClip rotateClip;
	public AudioClip clickClip;

	private readonly Dictionary<string, string> levelScenesByNames = new()
	{
		{ "ВВМ", "VVMLevel" },
		{ "1 семестр", "Term1Level" },
		{ "2 семестр", "Term2Level" }
	};

	private void Start()
	{
		UpdatePlanetPositions();
		PlayerPrefs.SetInt("IsAvailable_VVMLevel", 1);
	}

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
			TryLoadSelectedPlanetLevel();
		}

		if (Input.GetKeyDown(KeyCode.F))
			ResetCurrentPlanetRecord();
	}

	private void TryLoadSelectedPlanetLevel()
	{
		var level = planets[0].name;
		if (levelScenesByNames.TryGetValue(level, out var sceneName))
		{
			if (PlayerPrefs.GetInt($"IsAvailable_{sceneName}", 0) == 1)
				SceneManager.LoadScene(sceneName);
		}
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

		var levelName = planets[0].name;
        planetNameText.text = levelName;

		if (levelScenesByNames.TryGetValue(levelName, out var sceneName))
		{
            var best = PlayerPrefs.GetInt($"BestScore_{sceneName}", 0);
            var available = PlayerPrefs.GetInt($"IsAvailable_{sceneName}", 0);
            unavailablePlanetText.text = available > 0 ? null : "Недоступно!";
			bestScoreText.text = $"Рекорд: {best}";
        }

        else
			bestScoreText.text = "Рекорд: 0";
    }

	private void ResetCurrentPlanetRecord()
	{
        var levelName = planets[0].name;
        planetNameText.text = levelName;

        if (levelScenesByNames.TryGetValue(levelName, out var sceneName))
        {
            var prefsKey = $"BestScore_{sceneName}";
			PlayerPrefs.DeleteKey(prefsKey);
			PlayerPrefs.Save();

            bestScoreText.text = "Рекорд: 0";
        }
    }
}