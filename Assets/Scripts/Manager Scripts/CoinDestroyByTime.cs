using UnityEngine;
using System.Collections;

public class CoinDestroyByTime : MonoBehaviour {

	public bool destroyed;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5f);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Player")
		{
			destroyed = true;
		}
	}
}
