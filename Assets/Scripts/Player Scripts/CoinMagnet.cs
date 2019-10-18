using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour {

	public GameObject player;
	void Update ()
	{
		if (player == null)
		{
			player = GameObject.FindWithTag ("Player");
		}
		else
		{
			transform.position = player.transform.position;
		}
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Coin") {
			float x,y;
			Rigidbody2D colRB = col.transform.GetComponent<Rigidbody2D> ();

			if (col.transform.position.x < transform.position.x)			
				x = 0.001f;
			else
				x = -0.001f;

			if (col.transform.position.y < transform.position.y)
				y = 0.0005f;
			else
				y = -0.0005f;
			
			colRB.AddForce (new Vector2 (x, y));
		}
	}
}