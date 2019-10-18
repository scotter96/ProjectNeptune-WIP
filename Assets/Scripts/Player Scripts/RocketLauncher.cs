using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour 
{
	public GameObject rocketPrefab;
	public float speed = 25f;
	public Transform rocketSpawn;
	GameObject bulletInstance;
	
	public void Start ()
	{
		InvokeRepeating ("Shoot", 0.1f, 2f);
	}

	public void OnStop ()
	{
		CancelInvoke ();
	} 

	void Shoot()
	{
		GetComponent<AudioSource> ().Play ();
		bulletInstance = Instantiate
		(rocketPrefab, rocketSpawn.transform.position, Quaternion.Euler (0, 0, 180)) as GameObject;
		bulletInstance.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, 0);
	}
}
