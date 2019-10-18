using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	Animator anim;
	Transform text;
	bool check;
	public Transform respawnSite;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		text = transform.Find ("Text");
		respawnSite = GameObject.Find ("Respawner").GetComponent<Transform> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Player") {
			anim.SetTrigger ("Check");
			respawnSite.position = gameObject.transform.position;
			if (check == false) {
				text.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				Invoke ("HideText", 2);
			}
		}
	}

	void HideText ()
	{
		check = true;
		text.gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}
}
