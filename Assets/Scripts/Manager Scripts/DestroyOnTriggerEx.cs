using UnityEngine;
using System.Collections;

public class DestroyOnTriggerEx : MonoBehaviour 
{
	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Bullet") {
			other.GetComponent<Rocket> ().OnExplode ();
			Destroy (other.gameObject);
		}
	}
}
