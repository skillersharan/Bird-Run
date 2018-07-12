
using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour
{
	public GameObject[] danger;
	public float fireRate = 0.4f;
	public float spawnWidth;
	public bool enableRandomRotaion = false;
	private float seconds;
	public float startDelay;
	// Update is called once per frame
	private bool startSpawn = false;
	public float spawnOffset = 0;
	public bool randomHeight = false;
	public static float tojoratio = 8;
	public bool randomColor = false;
	public Color[] colors;
	IEnumerator Delay (float delay)
	{
		yield return new WaitForSeconds (delay);
		startSpawn = true;
	}

	void Start ()
	{
		tojoratio = GameController.gameSpeed * fireRate;
	}

	void Update ()
	{
		// instantiates our danger objects
		StartCoroutine (Delay (startDelay));

		if (startSpawn) {
			fireRate = tojoratio / GameController.gameSpeed;
			Spawn (fireRate);
		}
	}

	void Spawn (float rate)
	{
		// timer, adds up the delta time for seconds
		seconds += Time.deltaTime;
		// if seconds great than our rate as defined in the inspector
		if (seconds > rate) {
			// random x position
			float randomX = Random.Range (transform.position.x - spawnWidth, transform.position.x + spawnWidth);
			Vector3 position = new Vector3
                (
                randomX,
                transform.position.y,
                transform.position.z
			);
			Quaternion instarotation;
			// randomly select an object in the danger array
			int randomObj = Random.Range (0, danger.Length);
			instarotation = new Quaternion (danger [randomObj].transform.rotation.x, danger [randomObj].transform.rotation.y, danger [randomObj].transform.rotation.z, danger [randomObj].transform.rotation.w);
			if (enableRandomRotaion) {
				float randomYRotation = Random.Range (0, 360);
				instarotation.eulerAngles = new Vector3 (danger [randomObj].transform.rotation.eulerAngles.x, randomYRotation, danger [randomObj].transform.rotation.eulerAngles.z);
			} 
		
			GameObject instantiatedobject = Instantiate (danger [randomObj], position, instarotation) as GameObject;
			if (randomHeight)
				instantiatedobject.transform.localScale = new Vector3 (5, Random.Range (5, 16), 5);
			if (randomColor) {	
				int x = Random.Range (0, colors.Length);
				
				instantiatedobject.GetComponent<Renderer> ().material.color = colors [x];

			}
				
			// zero out the seconds variable
			seconds = 0;
		}
	}
}
