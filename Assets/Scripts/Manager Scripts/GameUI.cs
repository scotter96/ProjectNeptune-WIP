using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

	[HideInInspector]
	public float WB_Popup_time = 0;

	public float popupTimeoutFirstPlay = 5;
	public float popupTimeout = 15;

	public Text waveShadow;
	public Text waveText;
	public Text waveBreakShadow;
	public Text waveBreakGO;
	public Text waveTimerShadow;
	public Text waveTimer;

	public RectTransform weaponPanel, shopPopup;

	bool WB_Popup;

	Rigidbody2D playerRB;

	GM gameManager;

	// Use this for initialization
	void Awake () {
		if (playerRB == null)
			playerRB = GameObject.FindWithTag ("Player").GetComponent<Rigidbody2D> ();

		if (gameManager == null)
			gameManager = GameObject.Find ("GM").GetComponent<GM> ();

		// To make sure if this is survival mode, the starting wave is 0
		gameManager.wave = 0;
		
		float SDx = Screen.width * 0.15f;
		float SDy = Screen.height * 0.24f;
		float defaultXPos = SDx / 2;
		float defaultYPos = SDy / 2;

		// SHOP POPUP PANEL SETTINGS
		shopPopup.sizeDelta = new Vector2 (Screen.width * 0.4f, Screen.height * 0.4f);		
		// END OF SHOP POPUP PANEL SETTINGS

		// SCROLLABLE PANEL SETTINGS
		int leftPadding = Mathf.RoundToInt (shopPopup.sizeDelta.x / 8);
		int topPadding = Mathf.RoundToInt (shopPopup.sizeDelta.y / 10);
		int bottomPadding = Mathf.RoundToInt (shopPopup.sizeDelta.y * 0.225f);
		int horizontalSpacing = Mathf.RoundToInt ((shopPopup.sizeDelta.x * 0.325f) / 4);
		int verticalSpacing = bottomPadding;

		HorizontalLayoutGroup[] shopHorizontals = shopPopup.GetComponentsInChildren<HorizontalLayoutGroup>();
		foreach (HorizontalLayoutGroup horizontal in shopHorizontals)
		{
			horizontal.padding.left = leftPadding;
			horizontal.spacing = horizontalSpacing;
		}

		VerticalLayoutGroup shopVertical = shopPopup.GetComponentInChildren<VerticalLayoutGroup>();
		shopVertical.padding.top = topPadding;
		shopVertical.padding.bottom = bottomPadding;
		shopVertical.spacing = verticalSpacing;
		// END OF SCROLLABLE PANEL SETTINGS

		weaponPanel.sizeDelta = new Vector2 (Screen.width * 0.3f, SDy);
		weaponPanel.anchoredPosition = new Vector2 (-(weaponPanel.sizeDelta.x / 2), -(weaponPanel.sizeDelta.y / 2));

		RectTransform[] weaponButtonsRT = weaponPanel.GetComponentsInChildren <RectTransform> ();

		float buttonsAP = defaultYPos;

		for (int i = 1; i < weaponButtonsRT.Length; i++) {
			weaponButtonsRT[i].sizeDelta = new Vector2 (SDx, SDy);
			weaponButtonsRT[i].anchoredPosition = new Vector2 (-(buttonsAP + (SDx * (i-1))), -buttonsAP);
		}

		RectTransform[] transforms = GameObject.Find ("Canvas").GetComponentsInChildren<RectTransform> ();

		foreach (RectTransform RT in transforms) {
			if (!RT.name.Contains ("Panel") && !RT.name.Contains ("Shop") && !RT.name.Contains ("Layout")) {
				 RT.sizeDelta = new Vector2 (SDx, SDy);

				if (RT.name.EndsWith ("Jump"))
					RT.anchoredPosition = new Vector2 (-defaultXPos - SDx, defaultYPos);
				else if (RT.name.EndsWith ("Fire"))
					RT.anchoredPosition = new Vector2 (-defaultXPos, defaultYPos);
				else if (RT.name.EndsWith ("Right"))
					RT.anchoredPosition = new Vector2 (defaultXPos + SDx, defaultYPos);
				else if (RT.name.StartsWith ("Button"))
					RT.anchoredPosition = new Vector2 (defaultXPos, defaultYPos);
				else if (RT.name.StartsWith ("Coin"))
					RT.anchoredPosition = new Vector2 (defaultXPos / 2, -defaultYPos / 2);
				/* for pre-alpha release only
				else if (RT.name.StartsWith ("Version")) {
					RT.anchoredPosition = new Vector2 (-defaultXPos, -defaultYPos);
					if (!RT.name.EndsWith ("Shadow"))
						RT.anchoredPosition = new Vector2 (-defaultXPos - 5, -defaultXPos + 5);
				} */
				else if (RT.name.StartsWith ("Wave")) {
					if (RT.name.Contains ("Break")) {
						if (RT.name.Contains ("Timer")) {
							RT.anchoredPosition = new Vector2 (0, -SDy);
							if (!RT.name.EndsWith ("Shadow"))
								RT.anchoredPosition = new Vector2 (0, -defaultXPos + 5);
						}
					} else {
						RT.anchoredPosition = new Vector2 (0, -defaultYPos);
						if (!RT.name.EndsWith ("Shadow"))
							RT.anchoredPosition = new Vector2 (0, -defaultXPos + 5);
					}
				} else if (RT.name.StartsWith ("Points")) {
					RT.anchoredPosition = new Vector2 ((SDx * 2 - SDx / 3), -SDy * 0.75f);
					if (!RT.name.EndsWith ("Shadow"))
						RT.anchoredPosition = new Vector2 ((SDx * 2 - SDx / 3) - 5, (-SDy * 0.75f) + 5);
				} else if (RT.name.StartsWith ("GO")) {
					RT.anchoredPosition = new Vector2 (-5, -5);
					if (!RT.name.EndsWith ("Shadow"))
						RT.anchoredPosition = new Vector2 (-5, 5);
				}
			}
		}

		ChangeWaveText ();

		// temporary scripts to disable white wave indicator texts (Game UIs still shit in my phone)
		waveTimer.gameObject.SetActive (false);
		waveText.gameObject.SetActive (false);

		/* for pre-alpha release only
		GameObject.Find ("VersionInfo").SetActive (false);
		*/
	}

	void Update()
	{
		if (playerRB == null && !gameManager.dead)
			playerRB = GameObject.FindWithTag ("Player").GetComponent<Rigidbody2D> ();

		if (gameManager == null)
			gameManager = GameObject.Find ("GM").GetComponent<GM> ();

		// PASSIVE MODE: TO FREEZE THE PLAYER AND IGNORE THE ENEMIES (FOR DEBUGGING PURPOSES ONLY)
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.Home))
		{
			playerRB.simulated = !playerRB.simulated;
		}

		if (Input.GetKeyDown (KeyCode.End))
		{
			WB_Popup = !WB_Popup;
		}
		#endif

		// Debug.Log ("Popup? " + WB_Popup);
		// Debug.Log ("Popup time left: " + WB_Popup_time);

		if (WB_Popup) {
			WB_Popup_time -= Time.deltaTime;

			if (WB_Popup_time <= 0) {
				waveBreakShadow.text = "";
				waveBreakGO.text = "";
				waveTimerShadow.text = "";
				waveTimer.text = "";

				WB_Popup_time = 0;
				WB_Popup = false;
				gameManager.WaveStart ();
			}
			else {
				waveTimerShadow.text = "Next wave starts in: " + WB_Popup_time.ToString ("0");
				waveTimer.text = "Next wave starts in: " + WB_Popup_time.ToString ("0");
			}
		}
	}

	public void ChangeWaveText()
	{
		waveShadow.text = "wave " + gameManager.wave;
		waveText.text = "wave " + gameManager.wave;
	}

	public void WaveBreakPopup ()
	{
		WB_Popup = true;

		if (gameManager.wave == 0)
			WB_Popup_time = popupTimeoutFirstPlay;
		else {
			WB_Popup_time = popupTimeout;
			waveBreakShadow.text = "Next Wave!";
			waveBreakGO.text = "Next Wave!";
		}		
	}
}