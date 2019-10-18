using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Finishpoint : MonoBehaviour {
	
	Animator anim;
	Transform text;
	bool check;
	
	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		text = transform.Find ("Text");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Player") {
			anim.SetTrigger ("Check");
			if (check == false) {
				text.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				Invoke ("HideText", 2);
				Invoke ("EndGame", 5);
				col.gameObject.GetComponent<PlayerControl>().enabled = false;
				col.gameObject.GetComponentInChildren<Gun>().enabled = false;
			}
		}
	}
	
	void HideText ()
	{
		check = true;
		text.gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}

	void EndGame ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}
}
