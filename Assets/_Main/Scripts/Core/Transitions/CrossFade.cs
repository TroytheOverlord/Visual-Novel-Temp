using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CrossFade : SceneTransition
{
    [Header("Fade Settings")]
    public CanvasGroup canvasGroup; // Reference to the CanvasGroup for the crossfade effect
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Awake()
    {
        // Ensure the CanvasGroup is initialized properly
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public override IEnumerator AnimateTransitionIn()
    {
        canvasGroup.alpha = 0f; // Start with a completely transparent canvas
        var tweener = canvasGroup.DOFade(1f, fadeDuration); // Fade to opaque
        yield return tweener.WaitForCompletion(); // Wait until the fade is complete
    }

    public override IEnumerator AnimateTransitionOut()
    {
        var tweener = canvasGroup.DOFade(0f, fadeDuration); // Fade back to transparent
        yield return tweener.WaitForCompletion(); // Wait until the fade is complete
    }
}

