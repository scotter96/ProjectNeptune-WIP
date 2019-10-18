using UnityEngine;
using System.Collections;

public class PlatformFall : MonoBehaviour {
	
	public float fallDelay;
	GM GC;

	private Rigidbody2D rb2d;
	
	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		GC = GameObject.Find ("GM").GetComponent<GM>();
	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Invoke ("Fall", fallDelay);
		}
	}
	
	void Fall()
	{
		rb2d.isKinematic = false;
		GC.ObjectsResetOnDemand ();
		Invoke ("Destroy", 2.5f);
	}

	void Destroy ()
	{
		Destroy (gameObject);
	}
	
	
	
}