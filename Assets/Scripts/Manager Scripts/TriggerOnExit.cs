using UnityEngine;
using System.Collections;

public class TriggerOnExit : MonoBehaviour {

//	public RocketLauncher RL;
	public GameObject target;
	public GameObject rocket;
	public int i = 2;
	public bool destroyed = false;

	void OnTriggerEnter2D (Collider2D col)
	{
		if ((col.tag == "Player") && target.GetComponent<EnemyShooterIdle> ().destroyed == false) {
			i--;
			if (i==1)
			{
				target.AddComponent<RocketLauncher>();
				target.GetComponent<RocketLauncher>().rocketSpawn = target.transform.GetChild(1);
				target.GetComponent<RocketLauncher>().rocketPrefab = rocket;
			}
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if ((col.tag == "Player") && target.GetComponent<EnemyShooterIdle> ().destroyed == false)
		{
			i++;
			Destroy (target.GetComponent<RocketLauncher>());
		}
	}

	public void OnDestroy ()
	{
		destroyed = true;
		Destroy (gameObject, 0.5f);
	}
}
