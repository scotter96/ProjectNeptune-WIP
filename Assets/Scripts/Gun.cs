using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public int shootDelay = 1;

	public float speed = 20f;				// The speed the rocket will fire at.
	float timeSinceLastShot;
	float time = 0;
	float rocketsSpawned;
	float resetShootDelayTimer = 0;		// For rapid-fire bazooka only.

	public bool UIShoot = false;
	public bool update = false;
	bool tripleFired = false;
	bool noReload = false;

	PlayerControl playerCtrl;		// Reference to the PlayerControl script.

	Animator anim;					// Reference to the Animator component.

	public string equipped = "Basic";

	SpriteRenderer bazookaSprite;

	GM gm;

	[System.Serializable] // Prefab of the rockets
	public class BazookaPrefabs
	{
		public Rigidbody2D basicRocket;
		public Rigidbody2D tripleRocket;
		public Rigidbody2D rapidRocket;
		public Rigidbody2D specialRocket;
		public Rigidbody2D sniperRocket;
	}
	public BazookaPrefabs bazookaPrefabs;

	[System.Serializable] // Sprites of the rockets
	public class BazookaSprites
	{
		public Sprite basicBazookaSprite;
		public Sprite TRBazookaSprite;
		public Sprite RFBazookaSprite;
		public Sprite specialBazookaSprite;
		public Sprite sniperBazookaSprite;
	}
	public BazookaSprites bazookaSprites;

	void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
		bazookaSprite = transform.root.GetChild (0).GetComponent<SpriteRenderer> ();
		gm = GameObject.Find ("GM").GetComponent<GM> ();
	}

	void Update ()
	{
		if (timeSinceLastShot < 1)
			timeSinceLastShot += Time.deltaTime;
		else if (timeSinceLastShot > 1)
			timeSinceLastShot = 1;

		if (resetShootDelayTimer < 1)
			resetShootDelayTimer += Time.deltaTime;
		else if (resetShootDelayTimer > 1)
			resetShootDelayTimer = 1;

		/* Debugging commands for rapid-fire rockets
		Debug.Log ("timeSinceLastShot: " + timeSinceLastShot);
		Debug.Log ("resetShootDelayTimer: " + resetShootDelayTimer);
		Debug.Log ("rocketSpawned: " + rocketsSpawned);
		*/

#if UNITY_EDITOR
		if (Input.GetKeyUp (KeyCode.Q))
		{
			noReload = !noReload;
			Debug.Log (noReload);
		}

		else if (Input.GetKeyUp (KeyCode.Alpha1))
		{
			if (gm.bazookaLevel < 1)
				gm.bazookaLevel = 1;
			
			ChangeWeapon ("Basic");
		}
		
		else if (Input.GetKeyUp (KeyCode.Alpha2))
		{
			if (gm.TRBazookaLevel < 1)
				gm.TRBazookaLevel = 1;
			
			ChangeWeapon ("Triple");
		}
		
		else if (Input.GetKeyUp (KeyCode.Alpha3))
		{
			if (gm.RFBazookaLevel < 1)
				gm.RFBazookaLevel = 1;
			
			ChangeWeapon ("Rapid-fire");
		}

		else if (Input.GetKeyUp (KeyCode.Alpha5))
		{
			if (gm.SniperBazookaLevel < 3)
				gm.SniperBazookaLevel ++;

			ChangeWeapon ("Sniper");
		}
		
		else if (Input.GetKeyUp (KeyCode.Alpha4))
			ChangeWeapon ("Special");
#endif
		if (equipped == "Basic" || equipped == "Sniper") {
			if (timeSinceLastShot >= shootDelay || noReload) {
				timeSinceLastShot = shootDelay;
				rocketsSpawned = 0;
			}
		} else if (equipped == "Triple") {
			if (noReload)
				timeSinceLastShot = shootDelay;

			if (rocketsSpawned >= 3) {
				tripleFired = false;
				rocketsSpawned = 0;
			}
		} else if (equipped == "Rapid-fire") {
			if (rocketsSpawned < 4 || noReload)
				timeSinceLastShot = shootDelay;

			if ((rocketsSpawned >= 4 || rocketsSpawned < 4) && resetShootDelayTimer >= shootDelay) {
				rocketsSpawned = 0;
			}	
		} else if (equipped == "Special" || noReload) {
			timeSinceLastShot = shootDelay;
			rocketsSpawned = 0;
		}

		if (tripleFired) {
			if (time < 0.1f)
				time += Time.deltaTime;
			else {
				if (rocketsSpawned < 3) {
					Fire ();
					time = 0;
				}
			}
		}

		if ((Input.GetButtonDown ("Fire1") || UIShoot) && timeSinceLastShot >= shootDelay)
		{
			if (!noReload) { // If No Reload cheat is off
				if (equipped == "Basic" || equipped == "Sniper") {
					Invoke ("UIShootFalse", shootDelay);
					Fire ();
				} 
				else if (equipped == "Rapid-fire" || equipped == "Special") {
					UIShootFalse ();
					Fire ();
				}
				else if (equipped == "Triple") {
					UIShootFalse ();
					tripleFired = true;
				}
			}
			else if (noReload) { // If No Reload cheat is on
				UIShootFalse ();
				Fire ();
			}
		}
	}

	void UIShootFalse ()
	{
		UIShoot = false;
		update = false;
	}

	void Fire ()
	{
		timeSinceLastShot = 0;

		// ... set the animator Shoot trigger parameter and play the audioclip.
		anim.SetTrigger("Shoot");
		GetComponent<AudioSource>().Play();

		Rigidbody2D selectedRocket = null;
		if (equipped == "Basic")
			selectedRocket = bazookaPrefabs.basicRocket;
		else if (equipped == "Triple")
			selectedRocket = bazookaPrefabs.tripleRocket;
		else if (equipped == "Rapid-fire") {
			resetShootDelayTimer = 0;
			selectedRocket = bazookaPrefabs.rapidRocket;
		}
		else if (equipped == "Special")
			selectedRocket = bazookaPrefabs.specialRocket;
		else if (equipped == "Sniper")
			selectedRocket = bazookaPrefabs.sniperRocket;

		rocketsSpawned++;

		// If the player is facing right...
		if(playerCtrl.facingRight)
		{
			// ... instantiate the rocket facing right and set it's velocity to the right. 
			Rigidbody2D bulletInstance = Instantiate(selectedRocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2(speed, 0);
		}
		else
		{
			// Otherwise instantiate the rocket facing left and set it's velocity to the left.
			Rigidbody2D bulletInstance = Instantiate(selectedRocket, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2(-speed, 0);
		}
	}

	public void ChangeWeapon (string weapon)
	{
		equipped = weapon;
		Debug.Log (equipped);

		if (weapon == "Basic") {			
			bazookaSprite.sprite = bazookaSprites.basicBazookaSprite;
		} 
		else if (weapon == "Triple") {
			bazookaSprite.sprite = bazookaSprites.TRBazookaSprite;
		} 
		else if (weapon == "Rapid-fire") {
			bazookaSprite.sprite = bazookaSprites.RFBazookaSprite;
		} 
		else if (weapon == "Special") {
			bazookaSprite.sprite = bazookaSprites.specialBazookaSprite;
		}
		else if (weapon == "Sniper") {
			bazookaSprite.sprite = bazookaSprites.sniperBazookaSprite;
		}
	}
}
