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
        image.rectTransform.sizeDelta = new Vector2(55, 55);
    }
     

    void Update()
    {
        var left = Input.GetKey(KeyCode.A);
        var right = Input.GetKey(KeyCode.D);
        var run = left || right;

        animator.SetBool("isRunning", run);

        if (right)
            image.rectTransform.localScale = new Vector3(-1, 1, 1);
        else if (left)
            image.rectTransform.localScale = Vector3.one;

        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("shoot");
    }
}
