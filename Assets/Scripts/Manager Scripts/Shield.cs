using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	public Sprite[] shieldSprites;

	SpriteRenderer thisSprite;

	bool shieldReduced = false;

	public float XForceToPlayer = -5000, YForceToPlayer = 0;

	GM gm;

	Collider2D thisCollider;

	PickupTrigger pickupTrigger;

	void Awake () {
		thisSprite = gameObject.GetComponent<SpriteRenderer> ();
		gm = GameObject.Find ("GM").GetComponent<GM> ();
		thisCollider = gameObject.GetComponent<Collider2D> ();
		pickupTrigger = GameObject.FindWithTag ("Player").GetComponent<PickupTrigger>();
	}

	void Start ()
	{
		ChangeSprite ();
	}

	public void ChangeSprite ()
	{
		thisSprite.sprite = shieldSprites [gm.shieldLevel - 1];
	}

	void OnTriggerEnter2D (Collider2D col)
	{		
		if (!shieldReduced && (col.gameObject.name == "frontCheck" || col.gameObject.name == "BackCollider")) 
		{
			thisCollider.enabled = false;

			if (col.gameObject.name == "frontCheck") {
				EnemyPatrol ep = col.transform.root.GetComponent<EnemyPatrol> ();
				ep.Flip ();

				Invoke ("ReduceShield", 0.1f);
			}
			else if (col.gameObject.name == "BackCollider") {
				Invoke ("ReduceShield", 0.1f);
			}
			AddForceToPlayer (XForceToPlayer,YForceToPlayer);
			shieldReduced = true;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		 if (shieldReduced && (col.gameObject.name == "frontCheck" || col.gameObject.name == "BackCollider")) 
			shieldReduced = false;
	}

	public void ReduceShield ()
	{
		gm.shieldLevel -= 1;
		pickupTrigger.DisableShieldRent();

		thisCollider.enabled = true;

		if (gm.shieldLevel <= 0)
			Destroy (this.gameObject);
		else
			ChangeSprite ();
	}

	void AddForceToPlayer (float x, float y)
	{
		Vector3 rootScale = transform.root.localScale;
		float m = (rootScale.x);
		x *= m;
		float n = rootScale.y;
		y *= n;

		Rigidbody2D thisRB = transform.root.GetComponent<Rigidbody2D> ();
		Vector2 force = new Vector2 (x, y);
		thisRB.AddForce (force);
		thisRB.velocity = new Vector2 (0,0);
		Debug.Log (force);
	}
}