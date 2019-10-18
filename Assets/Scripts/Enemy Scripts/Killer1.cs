using UnityEngine;
using System.Collections;

public class Killer1 : MonoBehaviour 
{
	public GameObject target;

	GM gm;

	void Awake ()
	{
		gm = GameObject.Find ("GM").GetComponent<GM> ();
	}

	void Update ()
	{
/*		if (target.GetComponent<EnemyPatrol> ().defeated == false)
		{
			if (target.GetComponent<EnemyPatrol> ().facingRight == true)
				transform.position = new Vector3 ((target.transform.position.x + 0.052f), (target.transform.position.y + 0.98f), target.transform.position.z);
			else if (target.GetComponent<EnemyPatrol> ().facingRight == false)
				transform.position = new Vector3 ((target.transform.position.x - 0.052f), (target.transform.position.y + 0.98f), target.transform.position.z);
		}	*/

		if (target.GetComponent<EnemyPatrol> ().defeated == true)
			Destroy (gameObject);
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			target.GetComponent<EnemyPatrol>().SpawnDeadFlat ();
			Destroy (target.gameObject);
			Destroy (gameObject);
			col.gameObject.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, 800f));
		}
	}	

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Shield")
		{
			target.GetComponent<EnemyPatrol>().SpawnDeadFlat ();
			Destroy (target.gameObject);
			Destroy (gameObject);
			col.transform.parent.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, 800f));
		}
	}
}