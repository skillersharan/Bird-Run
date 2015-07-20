using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float bottomMax;
	public float motionSensitivity = 2f;
	public bool invertYAxis;
	public float boundsWidth;
	public float minBoundsHeight;
	public float maxBoundsHeight;

	private float verticalFlying;
	public float upDownDelta = 0.05f;

	public float speed;
	public float tilt;
	public float bounce;
	public float bounceSpeed;

	public float fireDistance;
	public AudioClip audioExplosion;
	public float volume = 0.1f;

	private Vector3 defaultPos;
	private float inputHorizontal;
	private float inputVertical;
	private float seconds;

	private Rigidbody rigid;
	private AudioSource audioSrc;
	private Renderer rend;
	public GameObject PlayerMesh;
	public BoxCollider col1;
	public BoxCollider col2;

	public ParticleSystem explosion;


	IEnumerator GameoverWait (float f)
	{
		GameObject exp = Instantiate (explosion, transform.position, transform.rotation) as GameObject;
		yield return new WaitForSeconds (f);
		Destroy (exp);
		GameController.changeState = true;
	}

	void Start ()
	{
		// cache components
		rigid = GetComponent<Rigidbody> ();
		audioSrc = GetComponent<AudioSource> ();
		rend = PlayerMesh.GetComponent<Renderer> ();
		//col = GetComponent<Collider> ();

		// fetch default position from our inspector
		defaultPos = transform.position;
	}

	void Update ()
	{
		// grabs input in update loop for best accuracy
		PlayerInput ();

		// based on game states, do something...
		GameStates gameState = GameController.gameState;
		if (gameState == GameStates.GamePlay) {
			// disable renderer and collider
			if (!rend.enabled)
				rend.enabled = true;
			if (!col1.enabled)
				col1.enabled = true;
			if (!col2.enabled)
				col2.enabled = true;
		} else {
			// disable renderer and collider
			if (rend.enabled)
				rend.enabled = false;
			if (col1.enabled)
				col1.enabled = false;
			if (col2.enabled)
				col2.enabled = false;
			// reset player position to default
			transform.position = defaultPos;
		}
	}

	void FixedUpdate ()
	{
		// run movement function to handle rigidbody physics
		Movement ();
	}

	void PlayerInput ()
	{


		// fetch our input for movememnt
		if (Application.platform == RuntimePlatform.Android) {


			inputHorizontal = Input.acceleration.x * motionSensitivity;
			inputVertical = Input.acceleration.y * motionSensitivity;
		} else {
			inputHorizontal = Input.GetAxis ("Horizontal");
			inputVertical = Input.GetAxis ("Vertical");
		}

		if (invertYAxis) {
			inputVertical *= -1;
		}
	}

	void Movement ()
	{
		// update player velocity
		Vector3 input = new Vector3 (inputHorizontal, inputVertical, 0.0f);
		rigid.velocity = input * speed;

		// create a hover effect using a sin wave
		//float bounceY = rigid.position.y + bounce * Mathf.Sin (bounceSpeed * Time.time);

		// apply hover effect, and clamp player within bounds
		rigid.position = new Vector3 (Mathf.Clamp (rigid.position.x, -boundsWidth, boundsWidth),
		                              Mathf.Clamp (rigid.position.y, minBoundsHeight, maxBoundsHeight),		                              		
                                         rigid.position.z);

		// apply tilt effect to our rotation
		float tiltX = rigid.velocity.y * -tilt;
		float tiltZ = rigid.velocity.x * -tilt;
		rigid.rotation = Quaternion.Euler (tiltX, 0.0f, tiltZ);
	}


	void Destruction ()
	{
		// play destruction sound
		audioSrc.clip = audioExplosion;
		audioSrc.volume = volume;
		audioSrc.Play ();

		GameController.gameState = GameStates.GameOver;

		StartCoroutine (GameoverWait (2));

		// change game state to game over

	}

	void OnTriggerEnter (Collider coll)
	{
		// if player collides with a danger object...
		if (coll.gameObject.tag == "Danger") {
			// run destruction function
			Destruction ();
		}

	}
}
