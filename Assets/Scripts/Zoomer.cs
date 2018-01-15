using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Zoomer : MonoBehaviour {

	[SerializeField]
	private float zoomDelay = 0f;

	[SerializeField]
	private float zoomDuration = 2f;

	[SerializeField]
	private float initialZoom = 4f;

	[SerializeField]
	private float zoomFactor = 0.5f;

	[SerializeField]
	private Level level;

	private Camera myCamera;

	void Awake () {
        myCamera = GetComponent<Camera> ();
	}

	void Start() {
		level.OnStarted += adapt;
	}

	private void adapt() {
		int size = Mathf.Max (level.HorizontalLength, level.VerticalLength);
		SetZoom (initialZoom + size * zoomFactor);
	}

	public void SetZoom (float zoom) {
		StartCoroutine(setZoom(myCamera.orthographicSize, zoom, zoomDelay, zoomDuration));
	}

	private IEnumerator setZoom(float from, float to, float delay, float duration) {
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}

		float t = 0f;
		while (true)
		{
			t += Time.deltaTime / duration;
            myCamera.orthographicSize = Mathf.Lerp(from, to, t);

			if (t >= 1) break;
			yield return null;
		}
	}
}
