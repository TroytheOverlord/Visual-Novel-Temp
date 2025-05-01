using System.Collections;
using UnityEngine;
using Dialogue;

public class CanvasGroupController
{
    private const float DEFAULT_FADE_SPEED = 3f;

    private MonoBehaviour owner;
    private CanvasGroup rootCG;

    private Coroutine co_showing = null;
    private Coroutine co_hiding = null;

    public bool isShowing => co_showing != null;
    public bool isHiding => co_hiding != null;
    public bool isFading => isShowing || isHiding;
    public bool isVisible => co_showing != null || rootCG.alpha > 0;
    public float alpha { get { return rootCG.alpha; } set {rootCG.alpha = value;} }

    public CanvasGroupController(MonoBehaviour owner, CanvasGroup rootCG)
    {
        this.owner = owner;
        this.rootCG = rootCG;
    }

    public Coroutine Show(float speed = 1f, bool immediate = false)
    {
        if (isShowing) return co_showing;

        if (isHiding)
        {
            owner.StopCoroutine(co_hiding);
            co_hiding = null;
        }

        co_showing = owner.StartCoroutine(Fading(1, speed, immediate));
        return co_showing;
    }

    public Coroutine Hide(float speed = 1f, bool immediate = false)
    {
        if (isHiding) return co_hiding;

        if (isShowing)
        {
            owner.StopCoroutine(co_showing);
            co_showing = null;
        }

        co_hiding = owner.StartCoroutine(Fading(0, speed, immediate));
        return co_hiding;
    }

    private IEnumerator Fading(float targetAlpha, float speed, bool immediate)
    {
        CanvasGroup cg = rootCG;

        if (immediate)
        {
            cg.alpha = targetAlpha;
        }
        else
        {
            while (!Mathf.Approximately(rootCG.alpha, targetAlpha))
            {
                cg.alpha = Mathf.MoveTowards(cg.alpha, targetAlpha, Time.deltaTime * DEFAULT_FADE_SPEED * speed);
                yield return null;
            }
            
        }

        co_showing = null;
        co_hiding = null;
    }

    public void SetInteractableState(bool active)
    {
        rootCG.interactable = active;
        rootCG.blocksRaycasts = active;
    }
}


