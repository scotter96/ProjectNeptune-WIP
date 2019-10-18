using UnityEngine;
using System.Collections;

public class EnemyShooterIdle : MonoBehaviour 
{
	public GameObject deadPrefab;
	GM GC;
	int hit;
	float X;
	public TriggerOnExit rocketTriggerZone;
	public bool facingRight;
	public bool destroyed = false;

	void Start ()
	{
		GC = GameObject.Find ("GM").GetComponent<GM> ();
		X = transform.localScale.x;
		if (X > 0)
			facingRight = true;
		else if (X < 0)
			facingRight = false;
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Bullet") {
			if (GC.bazookaLevel == 1) {			
				hit += 1;
				if (hit >= 3) {
					SpawnDead ();
					Destroy (gameObject, 0.5f);
					destroyed = true;
				}
			} else if (GC.bazookaLevel == 2) {			
				hit += 2;
				if (hit >= 3) {
					SpawnDead ();
					Destroy (gameObject, 0.5f);
					destroyed = true;
				}
			} else if (GC.bazookaLevel >= 3) {			
				hit += 3;
				if (hit >= 3) {
					SpawnDead ();
					Destroy (gameObject, 0.5f);
					destroyed = true;
				}
			}
		}
	}
	
	void SpawnDead ()
	{
		GameObject theDead = Instantiate (deadPrefab, transform.position, Quaternion.identity) as GameObject;
		if (facingRight)
		{
			theDead.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
		}
		else
			theDead.transform.localScale = new Vector3 (-0.5f, 0.5f, 1);
	}

	public void SpawnDeadFlat ()
	{
		GameObject theDead = Instantiate (deadPrefab, transform.position, Quaternion.identity) as GameObject;
		if (facingRight)
		{
			theDead.transform.localScale = new Vector3 (0.5f, 0.35f, 1);
		}
		else
			theDead.transform.localScale = new Vector3 (-0.5f, 0.35f, 1);
	}

	public void OnSafeZoneDestroy ()
	{
		rocketTriggerZone.OnDestroy ();
	}
}
