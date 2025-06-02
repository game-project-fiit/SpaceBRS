using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimator : MonoBehaviour
{
	private Animator animator;
	private Image image;
	private Sprite last;

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		image = GetComponentInChildren<Image>();
		image.preserveAspect = true;
		image.rectTransform.sizeDelta = new(55, 55);
	}

	void Update()
	{
		var controlScheme = HotkeysPanelController.GetControlScheme();
		bool left, right;

		if (controlScheme == "Arrows")
		{
			left = Input.GetKey(KeyCode.LeftArrow);
			right = Input.GetKey(KeyCode.RightArrow);
		}
		else
		{
			left = Input.GetKey(KeyCode.A);
			right = Input.GetKey(KeyCode.D);
		}

		animator.SetBool("isRunning", left || right);

		if (right)
			image.rectTransform.localScale = new(-1, 1, 1);
		else if (left)
			image.rectTransform.localScale = Vector3.one;

		if (Input.GetKeyDown(KeyCode.Space))
			animator.SetTrigger("shoot");
	}
}