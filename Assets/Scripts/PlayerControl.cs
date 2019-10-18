using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public bool facingRight = true;			// For determining which way the player is currently facing.
	public bool jump = false;				// Condition for whether the player should jump.

	public float h;
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.

	bool isDebug;

	GM gm;

	void Awake()
	{
		// Setting up references.
		gm = GameObject.Find ("GM").GetComponent<GM>();
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}

	void Start ()
	{
		if (gm.shieldLevel > 0)
			gm.CreateShield ();

		Debug.Log (transform.parent);
	}

	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded)
			jump = true;

		if (Input.GetKeyDown (KeyCode.Tab)) {
			h = 0;
			isDebug = !isDebug;
			Debug.Log ("KeyInput: " + isDebug);
		}
	}


	void FixedUpdate ()
	{
		// Cache the horizontal input (use it on Editor / Standalone only).
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (isDebug == true)
			h = Input.GetAxis("Horizontal");
		#endif

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Play a random jump audio clip.
			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!GetComponent<AudioSource>().isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}
	
	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.\
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		if (gm.shieldLevel == 0) {
			if (col.gameObject.tag == "Bullet" || col.gameObject.name == "BelowCollider" || col.gameObject.tag == "Cannonball") {
				Collider2D[] cols = gameObject.GetComponents<Collider2D> ();
				foreach (Collider2D c in cols) {
					c.isTrigger = true;
				}
				
				// Move all sprite parts of the player to the front
				SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer> ();
				foreach (SpriteRenderer s in spr) {
					s.sortingLayerName = "UI";
				}
				
				// ... disable user Player Control script
				GetComponent<PlayerControl> ().enabled = false;
				
				// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
				GetComponentInChildren<Gun> ().enabled = false;
				
				// ... Trigger the 'Die' animation state
				anim.SetTrigger ("Die");
				Debug.Log (col.gameObject.name + " hit you!");
			}
		}
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		if (gm.shieldLevel == 0) {
			if (col.gameObject.name == "frontCheck" || col.gameObject.name == "BackCollider") {
				Collider2D[] cols = gameObject.GetComponents<Collider2D> ();
				foreach (Collider2D c in cols) {
					c.isTrigger = true;
				}
				
				// Move all sprite parts of the player to the front
				SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer> ();
				foreach (SpriteRenderer s in spr) {
					s.sortingLayerName = "UI";
				}
				
				// ... disable user Player Control script
				GetComponent<PlayerControl> ().enabled = false;
				
				// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
				GetComponentInChildren<Gun> ().enabled = false;
				
				// ... Trigger the 'Die' animation state
				anim.SetTrigger ("Die");
				// Debug.Log (col.gameObject.name + " hit you!");
			}
		}
	}
}