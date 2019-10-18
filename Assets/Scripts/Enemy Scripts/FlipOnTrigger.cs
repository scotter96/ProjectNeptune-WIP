using UnityEngine;
using System.Collections;

public class FlipOnTrigger : MonoBehaviour 
{
	EnemyPatrol EP;

	void Start ()
	{
		EP = transform.parent.GetComponent<EnemyPatrol> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Wall") {
			EP.Flip ();
		}
	}
}
