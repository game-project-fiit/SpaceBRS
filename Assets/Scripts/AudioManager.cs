using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }
	public AudioMixer audioMixer;
	public AudioSource SVXSource;
	private const string KeyMusic = "MusicVolume";
	private const string KeySVX = "SVXVolume";

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			LoadVolumes();
		}
		else
			Destroy(gameObject);
	}

	private void LoadVolumes()
	{
		var music = PlayerPrefs.GetFloat(KeyMusic, 1f);
		var sound = PlayerPrefs.GetFloat(KeySVX, 1f);

		audioMixer.SetFloat("VolMusic", Mathf.Log10(Mathf.Clamp01(music)) * 20f);
		audioMixer.SetFloat("VolSVX", Mathf.Log10(Mathf.Clamp01(sound)) * 20f);
	}

	public void SetMusicVolume(float linear)
	{
		PlayerPrefs.SetFloat(KeyMusic, linear);
		audioMixer.SetFloat("VolMusic", Mathf.Log10(Mathf.Clamp01(linear)) * 20f);
	}

	public void setSVXVolume(float linear)
	{
		PlayerPrefs.SetFloat(KeySVX, linear);
		audioMixer.SetFloat("VolSVX", Mathf.Log10(Mathf.Clamp01(linear)) * 20f);
	}

	public void PlaySVX(AudioClip clip)
	{
		if (!clip || !SVXSource) return;

		var volume = PlayerPrefs.GetFloat(KeySVX, 1f);
		SVXSource.PlayOneShot(clip, volume);
	}
}