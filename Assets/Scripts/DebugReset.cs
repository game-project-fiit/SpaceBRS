using UnityEngine;

public class DebugReset : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey("StoryViewed");
            Debug.Log("StoryViewed is clear");
        }
    }
}
