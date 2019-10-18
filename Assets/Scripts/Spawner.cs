using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	GM gameManager;

	public bool canSpawn;
	public bool limitReached;

	float time;

	public int initialEnemyCounts = 5;
	public int spawnCounter;
	int maxEnemies, spawnDelay;

	public GameObject[] enemyPrefabs;		// Array of enemy prefabs.
	public GameObject[] enemiesOnScene;		// Array of enemy objects in active scene.

	void Awake ()
	{
		gameManager = GameObject.Find ("GM").GetComponent<GM> ();
	}

	void Start ()
	{
		time = 0;
		spawnCounter = 0;
		limitReached = false;
	}

	void Update ()
	{
		time += Time.deltaTime;
		maxEnemies = initialEnemyCounts + (2 * (gameManager.wave - 1));
		enemiesOnScene = GameObject.FindGameObjectsWithTag ("Enemy");

		/* Disabled scripts #4: Debugging scripts
		Debug.Log ("Enemies on Scene: " + enemiesOnScene.Length);
		Debug.Log ("Total enemies spawned: " + spawnCounter);
		Debug.Log ("Number of enemies < max: " + canSpawn);
		Debug.Log ("Is in wave: " + gameManager.enemySpawning);
		Debug.Log ("Max enemies this wave: " + maxEnemies);
		Debug.Log ("This is wave " + gameManager.wave);
		Debug.Log ("Has the limit reached? " + limitReached);
		*/

		foreach (GameObject enemy in enemiesOnScene) {
			if (enemy.transform.position.x <= -41 || enemy.transform.position.x >= 41)
				canSpawn = false;
			else
				canSpawn = true;
		}

		if (Input.GetKeyDown (KeyCode.Space))
			spawnCounter = 0;

		if (enemiesOnScene.Length == 0)
			canSpawn = true;

		if (spawnCounter >= maxEnemies)
			limitReached = true;
		else
			limitReached = false;

		if (gameManager.wave < 6)
			// returns either 3, 4, or 5
			spawnDelay = Random.Range (3, 6); 
		else if (gameManager.wave >= 6 && gameManager.wave < 11)
			// returns either 3 or 4
			spawnDelay = Random.Range (3, 5); 
		else if (gameManager.wave >= 11 && gameManager.wave < 16)
			// returns either 2, 3, or 4
			spawnDelay = Random.Range (2, 5); 
		else if (gameManager.wave >= 16 && gameManager.wave < 21)
			// returns either 2 or 3
			spawnDelay = Random.Range (2, 4); 
		else if (gameManager.wave >= 21 && gameManager.wave < 26)
			// returns either 1, 2, or 3
			spawnDelay = Random.Range (1, 4); 
		else if (gameManager.wave >= 26 && gameManager.wave < 31)
			// returns either 1 or 2
			spawnDelay = Random.Range (1, 3); 
		else if (gameManager.wave >= 31 && gameManager.wave < 41)
			// returns 1
			spawnDelay = 1; 
		
		if (time >= spawnDelay)
		{
			time = 0;
			/* int randomPosDelay = Random.Range (1, 5);
			Invoke ("RandomizeSpawnPos", randomPosDelay); */
			RandomizeSpawnPos ();
			if (canSpawn && enemiesOnScene.Length < maxEnemies && gameManager.enemySpawning && !limitReached)
				//Invoke ("Spawn", spawnDelay);
				Spawn ();
		}
	}

	void RandomizeSpawnPos ()
	{
		float xPos;
		int spawnFromLeft = Random.Range (0, 2);
		if (spawnFromLeft == 0)	// Spawns from right
			xPos = 43f;
		else
			xPos = -43f;
		transform.position = new Vector3 (xPos, transform.position.y, transform.position.z);
	}

	void Spawn ()
	{
		// Instantiate a random enemy.
		spawnCounter += 1;
		// used for multiple prefab of enemies:	int enemyIndex = Random.Range(0, enemies.Length);
		int enemyIndex = 0;
		Instantiate(enemyPrefabs[enemyIndex], transform.position, transform.rotation);
	}
}
