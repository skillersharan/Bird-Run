using UnityEngine;
using System.Collections;

public class CamColor : MonoBehaviour
{
	private Camera cam;
	private Color color1 = Color.red;
	private Color color2 = Color.yellow;
	public Color[] CamColour;

	private float t = 0;
	private short i = 1;
	private float prevTime;

	public int RainColor;

	public GameObject Rain;
	public float duration = 3.0F;
	public float smooth = 3f;
	// Use this for initialization
	void Start ()
	{
		cam = GetComponent<Camera> ();
		if (CamColour.Length == 0)
			return;
		color1 = CamColour [0];
		color2 = CamColour [1];
		cam.backgroundColor = color1;
		prevTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (CamColour.Length == 0)
			return;

		float currentTime = Time.time;
		t = (currentTime - prevTime) / duration;
		if (t > 1) {
			prevTime = currentTime;
			t = 0;
			color1 = color2;
			i = (short)((i + 1) % CamColour.Length);
			color2 = CamColour [i];
		}
		if (i == RainColor || i == RainColor + 1) {
			Rain.gameObject.SetActive (true);
		} else {
			Rain.gameObject.SetActive (false);
		}
		cam.backgroundColor = Color.Lerp (color1, color2, t);
	}
}