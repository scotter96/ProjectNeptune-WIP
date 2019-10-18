using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

	public float speed;
	float X;
	float xScale;
	float hit;

	public bool facingRight;
	public bool defeated = false;

	GM GC;
	Gun gun;

	public int numberOfHitToDeath = 3; // default amount of hit to kill this enemy instance

	public GameObject deadPrefab;

	void Awake ()
	{
		X = transform.position.x;
		GC = GameObject.Find ("GM").GetComponent<GM> ();
		gun = GameObject.Find ("Gun").GetComponent<Gun> ();
	}

	void Start () {
		facingRight = true;
		hit = 0;
	}

	void Update ()
	{
		if (facingRight)
			X += Time.deltaTime * speed;
		else
			X -= Time.deltaTime * speed;

			transform.position = new Vector3 (X, transform.position.y, transform.position.z);
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		float hitAddition = 0;

		if (other.gameObject.tag == "Bullet")
		{
			if (gun.equipped == "Basic") {
				if (GC.bazookaLevel == 1)
					hitAddition = 1;
				else if (GC.bazookaLevel == 2)
					hitAddition = 2;
				else if (GC.bazookaLevel == 3)
					hitAddition = 3;
			}
			else if (gun.equipped == "Triple") {
				if (GC.TRBazookaLevel == 1)
					hitAddition = 1;
				else if (GC.TRBazookaLevel == 2)
					hitAddition = 2;
				else if (GC.TRBazookaLevel == 3)
					hitAddition = 3;
			}
			else if (gun.equipped == "Rapid-fire") {
				if (GC.RFBazookaLevel == 1)
					hitAddition = 0.33f;
				else if (GC.RFBazookaLevel == 2)
					hitAddition = 0.66f;
				else if (GC.RFBazookaLevel == 3)
					hitAddition = 1;
			}
			else if (gun.equipped == "Special" || gun.equipped == "Sniper") {
				hitAddition = numberOfHitToDeath;
			}
					
			hit += hitAddition;

			if (hit >= numberOfHitToDeath)
			{
				SpawnDead ();
				defeated = true;
				Destroy (gameObject, 0.1f);
			}
		}
	}
	
	void SpawnDead ()
	{
		GameObject theDead = Instantiate (deadPrefab, transform.position, Quaternion.identity) as GameObject;
		xScale = theDead.transform.localScale.x;
		if (facingRight)
		{
			theDead.transform.localScale = new Vector3 (xScale, 0.4f, 1);
		}
		else
			theDead.transform.localScale = new Vector3 (-xScale, 0.4f, 1);
	}

	public void SpawnDeadFlat ()
	{
		GameObject theDead = Instantiate (deadPrefab, transform.position, Quaternion.identity) as GameObject;
		xScale = theDead.transform.localScale.x;

		if (facingRight)
			theDead.transform.localScale = new Vector3 (xScale, 0.2f, 1);
		else
			theDead.transform.localScale = new Vector3 (-xScale, 0.2f, 1);
	}

	public void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
