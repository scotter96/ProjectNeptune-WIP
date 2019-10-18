using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	public GameObject[] pickupPrefabs;

	int wave = 0;
	int pickupNumber;
	int maxPickupsOnScene;

	float xPos, yPos, lastXPos;

	bool pickupSpawned;

	GM gm;

	void Awake ()
	{
		gm = GameObject.Find ("GM").GetComponent<GM> ();

		pickupSpawned = false;
	}

	void Update ()
	{		
		maxPickupsOnScene = Random.Range (1, 4);

		if (wave != gm.wave || wave == 0) {
			wave = gm.wave;
			pickupSpawned = false;
		}

		else
		{
			if (wave % 1 == 0) // ? Be able to spawn pickups if wave number is dividable by 3
			{
				if (!pickupSpawned) // ? Be able to spawn pickups on wave start, indicated by pickupSpawned variable still false
				{
					for (int i = 0; i < maxPickupsOnScene; i++) {
						pickupNumber = Random.Range (0, pickupPrefabs.Length);

						if (Mathf.Approximately (xPos, lastXPos) || xPos == 0) {
							xPos = Random.Range (-35, 36);
							yPos = -4.5f;
						} 
						else {
							Instantiate (pickupPrefabs [pickupNumber], new Vector3 (xPos, yPos), Quaternion.identity);
							lastXPos = xPos;
						}
					}
					
					pickupSpawned = true;
				}
			}
		}
	}
}
