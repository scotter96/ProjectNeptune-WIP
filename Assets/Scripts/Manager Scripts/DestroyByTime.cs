using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

	public float time;
	public GameObject coinPrefab;
	GameObject theCoin;
	int coinCounts;
	GM GC;

	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GM").GetComponent<GM> ();
		Destroy (gameObject, time);
		coinCounts = GC.coinMultiplier;
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
		}
	}
}