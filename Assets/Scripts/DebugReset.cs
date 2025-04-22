using UnityEngine;

public class DebugReset : MonoBehaviour
{
	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.R)) return;

		PlayerPrefs.DeleteKey("StoryViewed");
		if (Debug.isDebugBuild)
			Debug.Log("StoryViewed is clear");
	}
}