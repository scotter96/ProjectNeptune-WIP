using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour 
{
	public RocketLauncher rocketLauncher;
	public bool inside;
	GM GC;

	void Start ()
	{
		GC = GameObject.Find ("GM").GetComponent<GM> ();
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Player") {
			inside = true;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.tag == "Player") {
			inside = false;
		}
	}

	void Update ()
	{
		if (GC.dead) {
			inside = false;
		}
	}
}
