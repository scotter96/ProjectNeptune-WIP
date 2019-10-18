using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupTrigger : MonoBehaviour {

	GM gm;
	Gun gun;
	Shield shield; //Unused variable in favor of Disabled scripts #6

	public GameObject activePowerups, powerupIconPrefab;

	[System.Serializable]
	public class IconSprites
	{
		public Sprite x2PowerupIcon, x4PowerupIcon, x8PowerupIcon, shieldPowerupIcon, speedPowerupIcon, slowPowerupIcon;
		public Sprite rapidFirePowerupIcon, triplePowerupIcon, sniperPowerupIcon, specialPowerupIcon;
	}
	public IconSprites iconSprites;

	Animator[] statusBarsAnim;
	
	float rentTimeForMultiplier, rentTimeForBazooka, rentTimeForShield, rentTimeForSpeed;

	string lastBazooka;

	bool rent2x, rent4x, rent8x, rentBazooka, rentShield, rentSpeed;

	void Awake () {
		gm = GameObject.Find ("GM").GetComponent<GM> ();
		gun = GameObject.Find ("Gun").GetComponent<Gun> ();
	}
	
	void OnTriggerEnter2D (Collider2D c) {
		if (c.gameObject.tag == "Pickups")
		{
			GameObject newPowerup = Instantiate (powerupIconPrefab, activePowerups.transform);
			// newPowerup.transform.parent = activePowerups.transform;
			// newPowerup.GetComponent<RectTransform>().localScale = new Vector3 (1,1,1);
			Image newPowerupImage = newPowerup.GetComponent<Image>();
			VerticalLayoutGroup layout = newPowerup.GetComponent<VerticalLayoutGroup>();
			layout.padding.top = Mathf.FloorToInt (newPowerup.GetComponent<RectTransform>().sizeDelta.y);

			Animator statusBarAnimator = newPowerup.GetComponentInChildren<Animator>();
			statusBarAnimator.SetFloat ("speed", Time.deltaTime);

			Debug.Log ("Delta time: " + Time.deltaTime);
			Debug.Log ("Animator speed: " + statusBarAnimator.GetFloat ("speed"));

			// If this is Health pickup
			if (c.gameObject.name.Contains ("Health")) {
				if (c.gameObject.name.Contains ("10")) {
					// Add +10 HP to player scripts here
				} else if (c.gameObject.name.Contains ("25")) {
					// Add +25 HP to player scripts here
				} else if (c.gameObject.name.Contains ("50")) {
					// Add +50 HP to player scripts here
				} else if (c.gameObject.name.Contains ("100")) {
					// Add +100 HP to player scripts here
				}
			}

			// If this is Coin Multiplier pickup
			else if (c.gameObject.name.Contains ("Multi")) {
				Debug.Log ("Before pickup: " + gm.coinMultiplier);
				rentTimeForMultiplier = 15;

				if (c.gameObject.name.Contains ("2")) {
					gm.coinMultiplier *= 2;
					rent2x = true;

					DestroyExistingPowerupIcon ("2x");
					newPowerup.name = "Powerup_2x";
					newPowerupImage.sprite = iconSprites.x2PowerupIcon;
				} 
				else if (c.gameObject.name.Contains ("4")) {
					if (rent2x) {
						gm.coinMultiplier /= 2;
						rent2x = false;

						DestroyExistingPowerupIcon ("2x");
					}
					
					gm.coinMultiplier *= 4;
					rent4x = true;

					DestroyExistingPowerupIcon ("4x");
					newPowerup.name = "Powerup_4x";
					newPowerupImage.sprite = iconSprites.x4PowerupIcon;
				}
				else if (c.gameObject.name.Contains ("8")) {
					if (rent2x) {
						gm.coinMultiplier /= 2;
						rent2x = false;

						DestroyExistingPowerupIcon ("2x");
					}
					if (rent4x) {
						gm.coinMultiplier /= 4;
						rent4x = false;

						DestroyExistingPowerupIcon ("4x");
					}

					gm.coinMultiplier *= 8;
					rent8x = true;

					DestroyExistingPowerupIcon ("8x");
					newPowerup.name = "Powerup_8x";
					newPowerupImage.sprite = iconSprites.x8PowerupIcon;
				}

				Debug.Log ("After pickup: " + gm.coinMultiplier);
			}

			// If this is Bazooka pickup
			else if (c.gameObject.name.Contains ("Bazooka")) {
				Debug.Log ("Before pickup: " + gun.equipped);

				if (!rentBazooka)
					lastBazooka = gun.equipped;
				
				rentBazooka = true;
				rentTimeForBazooka = 10;

				if (c.gameObject.name.Contains ("Rapid-fire")) {					
					gun.ChangeWeapon ("Rapid-fire");

					DestroyExistingPowerupIcon ("Rapid");
					newPowerup.name = "Powerup_Rapid";
					newPowerupImage.sprite = iconSprites.rapidFirePowerupIcon;
				}
				else if (c.gameObject.name.Contains ("Special")) {
					gun.ChangeWeapon ("Special");

					DestroyExistingPowerupIcon ("Special");
					newPowerup.name = "Powerup_Special";
					newPowerupImage.sprite = iconSprites.specialPowerupIcon;
				}
				else if (c.gameObject.name.Contains ("Triple")) {
					gun.ChangeWeapon ("Triple");

					DestroyExistingPowerupIcon ("Triple");
					newPowerup.name = "Powerup_Triple";
					newPowerupImage.sprite = iconSprites.triplePowerupIcon;
				}
				else if (c.gameObject.name.Contains ("Sniper")) {
					gun.ChangeWeapon ("Sniper");

					DestroyExistingPowerupIcon ("Sniper");
					newPowerup.name = "Powerup_Sniper";
					newPowerupImage.sprite = iconSprites.sniperPowerupIcon;
				}
				Debug.Log ("After pickup: " + gun.equipped);
			}

			// If this is Shield pickup
			else if (c.gameObject.name.Contains ("Shield")) {
				Debug.Log ("Before pickup: " + gm.shieldLevel);

				gm.BuyShield ();

				// Disabled scripts #6: Scripts to rent (not permanently bought) shield for 10 seconds
				rentShield = true;
				rentTimeForShield = 10;
				shield = GameObject.FindWithTag ("Shield").GetComponent<Shield> ();

				DestroyExistingPowerupIcon ("Shield");
				newPowerup.name = "Powerup_Shield";
				newPowerupImage.sprite = iconSprites.shieldPowerupIcon;

				Debug.Log ("After pickup: " + gm.shieldLevel);
			}

			else if (c.gameObject.name.Contains ("Speed")) {
				Debug.Log ("Before pickup: " + Time.timeScale + "x speed");

				if (!rentSpeed)
					rentSpeed = true;
				
				rentTimeForSpeed = 10;
				Time.timeScale = 2;

				DestroyExistingPowerupIcon ("Speed");
				newPowerup.name = "Powerup_Speed";
				newPowerupImage.sprite = iconSprites.speedPowerupIcon;

				Debug.Log ("After pickup: " + Time.timeScale + "x speed");
			}

			else if (c.gameObject.name.Contains ("Slow")) {
				Debug.Log ("Before pickup: " + Time.timeScale + "x speed");

				if (!rentSpeed)
					rentSpeed = true;
				
				rentTimeForSpeed = 2.5f;
				Time.timeScale = 0.5f;

				DestroyExistingPowerupIcon ("Slow");
				newPowerup.name = "Powerup_Slow";
				newPowerupImage.sprite = iconSprites.slowPowerupIcon;

				Debug.Log ("After pickup: " + Time.timeScale + "x speed");
			}

			Destroy (c.gameObject);
		}
	}
	
	void DestroyExistingPowerupIcon (string name)
	{
		Transform[] powerups = activePowerups.GetComponentsInChildren<Transform>();
		foreach (Transform powerup in powerups)
		{
			if (name == "2x" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "4x" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "8x" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Speed" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Slow" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Rapid" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Triple" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Special" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Sniper" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
			else if (name == "Shield" && powerup.name.Contains (name))
				Destroy (powerup.gameObject);
		}
	}
	void Start ()
	{
		Debug.Log ("Original multi: " + gm.coinMultiplier);
		Debug.Log ("Original bazooka: " + gun.equipped);
		Debug.Log ("Original shield: " + gm.shieldLevel);
	}

	public void DisableShieldRent ()
	{
		rentShield = false;
		rentTimeForShield = 0;

		DestroyExistingPowerupIcon ("Shield");
		UpdateStatusBars ();
		Debug.Log ("Original shield: " + gm.shieldLevel);
	}

	void DisableMultiplierRent ()
	{
		if (rent2x) {
			gm.coinMultiplier /= 2;
			rent2x = false;
		}
		else if (rent4x) {
			gm.coinMultiplier /= 4;
			rent4x = false;
		}
		else if (rent8x) {
			gm.coinMultiplier /= 8;
			rent8x = false;
		}
	}

	void UpdateStatusBars ()
	{
		statusBarsAnim = activePowerups.GetComponentsInChildren<Animator>();
	}

	void Update ()
	{
		/*
		if (rent2x || rent4x || rent8x)
		{
			Debug.Log ("multi rent time: " + rentTimeForMultiplier);
		}
		if (rentBazooka)
		{
			Debug.Log ("bazooka rent time: " + rentTimeForBazooka);
		}
		if (rentShield)
		{
			Debug.Log ("shield rent time: " + rentTimeForShield);
		}
		if (rentSpeed)
		{
			Debug.Log ("speed rent time: " + rentTimeForSpeed);
		}
		
		if (rent2x || rent4x || rent8x || rentBazooka || rentShield || rentSpeed)
		{
			if (statusBarsAnim.Length != 0)
			{
				foreach (Animator statusBarAnim in statusBarsAnim)
				{
					statusBarAnim.speed = Time.deltaTime;			
				}
			}
		}
		*/
		// Debug.Log (rentTimeForSpeed);

		// Once multiplier rent time is up
		if (rentTimeForMultiplier > 0)
		{
			rentTimeForMultiplier -= Time.deltaTime;
			UpdateStatusBars ();
		}
		
		else if (rentTimeForMultiplier <= 0 && (rent2x || rent4x || rent8x)) {
			DisableMultiplierRent ();
			rentTimeForMultiplier = 0;

			DestroyExistingPowerupIcon ("2x");
			DestroyExistingPowerupIcon ("4x");
			DestroyExistingPowerupIcon ("8x");
			UpdateStatusBars ();
			Debug.Log ("Original multi: " + gm.coinMultiplier);
		}

		// Once bazooka rent time is up
		if (rentTimeForBazooka > 0)
		{
			rentTimeForBazooka -= Time.deltaTime;
			UpdateStatusBars ();
		}
		
		else if (rentTimeForBazooka <= 0 && rentBazooka) {
			gun.ChangeWeapon (lastBazooka);
			rentBazooka = false;
			rentTimeForBazooka = 0;

			DestroyExistingPowerupIcon ("Rapid");
			DestroyExistingPowerupIcon ("Triple");
			DestroyExistingPowerupIcon ("Sniper");
			DestroyExistingPowerupIcon ("Special");
			UpdateStatusBars ();
			Debug.Log ("Original bazooka: " + gun.equipped);
		}

		// Disabled scripts #6: Once shield rent time is up
		if (rentTimeForShield > 0)
			rentTimeForShield -= Time.deltaTime;
			
		else if (rentTimeForShield <= 0 && rentShield) {
			shield.ReduceShield ();
			rentShield = false;
			rentTimeForShield = 0;

			DestroyExistingPowerupIcon ("Shield");
			UpdateStatusBars ();
			Debug.Log ("Original shield: " + gm.shieldLevel);
		}		

		if (rentTimeForSpeed > 0)
		{
			rentTimeForSpeed -= Time.deltaTime;
			UpdateStatusBars ();
		}
		else if (rentTimeForSpeed <= 0 && rentSpeed) {
			rentSpeed = false;
			rentTimeForSpeed = 0;
			Time.timeScale = 1;

			DestroyExistingPowerupIcon ("Speed");
			DestroyExistingPowerupIcon ("Slow");
			UpdateStatusBars ();
		}

		if (gm.isWaveBreak) {
			if (rent2x || rent4x || rent8x || rentBazooka) {
				rentTimeForMultiplier = 0;
				rentTimeForBazooka = 0;

				DestroyExistingPowerupIcon ("Speed");
				DestroyExistingPowerupIcon ("Slow");
				DestroyExistingPowerupIcon ("Shield");
				DestroyExistingPowerupIcon ("Special");
				DestroyExistingPowerupIcon ("Rapid");
				DestroyExistingPowerupIcon ("Triple");
				DestroyExistingPowerupIcon ("Sniper");
				DestroyExistingPowerupIcon ("2x");
				DestroyExistingPowerupIcon ("4x");
				DestroyExistingPowerupIcon ("8x");
				UpdateStatusBars ();				
			}
		}
	}
}