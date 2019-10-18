using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	public int lives = 3;
	public int points = 0;
	public int bazookaLevel = 1;
	public int RFBazookaLevel = 0;
	public int TRBazookaLevel = 0;
	public int SniperBazookaLevel = 0;
	public int shieldLevel = 0;
	public int coinMultiplier = 1;
	public int coinValue = 10;
	public int wave = 0;
	public int highScore = 0;

	public string equippedBazookaInGunScript = null;
	public string slot1, slot2;

	Text livetxt;
	Text pointtxt;
	Text gotext;
	Text liveshtxt;
	Text pointshtxt;
	Text goshtext;
	Text waveShadow;
	Text waveText;

	public bool coinMagnet;
	public bool isSurvival;
	public bool isStory;
	public bool dead;
	public bool enemySpawning;
	public bool isWaveBreak;
	bool ResetOnDemand;
	bool sceneChangeScan;

	public GameObject playerPrefab;
	public GameObject fallingPlatPrefab;
	public GameObject shieldPrefab;
	GameObject respawner;

	AudioSource gameAudioManager;
	AudioSource menuAudioManager;

	public AudioClip buyClip;
	public AudioClip scoreClip;

	public int[] bazookaPrices;
	public int[] TRBazookaPrices;
	public int[] RFBazookaPrices;
	public int[] SniperBazookaPrices;
	public int[] multiPrices;
	public int[] shieldPrices;

	static GM instance;

	Spawner spawner;
	MobileUIButtons mobileUIScript;
	GameUI gameUI;
	MenuUI menuUI;
	Shield shield;
	Gun gun;
	Pauser pauser;
	Inventory inventory;

	void Start() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		enemySpawning = true;
		dead = false;

		if (coinMultiplier < 1)
			coinMultiplier = 1;

		if (SceneManager.GetActiveScene().name != "Menu") {
			if (SceneManager.GetActiveScene().name != "Survival" && SceneManager.GetActiveScene().name != "Test") {
				livetxt = GameObject.Find ("Lives").GetComponent<Text> ();
				liveshtxt = GameObject.Find ("Lives Shadow").GetComponent<Text> ();
			}
			pointtxt = GameObject.Find ("Points").GetComponent<Text> ();
			gotext = GameObject.Find ("GO").GetComponent<Text> ();		
			pointshtxt = GameObject.Find ("Points Shadow").GetComponent<Text> ();
			goshtext = GameObject.Find ("GO Shadow").GetComponent<Text> ();
			gameAudioManager = GameObject.Find ("Game Audio Manager").GetComponent<AudioSource> ();
			respawner = GameObject.Find ("Spawner");
			//		rocketTrigger1 = GameObject.Find ("SafeZone1").GetComponent<TriggerOnExit>();
			//		if (GameObject.Find("SafeZone2").activeInHierarchy)
			//			rocketTrigger2 = GameObject.Find ("SafeZone2").GetComponent<TriggerOnExit>();
			mobileUIScript = GameObject.Find ("EventSystem").GetComponent<MobileUIButtons> ();

			UpdatePoint ();
		}

		if (bazookaLevel == 0) {
//			GameObject.Find ("FireButton").GetComponent<Button>().interactable = false;
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = false;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = false;
		}
	}

	void Update () {
		// cheats (only usable during development)		
#if UNITY_EDITOR
		if (Input.GetKeyUp (KeyCode.E)) {
			if (SceneManager.GetActiveScene().name != "Menu")
			{
				AddPoint ();
				UpdatePoint ();
			}
		}
		else if (Input.GetKeyUp (KeyCode.Alpha8))
            wave += 1;
		else if (Input.GetKeyUp (KeyCode.W) && bazookaLevel < 3)
			BuyBazooka ();
		else if (Input.GetKeyUp (KeyCode.T) && lives < 9)
		{
			if (SceneManager.GetActiveScene().name != "Survival" && SceneManager.GetActiveScene().name != "Survival")
				On1UPBought ();
		}
		else if (Input.GetKeyUp (KeyCode.Y))
		{
			if (coinMultiplier < 10)
				coinMultiplier += 1;
		}
		else if (Input.GetKeyUp (KeyCode.Comma))
			Debug.Log (coinMultiplier);
		else if (Input.GetKeyUp (KeyCode.Space))
		{
			if (SceneManager.GetActiveScene().name == "Menu")
				SceneManager.LoadScene ("Survival");
		}
#endif
		if (Input.GetKeyUp (KeyCode.R))
			Reset ();

		/* Disabled scripts #1: Forgot what these functions are for
		if (SceneManager.GetActiveScene().name == "Menu")
			sceneChangeScan = true;
		
		if (sceneChangeScan)
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		else
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		*/
			
		if (SceneManager.GetActiveScene ().name == "Menu") {
			if (menuAudioManager == null)
				menuAudioManager = GameObject.Find ("Menu Audio Manager").GetComponent<AudioSource> ();

			if (menuUI == null)
				menuUI = GameObject.Find ("EventSystem").GetComponent<MenuUI> ();

			if (enemySpawning)
				enemySpawning = false;
			
			if (isWaveBreak)
				isWaveBreak = false;
		}

		else
		{
			if (inventory == null)
				inventory = GameObject.Find ("Panel Weapon Select").GetComponent<Inventory> ();

			if (pauser == null)
				pauser = GameObject.Find ("EventSystem").GetComponent<Pauser> ();
			
			if (SceneManager.GetActiveScene ().name == "Survival") {
				spawner = GameObject.Find ("Spawner").GetComponent<Spawner> ();
				gameUI = GameObject.Find ("EventSystem").GetComponent<GameUI> ();

				// temp scripts to prevent shieldLevel from going less than 0
				if (shieldLevel < 0)
					shieldLevel = 0;

				if (!dead) {
					if (gun == null) {
						gun = GameObject.Find ("Gun").GetComponent<Gun> ();
						gun.ChangeWeapon (equippedBazookaInGunScript);
					}
				}

				if ((!isWaveBreak && spawner.limitReached && spawner.enemiesOnScene.Length <= 0) || !isWaveBreak && wave == 0)
					WaveBreak ();
			}
		}
	}

	void WaveBreak ()
	{
		enemySpawning = false;
		isWaveBreak = true;
		gameUI.WaveBreakPopup ();
	}

	public void WaveStart ()
	{
		wave += 1;
		spawner.spawnCounter = 0;
		enemySpawning = true;
		isWaveBreak = false;

		if (SceneManager.GetActiveScene ().name == "Survival")
			coinMultiplier = wave;
		
		gameUI.ChangeWaveText ();
	}

	// Disabled scripts #2: Forgot what these functions are for, but somehow the death and coin pickup mechanics won't work without this. What? Where I've been all this time? My god, I've wasted my life.
	// /*
	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	} 

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Survival") {
			sceneChangeScan = false;
			Start ();
		}
	}
	// */

	public void Reset ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void LoseLife ()
	{
		if (SceneManager.GetActiveScene ().name != "Survival") {
			lives -= 1;
			livetxt.text = lives + "";
			liveshtxt.text = lives + "";
		} 
		else
			lives = 0;

		enemySpawning = false;
		spawner.limitReached = false;
		dead = true;

		/* Disabled scripts #3: These scripts are for 2nd enemy type (rocket launcher alien)
		if (GameObject.Find("SafeZone1").activeInHierarchy)
			GameObject.Find ("SafeZone1").GetComponent<TriggerOnExit>().i = 0;
		else if (GameObject.Find("SafeZone2").activeInHierarchy)
			GameObject.Find ("SafeZone2").GetComponent<TriggerOnExit>().i = 0;
		rocketTrigger1.i = 0;
		rocketTrigger2.i = 0;
		*/

		GameOverCheck ();
	}

	public void AddPoint()
	{
		gameAudioManager.PlayOneShot (scoreClip, 1);
		points += coinValue * coinMultiplier;
	}

	public void UpdatePoint ()
	{
		pointtxt.text = points + "";
		pointshtxt.text = points + "";
	}

	void GameOverCheck ()
	{
		if (lives > 0)
			StartCoroutine ("Respawn");
		else if (lives <= 0) {
			gotext.text = "GAME OVER";
			goshtext.text = "GAME OVER";

			// StartCoroutine ("ReloadGame");
			pauser.StartCoroutine ("BackToMenu");
		}
	}

	// Only used on Story mode
	IEnumerator Respawn ()
	{			
		dead = false;

		yield return new WaitForSeconds(2);

		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MyCamera>().enabled = true;

		Instantiate (playerPrefab, respawner.transform.position, respawner.transform.rotation);

		if (bazookaLevel < 1) {
//			GameObject.Find ("FireButton").GetComponent<Button>().interactable = false;
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = false;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = false;
		}
		else if (bazookaLevel >= 1) {
//			GameObject.Find ("FireButton").GetComponent<Button>().interactable = true;
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = true;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = true;
		}
		if (ResetOnDemand)
			Instantiate (fallingPlatPrefab, fallingPlatPrefab.transform.position, Quaternion.identity);
		
		mobileUIScript.Start ();
	}

	IEnumerator ReloadGame()
	{			
		yield return new WaitForSeconds(3);
		dead = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void ObjectsResetOnDemand()
	{
		ResetOnDemand = true;
	}

	public void BuyMagnet ()
	{
		coinMagnet = true;

	}
	public void BuyBazooka()
	{
		if (bazookaLevel < 3) {
			if (points >= bazookaPrices [bazookaLevel]) {
				points -= bazookaPrices [bazookaLevel];
				bazookaLevel += 1;

				if (SceneManager.GetActiveScene ().name != "Menu") {
					gameAudioManager.PlayOneShot (buyClip, 1);
					UpdatePoint ();
				}
				else
					menuAudioManager.PlayOneShot (buyClip, 1);
			}
		}

		Debug.Log ("Bazooka Level: " + bazookaLevel);
		if (bazookaLevel >= 1 && SceneManager.GetActiveScene().name != "Menu") {
			// GameObject.Find ("FireButton").GetComponent<Button>().interactable = true;
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = true;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = true;
		}

		if (bazookaLevel > 0)
			equippedBazookaInGunScript = "Basic";

		if (SceneManager.GetActiveScene ().name != "Menu") {
			if (slot1 != "Basic" && slot2 != "Basic")
				gun.ChangeWeapon ("Basic");
		}

		if (slot1 != "Basic" && slot2 != "Basic") {
			if (isSurvival) {
				if (!isStory) {
					if (bazookaLevel > 1) {
						if (SceneManager.GetActiveScene ().name == "Menu")
							menuUI.EquipNotifyOpen ();
						else
							inventory.EquipNotifyOpen ();
					}
				}
			}
			else {
				if (isStory) {
					if (bazookaLevel > 0) {
						if (SceneManager.GetActiveScene ().name == "Menu")
							menuUI.EquipNotifyOpen ();
						else
							inventory.EquipNotifyOpen ();
					}
				}
			}
		}
	}

	public void BuyTRBazooka()
	{
		if (TRBazookaLevel < 3) {
			if (points >= TRBazookaPrices [TRBazookaLevel]) {
				points -= TRBazookaPrices [TRBazookaLevel];
				TRBazookaLevel += 1;					

				if (SceneManager.GetActiveScene ().name != "Menu") {
					gameAudioManager.PlayOneShot (buyClip, 1);
					UpdatePoint ();
				}
				else
					menuAudioManager.PlayOneShot (buyClip, 1);
			}
		}

		Debug.Log ("Triple Bazooka Level: " + TRBazookaLevel);
		if (TRBazookaLevel >= 1 && SceneManager.GetActiveScene().name != "Menu") {
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = true;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = true;
		}
			
		if (TRBazookaLevel > 0)
			equippedBazookaInGunScript = "Triple";

		if (SceneManager.GetActiveScene ().name != "Menu") {
			if (slot1 != "Triple" && slot2 != "Triple")
				gun.ChangeWeapon ("Triple");			
		}

		if (slot1 != "Triple" && slot2 != "Triple") {
			if (TRBazookaLevel > 0) {
				if (SceneManager.GetActiveScene ().name == "Menu")
					menuUI.EquipNotifyOpen ();
				else
					inventory.EquipNotifyOpen ();
			}
		}
	}

	public void BuyRFBazooka()
	{
		if (RFBazookaLevel < 3) {
			if (points >= RFBazookaPrices [RFBazookaLevel]) {
				points -= RFBazookaPrices [RFBazookaLevel];
				RFBazookaLevel += 1;

				if (SceneManager.GetActiveScene ().name != "Menu") {
					gameAudioManager.PlayOneShot (buyClip, 1);
					UpdatePoint ();
				}
				else
					menuAudioManager.PlayOneShot (buyClip, 1);
			}
		}

		Debug.Log ("Rapid-fire Bazooka Level: " + RFBazookaLevel);
		if (RFBazookaLevel >= 1 && SceneManager.GetActiveScene().name != "Menu") {
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = true;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = true;
		}

		if (RFBazookaLevel > 0)
			equippedBazookaInGunScript = "Rapid-fire";

		if (SceneManager.GetActiveScene ().name != "Menu") {
			if (slot1 != "Rapid-fire" && slot2 != "Rapid-fire")
				gun.ChangeWeapon ("Rapid-fire");
		}

		if (slot1 != "Rapid-fire" && slot2 != "Rapid-fire") {
			if (RFBazookaLevel > 0) {
				if (SceneManager.GetActiveScene ().name == "Menu")
					menuUI.EquipNotifyOpen ();
				else
					inventory.EquipNotifyOpen ();
			}
		}
	}

	public void BuySniperBazooka()
	{
		if (SniperBazookaLevel < 3) {
			if (points >= SniperBazookaPrices [SniperBazookaLevel]) {
				points -= SniperBazookaPrices [SniperBazookaLevel];
				SniperBazookaLevel += 1;					

				if (SceneManager.GetActiveScene ().name != "Menu") {
					gameAudioManager.PlayOneShot (buyClip, 1);
					UpdatePoint ();
				}
				else
					menuAudioManager.PlayOneShot (buyClip, 1);
			}
		}

		Debug.Log ("Sniper Bazooka Level: " + SniperBazookaLevel);
		if (SniperBazookaLevel >= 1 && SceneManager.GetActiveScene().name != "Menu") {
			GameObject.Find ("Bazooka").GetComponent<SpriteRenderer> ().enabled = true;
			GameObject.Find ("Gun").GetComponent<Gun> ().enabled = true;
		}

		if (SniperBazookaLevel > 0)
			equippedBazookaInGunScript = "Sniper";

		if (SceneManager.GetActiveScene ().name != "Menu") {
			if (slot1 != "Sniper" && slot2 != "Sniper")
				gun.ChangeWeapon ("Sniper");			
		}

		if (slot1 != "Sniper" && slot2 != "Sniper") {
			if (SniperBazookaLevel > 0) {
				if (SceneManager.GetActiveScene ().name == "Menu")
					menuUI.EquipNotifyOpen ();
				else
					inventory.EquipNotifyOpen ();
			}
		}
	}

	public void On1UPBought()
	{
		lives += 1;
		livetxt.text = lives + "";
		liveshtxt.text = lives + "";

		if (SceneManager.GetActiveScene().name == "Menu")
			menuAudioManager.PlayOneShot (buyClip, 1);
		else
			gameAudioManager.PlayOneShot (buyClip, 1);
	}

	public void BuyMultiplier ()
	{
		if (coinMultiplier < 10) {
			if (points >= multiPrices[coinMultiplier]) {
				points -= multiPrices[coinMultiplier];
				coinMultiplier += 1;

				if (SceneManager.GetActiveScene ().name != "Menu") {
					gameAudioManager.PlayOneShot (buyClip, 1);
					UpdatePoint ();
				}
				else
					menuAudioManager.PlayOneShot (buyClip, 1);
			}
		}		
	}

	public void BuyShield ()
	{
		if (shieldLevel < 3) {
			if (SceneManager.GetActiveScene ().name != "Menu") {
				if (shieldLevel == 0)
					CreateShield ();
			}

			if (points >= shieldPrices[shieldLevel]) {
				points -= shieldPrices[shieldLevel];
				shieldLevel += 1;

				if (SceneManager.GetActiveScene ().name != "Menu") {
					shield = GameObject.FindWithTag ("Shield").GetComponent<Shield> ();
					shield.ChangeSprite ();

					gameAudioManager.PlayOneShot (buyClip, 1);
					UpdatePoint ();
				}
				else
					menuAudioManager.PlayOneShot (buyClip, 1);
			}	
		}
	}

	public void CreateShield ()
	{
		Transform playerTF = GameObject.FindWithTag ("Player").transform;
		GameObject shield = Instantiate (shieldPrefab, playerTF.position, Quaternion.identity) as GameObject;
		shield.transform.SetParent (playerTF);
		shield.transform.localScale = new Vector3 (1, 1, 1);
	}
}