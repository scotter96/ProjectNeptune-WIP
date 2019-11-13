using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

	public float time;
	public GameObject coinPrefab;
	Transform leftBlocker, rightBlocker;
	GameObject theCoin;
	int coinCounts;
	float leftBlockerX, rightBlockerX;
	GM GC;

	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GM").GetComponent<GM> ();
		Destroy (gameObject, time);
		coinCounts = GC.coinMultiplier;
		leftBlocker = GameObject.Find("Player_Left").transform;
		rightBlocker = GameObject.Find("Player_Right").transform;
		leftBlockerX = leftBlocker.position.x;
		rightBlockerX = rightBlocker.position.x;
		StartCoroutine (SpawnCoin ());
	}

	IEnumerator SpawnCoin ()
	{
		for (int i=0; i<1; i++) {
			theCoin = Instantiate (coinPrefab, transform.position, Quaternion.identity) as GameObject;
			theCoin.AddComponent<Rigidbody2D> ();
			theCoin.GetComponent<Rigidbody2D> ().gravityScale = 1f;
			theCoin.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-100f, 100f), 700f));
			theCoin.GetComponent<Rigidbody2D>().isKinematic = false;
			yield return new WaitForSeconds (0.1f);
			if (theCoin.GetComponent<CoinDestroyByTime>().destroyed == false)
				theCoin.GetComponent<CircleCollider2D> ().isTrigger = false;
			float coinX = theCoin.transform.position.x;
			if (coinX <= leftBlockerX || coinX >= rightBlockerX)
				theCoin.GetComponent<CoinPickup>().HitPlayer();
		}
	}
}