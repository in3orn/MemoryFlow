using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PopUpCanvas : MonoBehaviour {

    [SerializeField]
    private float showDuration = 0.5f;

    [SerializeField]
    private float hideDuration = 0.5f;

    private CanvasGroup canvasGroup;
	
	void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = false;
	}

    public void Show()
    {
		canvasGroup.blocksRaycasts = true;
        StartCoroutine(setOpacity(canvasGroup.alpha, 1f, showDuration));
    }

    public void Hide()
    {
		canvasGroup.blocksRaycasts = false;
        StartCoroutine(setOpacity(canvasGroup.alpha, 0f, hideDuration));
    }

    public void ShowImmediately()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void HideImmediately()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    private IEnumerator setOpacity(float from, float to, float duration)
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(from, to, t);

            if (t >= 1) break;
            yield return null;
        }
    }
}
