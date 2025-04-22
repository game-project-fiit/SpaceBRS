using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
	public GameObject[] slides;
	public int currentSlideIndex;
	public CanvasGroup blackOverlay;
	public float fadeDuration = 1f;
	public bool replayFromOptions;
	private bool isTransitioning;

	private void Start()
	{
		ShowSlide(currentSlideIndex);
		StartCoroutine(InitialFadeTransition());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
			NextSlide();
		if (Input.GetKeyDown(KeyCode.Escape))
			EndStory();
	}

	private void ShowSlide(int index)
	{
		foreach (GameObject slide in slides)
			slide.SetActive(false);

		if (index < 0 || index >= slides.Length) return;

		slides[index].SetActive(true);
		var canvasGroup = slides[index].GetComponent<CanvasGroup>();

		if (canvasGroup == null)
			canvasGroup = slides[index].AddComponent<CanvasGroup>();

		canvasGroup.alpha = 1f;
	}

	private IEnumerator InitialFadeTransition()
	{
		isTransitioning = true;
		blackOverlay.gameObject.SetActive(true);
		blackOverlay.alpha = 0f;

		yield return StartCoroutine(FadeCanvasGroup(blackOverlay, 0f, 1f, fadeDuration));

		currentSlideIndex++;
		if (currentSlideIndex < slides.Length)
		{
			ShowSlide(currentSlideIndex);
			var nextSlideCG = slides[currentSlideIndex].GetComponent<CanvasGroup>();

			if (nextSlideCG == null)
				nextSlideCG = slides[currentSlideIndex].AddComponent<CanvasGroup>();

			nextSlideCG.alpha = 0f;
			yield return StartCoroutine(CrossFadeCanvasGroup(blackOverlay, nextSlideCG, fadeDuration));
		}
		else
		{
			EndStory();
			yield break;
		}

		isTransitioning = false;
	}

	private IEnumerator CrossFadeCanvasGroup(CanvasGroup blackCG, CanvasGroup slideCG, float duration)
	{
		var elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			var t = elapsed / duration;
			blackCG.alpha = Mathf.Lerp(1f, 0f, t);
			slideCG.alpha = Mathf.Lerp(0f, 1f, t);

			yield return null;
		}

		blackCG.alpha = 0f;
		slideCG.alpha = 1f;
	}

	private void NextSlide() =>
		StartCoroutine(TransitionToNextSlide());

	private IEnumerator TransitionToNextSlide()
	{
		isTransitioning = true;
		var currentCG = slides[currentSlideIndex].GetComponent<CanvasGroup>();

		if (currentCG == null)
			currentCG = slides[currentSlideIndex].AddComponent<CanvasGroup>();

		yield return StartCoroutine(FadeCanvasGroup(currentCG, 1f, 0f, fadeDuration / 2));

		currentSlideIndex++;
		if (currentSlideIndex < slides.Length)
		{
			ShowSlide(currentSlideIndex);
			var nextCG = slides[currentSlideIndex].GetComponent<CanvasGroup>();

			if (nextCG == null)
				nextCG = slides[currentSlideIndex].AddComponent<CanvasGroup>();

			nextCG.alpha = 0f;
			yield return StartCoroutine(FadeCanvasGroup(nextCG, 0f, 1f, fadeDuration / 2));
		}

		else
			EndStory();

		isTransitioning = false;
	}

	private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
	{
		var elapsed = 0f;
		cg.alpha = start;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			cg.alpha = Mathf.Lerp(start, end, elapsed / duration);

			yield return null;
		}

		cg.alpha = end;
	}

	private void EndStory()
	{
		if (!gameObject.activeInHierarchy)
			return;

		if (replayFromOptions)
		{
			replayFromOptions = false;
			currentSlideIndex = 0;
			gameObject.SetActive(false);
			SettingsMenuController.instance?.ReturnToOptionsAfterPlot();
			enabled = false;
			Debug.Log("Replay ended, returning to options");
		}

		else
		{
			PlayerPrefs.SetInt("StoryViewed", 1);
			PlayerPrefs.Save();
			SceneManager.LoadScene("LevelsMenu");
		}
	}
}