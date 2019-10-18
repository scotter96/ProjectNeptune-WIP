using UnityEngine;
using System.Collections;

public class Remover : MonoBehaviour
{
	public GameObject splash;

	GM GC;

	void Start ()
	{
		GC = GameObject.Find ("GM").GetComponent<GM> ();
	}
		
	void OnTriggerEnter2D(Collider2D col)
	{
		// If the player hits the trigger...
		if (col.gameObject.tag == "Player") {
			// .. stop the camera tracking the player
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MyCamera> ().enabled = false;

			// .. stop the Health Bar following the player
			/*
			if(GameObject.FindGameObjectWithTag("HealthBar").activeSelf)
			{
				GameObject.FindGameObjectWithTag("HealthBar").SetActive(false);
			}
			*/
			// ... instantiate the splash where the player falls in.
			Instantiate (splash, col.transform.position, transform.rotation);
			// ... destroy the player.
			Destroy (col.gameObject);
			// ... decrease the player's life
			GC.LoseLife ();
			// Debug.Log (col.gameObject.name + " has drowned!");
		}
		else {
			// ... instantiate the splash where the enemy falls in.
			Instantiate (splash, col.transform.position, transform.rotation);

			// Destroy the enemy.
			Destroy (col.gameObject,0.5f);
//			Time.timeScale = 0; - used for testing only
			// Debug.Log (col.gameObject.name + " has drowned!");
		}
	}
}
