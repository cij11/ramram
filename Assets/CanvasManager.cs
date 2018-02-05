using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    CanvasGroup canvasGroup;
    Canvas canvas;
    float fadeTime = 1.0f;
    float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
        this.canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
        this.canvas = GetComponent<Canvas>() as Canvas;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetVisible(bool isVisible)
    {
        this.elapsedTime = 0.0f;

        if(isVisible)
        {
            StartCoroutine("DoFadeIn");
        } else
        {
            StartCoroutine("DoFadeOut");
        }
    }

    IEnumerator DoFadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1.0f - (elapsedTime / fadeTime));
            yield return null;
        }
    }

    IEnumerator DoFadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
            yield return null;
        }
    }
}
