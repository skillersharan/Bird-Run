using UnityEngine;
using System.Collections;

public class BonusItem : MonoBehaviour
{
	public float speedDiff;
	public float destroySeconds;
	public AudioClip player;
	public float volume;

	public float staminaBonus = 0.25f;
	public float scoreBonus = 10f;

	private Rigidbody rigid;
	private AudioSource audioSrc;
	private Renderer rend;
	private Collider col;

	// Use this for initialization
	void Start ()
	{
		// cache components
		rigid = GetComponent<Rigidbody> ();
		audioSrc = GetComponent<AudioSource> ();
		rend = GetComponent<Renderer> ();
		col = GetComponent<Collider> ();

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		// add forward force to the object
		rigid.velocity = new Vector3 (0, 0, 1) * -(speedDiff + GameController.gameSpeed);

		// apply hover effect to this game object
		rigid.position = new Vector3 (rigid.position.x, rigid.position.y, rigid.position.z);
	}

	void Destruction ()
	{
		// play destruction sonud
		audioSrc.volume = volume;
		audioSrc.Play ();

		// disable render and collider
		rend.enabled = false;
		col.enabled = false;
	}

	void OnTriggerEnter (Collider coll)
	{
		if (coll.gameObject.tag == "Player") {
			// play clip for player collision
			audioSrc.clip = player;

			// when player collects, award by
			// increasing the game score by 10 points
			GameController.gameScore += scoreBonus;
			if (GameController.stamina + staminaBonus <= 1)
				GameController.stamina = (GameController.stamina + staminaBonus);
			else {
				GameController.stamina = 1;
			}

			// run desctuion function
			Destruction ();
		}
	}
}