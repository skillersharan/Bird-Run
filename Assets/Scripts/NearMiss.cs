using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NearMiss : MonoBehaviour
{
	public Text nearMissText;
	public float timeToDisplay;
	// Use this for initialization
	void Start ()
	{
		nearMissText.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	IEnumerator nearMissDisplay ()
	{
		nearMissText.enabled = true;
		yield return new WaitForSeconds (timeToDisplay);
		nearMissText.enabled = false;

	}

	void OnTriggerExit (Collider coll)
	{
		if (coll.gameObject.tag == "Danger" && !nearMissText.enabled) {
			StartCoroutine (nearMissDisplay ());
		}
	}
}
