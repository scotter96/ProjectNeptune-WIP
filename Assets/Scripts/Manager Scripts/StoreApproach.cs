using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreApproach : MonoBehaviour
{
	public GameObject shopMenu;

	public SpriteRenderer renderer;

	public Sprite openSprite;
	public Sprite closedSprite;

	public bool isApproached;

	GM gameManager;

	void Awake ()
	{
		gameManager = GameObject.Find ("GM").GetComponent<GM> ();

		RectTransform shopRT = shopMenu.GetComponent<RectTransform> ();

		Button[] childButtons = shopMenu.GetComponentsInChildren<Button> ();

		for (int i = 0; i < childButtons.Length; i++) {
			RectTransform childI = childButtons [i].gameObject.GetComponent<RectTransform> ();
			childI.sizeDelta = new Vector2 (shopRT.sizeDelta.x * 0.325f, shopRT.sizeDelta.y * 0.34f);

			if (i < 2) {
				for (int j = i; j < 2; j++) {
					int k = 0;
					int n = 1;

					if (j == 0) {
						k = 20;
						n *= 1;
					}
					else {
						k = 0;
						n *= -1;
					}

					RectTransform childJ = childButtons [j].gameObject.GetComponent<RectTransform> ();
					childJ.anchoredPosition = new Vector2 (-shopRT.sizeDelta.x * 0.275f, ((shopRT.sizeDelta.y * 0.2f) + k) * n);
				}
			}
			else
			{
				for (int j = 2; j < childButtons.Length; j++) {
					int k = 0;
					int n = 1;

					if (j == 2) {
						k = 20;
						n *= 1;
					} 
					else {
						k = 0;
						n *= -1;
					}

					RectTransform childJ = childButtons [j].gameObject.GetComponent<RectTransform> ();
					childJ.anchoredPosition = new Vector2 (shopRT.sizeDelta.x * 0.275f, ((shopRT.sizeDelta.y * 0.2f) + k) * n);
				}
			}
		}
		SetShopMenu (false);
	}

	void Update ()
	{
		if (gameManager.isWaveBreak)
			renderer.sprite = openSprite;
		else
			renderer.sprite = closedSprite;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			if (gameManager.isWaveBreak)
				SetShopMenu (true);
			else
				SetShopMenu (false);
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			SetShopMenu (false);
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag == "Player") {
			if (!gameManager.isWaveBreak) {
				if (shopMenu.activeInHierarchy)
					SetShopMenu (false);
			} 
			else {
				if (!shopMenu.activeInHierarchy)
					SetShopMenu (true);
			}
		}
	}

	public void SetShopMenu (bool status)
	{
		shopMenu.SetActive (status);
		isApproached = status;
	}
}
