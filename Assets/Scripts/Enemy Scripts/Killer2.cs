using UnityEngine;
using System.Collections;

public class Killer2 : MonoBehaviour 
{
	public GameObject target;
	
	void Update ()
	{
		if (target.GetComponent<EnemyShooterIdle> ().destroyed == false)
		{
			if (target.GetComponent<EnemyShooterIdle> ().facingRight)
				transform.position = new Vector3 (target.transform.position.x, (target.transform.position.y - 0.15f), target.transform.position.z);
			else if (target.GetComponent<EnemyShooterIdle> ().facingRight == false)
				transform.position = new Vector3 (target.transform.position.x, (target.transform.position.y - 0.15f), target.transform.position.z);
		}

		else if (target.GetComponent<EnemyShooterIdle> ().destroyed == true)
			Destroy (gameObject);
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			target.GetComponent<EnemyShooterIdle>().SpawnDeadFlat ();
			Destroy (target.gameObject);
			Destroy (gameObject);
			target.GetComponent<EnemyShooterIdle>().OnSafeZoneDestroy();
			col.gameObject.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, 800f));
		}
	}
	
}
