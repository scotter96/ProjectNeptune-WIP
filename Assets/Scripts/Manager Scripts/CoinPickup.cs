using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {

	GM GC;

	void Start () {
		GC = GameObject.Find ("GM").GetComponent<GM>();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			Destroy (gameObject);
			GC.AddPoint ();
			GC.UpdatePoint();
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			Destroy (gameObject);
			GC.AddPoint ();
			GC.UpdatePoint();
		}
	}
	
}