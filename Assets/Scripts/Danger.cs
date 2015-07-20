
using UnityEngine;
using System.Collections;

public class Danger : MonoBehaviour
{
	public float speedDiff;
	public float volume;
	private Rigidbody rigid;

	// Use this for initialization
	void Start ()
	{

		// cache components
		rigid = GetComponent<Rigidbody> ();

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		// add forward force to the object
		rigid.velocity = new Vector3 (0, 0, 1) * -(speedDiff + GameController.gameSpeed);
	}
}
