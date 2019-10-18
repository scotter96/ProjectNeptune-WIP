using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.
	public PlayerControl playerControl;

	Gun gun;
	GM gm;

	int sniperResistance;

	/* Debugging commands to tell how much resistance does the gun have
	void Update ()
	{
		if (gun.equipped == "Sniper")
			Debug.Log ("Sniper Resistance: " + sniperResistance);
	}
	*/

	void Awake () 
	{
		gun = GameObject.Find ("Gun").GetComponent<Gun> ();
		gm = GameObject.Find ("GM").GetComponent<GM> ();

		// Destroy the rocket after 3 seconds if it doesn't get destroyed before then.
		int destroyTime;

		sniperResistance = gm.SniperBazookaLevel;

		if (gun.equipped != "Sniper")
			destroyTime = 3;
		else
			destroyTime = 5;
		
		Destroy (gameObject, destroyTime);
		Invoke ("OnExplode", destroyTime);
	}

	void OnCollisionEnter2D (Collision2D col) 
	{
		if (gun.equipped != "Sniper") {
			ExplodeRocket ();
		}
		else {
			if (sniperResistance <= 0)
				ExplodeRocket ();
		}

		if (col.gameObject.tag == "PlayerBarrier")
			ExplodeRocket ();
		else if (col.gameObject.tag == "Bullet")
			Destroy (col.gameObject);
	}

	void OnCollisionExit2D ()
	{
		if (gun.equipped == "Sniper") {
			if (sniperResistance > 0)
				sniperResistance--;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.tag == "RocketRadius")
		{
			ExplodeRocket ();
		}
	}

	void ExplodeRocket ()
	{
		// Call the explosion instantiation.
		OnExplode ();

		// Destroy the rocket.
		Destroy (gameObject);
	}

	public void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}
}