using UnityEngine;
using System.Collections;

public class DeathFromAbove : MonoBehaviour 
{
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy (transform.parent.gameObject);
		}
	}
}
