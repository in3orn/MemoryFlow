using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Showable))]
public class Field : MonoBehaviour
{
    public static readonly float SIZE = 1.2f;

	public enum TypeEnum
	{
		Horizontal = 0,
		Vertical
	}

	[SerializeField]
	private float setColorDelay = 0f;

	[SerializeField]
	private float setColorDuration = 0.5f;

	[SerializeField]
	private Color defaultColor;

	[SerializeField]
	private Color validColor;

	[SerializeField]
	private Vector2 fallPosition = Vector2.down * 20f;

	[SerializeField]
	private float fallDelay = 0f;

	[SerializeField]
	private float fallDuration = 2f;

	[SerializeField]
	private Vector3 fallRotation = Vector3.back * 30;

	private SpriteRenderer spriteRenderer;

    private Showable showable;

	private bool valid;

	public bool Valid { get { return valid; } }

	private bool broken;

	public bool Broken { get { return broken; } }

	public void Init(Vector2 position, bool valid)
    {
		transform.position = position;
		this.valid = valid;
		spriteRenderer.color = getInitColor();
		broken = false;
	}

    void Awake()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
        showable = GetComponent<Showable>();
    }

	public void Break()
	{
		broken = true;
		StartCoroutine(fall());
	}

    public void Show()
    {
        showable.Show();
    }

    public void Hide()
    {
        showable.Hide();
    }

	public void Mask()
	{
		StartCoroutine (setColor (spriteRenderer.color, defaultColor, setColorDelay, setColorDuration));
	}

	public void Unmask()
	{
		StartCoroutine (setColor (spriteRenderer.color, validColor, setColorDelay, setColorDuration));
	}

	private IEnumerator setColor(Color from, Color to, float delay, float duration)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}

		float t = 0f;
		while (true)
		{
			t += Time.deltaTime / duration;
			spriteRenderer.color = Color.Lerp(from, to, t);

			if (t >= 1) break;
			yield return null;
		}
	}

	private IEnumerator fall()
	{
		Vector2 from = transform.position;
		Vector2 to = from + fallPosition;

		float t = 0f;
		while (true)
		{
			t += Time.deltaTime / fallDuration;
			transform.position = Vector2.Lerp(from, to, t);
			transform.Rotate (fallRotation);

			if (t >= 1) break;
			yield return null;
		}
	}

	private Color getInitColor()
	{
		Color color = valid ? validColor : defaultColor;
		color.a = 0;

		return color;
	}
}
