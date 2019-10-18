using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {

	GM gm;
	StateManager stateManager;

	public Sprite[] bazookaSprites;
	public Sprite[] RFBazookaSprites;
	public Sprite[] TRBazookaSprites;
	public Sprite[] SNBazookaSprites;
	public Sprite[] multiSprites;
	public Sprite[] shieldSprites;

	public RectTransform equipNotify;

	public Text notifyText;

	public Text equippedWeaponsText;

	public Text highestWaveText;

	Button notifyButton;

	int bazookaPrice, RFBazookaPrice, TRBazookaPrice, SNBazookaPrice, shieldPrice, multiPrice;

	void Awake ()
	{		
		equipNotify.sizeDelta = new Vector2 (Screen.width / 2.5f, Screen.height / 4);

		RectTransform textRT = equipNotify.transform.GetChild (0).GetComponent<RectTransform> ();
		textRT.sizeDelta = new Vector2 (0, 0);
		textRT.anchoredPosition = new Vector2 (0, -16);

		RectTransform[] RTs = equipNotify.GetComponentsInChildren<RectTransform> ();
		foreach (RectTransform rt in RTs) {
			if (rt.name.EndsWith ("1")) {
				rt.sizeDelta = new Vector2 (0, 0);
				rt.anchoredPosition = new Vector2 (-96, -48);
			} else if (rt.name.EndsWith ("2")) {
				rt.sizeDelta = new Vector2 (0, 0);
				rt.anchoredPosition = new Vector2 (96, -48);
			}
		}

		equipNotify.gameObject.SetActive (false);
	}

	void Start () {		
		Text title = GameObject.Find ("Text_Main_Title").GetComponent<Text> ();
		title.text = " Neptune Alpha v" + Application.version;

		gm = GameObject.Find ("GM").GetComponent<GM> ();
		DontDestroyOnLoad (gm);

		stateManager = GameObject.Find ("GM").GetComponent<StateManager> ();

		InitUI();

		// To disable story mode button on main menu (temporary code to test overall game on version 0.3)
		if (gm.isSurvival)
			GameObject.Find ("Button_Start_Story").SetActive (false);
			
		highestWaveText.text = "Highest wave: " + gm.highScore;
	}

	public void PointCheat ()
	{
		gm.points += 5000;
		ReinitUI ();
	}

	void ReinitUI ()
	{
		stateManager.bazookaLevel = gm.bazookaLevel;
		stateManager.coinMultiplier = gm.coinMultiplier;
		stateManager.points = gm.points;
		stateManager.TRBazookaLevel = gm.TRBazookaLevel;
		stateManager.RFBazookaLevel = gm.RFBazookaLevel;
		stateManager.SNBazookaLevel = gm.SniperBazookaLevel;
		stateManager.shieldLevel = gm.shieldLevel;
		stateManager.equippedWeapon = gm.equippedBazookaInGunScript;
		stateManager.highScore = gm.highScore;

		//pricing
		bazookaPrice = gm.bazookaPrices [gm.bazookaLevel];
		/* Disabled scripts #8: Currently unused item for survival mode
		multiPrice = gm.multiPrices [gm.coinMultiplier]; */
		RFBazookaPrice = gm.RFBazookaPrices [gm.RFBazookaLevel];
		TRBazookaPrice = gm.TRBazookaPrices [gm.TRBazookaLevel];
		SNBazookaPrice = gm.SniperBazookaPrices [gm.SniperBazookaLevel];
		shieldPrice = gm.shieldPrices [gm.shieldLevel];

		int ScrWid = Screen.width;
		int ScrHgh = Screen.height;
		int SDy = Mathf.RoundToInt (ScrHgh / 6);

		RectTransform menuBGRT = GameObject.Find ("Image_BG").GetComponent<RectTransform> ();
		menuBGRT.sizeDelta = new Vector2 (ScrWid, ScrHgh);

		GameObject storeGO = GameObject.Find ("Text_Store_Title");
		RectTransform storeTF = storeGO.transform as RectTransform;
		storeTF.anchoredPosition = new Vector2 (0, ScrHgh / 2.5f);

		Text coinTxt = GameObject.Find ("Text_Coins_Info").GetComponent<Text> ();
		coinTxt.text = "" + gm.points;
		RectTransform coinTxtTF = coinTxt.transform as RectTransform;
		RectTransform coinImg = GameObject.Find ("Image_Coin").transform as RectTransform;

		RectTransform bazookaTF = GameObject.Find ("Button_Bazooka_Store").transform as RectTransform;
		bazookaTF.GetComponent<Image> ().sprite = bazookaSprites [gm.bazookaLevel];

		RectTransform TRBazookaTF = GameObject.Find ("Button_TR-Bazooka_Store").transform as RectTransform;
		TRBazookaTF.GetComponent<Image> ().sprite = TRBazookaSprites [gm.TRBazookaLevel];

		RectTransform RFBazookaTF = GameObject.Find ("Button_RF-Bazooka_Store").transform as RectTransform;
		RFBazookaTF.GetComponent<Image> ().sprite = RFBazookaSprites [gm.RFBazookaLevel];

		RectTransform SNBazookaTF = GameObject.Find ("Button_SN-Bazooka_Store").transform as RectTransform;
		SNBazookaTF.GetComponent<Image> ().sprite = SNBazookaSprites [gm.SniperBazookaLevel];

		RectTransform shieldTF = GameObject.Find ("Button_Shield_Store").transform as RectTransform;
		shieldTF.GetComponent<Image> ().sprite = shieldSprites[gm.shieldLevel];

		/* Disabled scripts #8: Currently unused item for survival mode
		RectTransform multiTF = GameObject.Find ("Button_Multiplier_Store").transform as RectTransform;
		multiTF.GetComponent<Image> ().sprite = multiSprites[gm.coinMultiplier - 1];
		*/

		Text bazookaTxt = GameObject.Find ("Text_Bazooka_Store").GetComponent<Text> ();
		bazookaTxt.fontSize = 36;

		Text RFBazookaTxt = GameObject.Find ("Text_RF-Bazooka_Store").GetComponent<Text> ();
		RFBazookaTxt.fontSize = 36;

		Text TRBazookaTxt = GameObject.Find ("Text_TR-Bazooka_Store").GetComponent<Text> ();
		TRBazookaTxt.fontSize = 36;

		Text SNBazookaTxt = GameObject.Find ("Text_SN-Bazooka_Store").GetComponent<Text> ();
		SNBazookaTxt.fontSize = 36;

		Text shieldTxt = GameObject.Find ("Text_Shield_Store").GetComponent<Text> ();
		shieldTxt.fontSize = 36;

		/* Disabled scripts #8: Currently unused item for survival mode
		Text multiTxt = GameObject.Find ("Text_Multiplier_Store").GetComponent<Text> ();
		multiTxt.fontSize = 36;
		*/

		if (gm.bazookaLevel < 3)
			bazookaTxt.text = "BAZOOKA\nUseful for wiping out enemies!\nPrice: " + bazookaPrice + "\nLevel: " + (gm.bazookaLevel + 1);
		else
			bazookaTxt.text = "BAZOOKA\nUseful for wiping out enemies!\nPrice: N/A" + "\nLevel: Maxed Out!";

		if (gm.TRBazookaLevel < 3)
			TRBazookaTxt.text = "TRIPLE BAZOOKA\nTriple treat!\nPrice: " + TRBazookaPrice + "\nLevel: " + (gm.TRBazookaLevel + 1);
		else
			TRBazookaTxt.text = "TRIPLE BAZOOKA\nTriple treat!\nPrice: N/A" + "\nLevel: Maxed Out!";
		
		if (gm.RFBazookaLevel < 3)
			RFBazookaTxt.text = "RAPID-FIRED BAZOOKA\nYou can rapidly pull the trigger!\nPrice: " + RFBazookaPrice + "\nLevel: " + (gm.RFBazookaLevel + 1);
		else
			RFBazookaTxt.text = "RAPID-FIRED BAZOOKA\nYou can rapidly pull the trigger!\nPrice: N/A" + "\nLevel: Maxed Out!";

		if (gm.SniperBazookaLevel < 3)
			SNBazookaTxt.text = "SNIPER BAZOOKA\nThis bad boy can shoot through yer enemies!\nPrice: " + SNBazookaPrice + "\nLevel: " + (gm.SniperBazookaLevel+ 1);
		else
			SNBazookaTxt.text = "SNIPER BAZOOKA\nThis bad boy can shoot through yer enemies!\nPrice: N/A" + "\nLevel: Maxed Out!";
		
		if (gm.shieldLevel < 3)
			shieldTxt.text = "SHIELD\nProtecting your hero since 2018!\nPrice: " + shieldPrice + "\nLevel: " + (gm.shieldLevel + 1);
		else
			shieldTxt.text = "SHIELD\nProtecting your hero since 2018!\nPrice: N/A" + "\nLevel: Maxed Out!";
		
		/* Disabled scripts #8: Currently unused item for survival mode
		if (gm.coinMultiplier < 10)
			multiTxt.text = "COIN MULTIPLIER\nKa-ching!\nPrice: " + multiPrice + "\nLevel: " + (gm.coinMultiplier + 1);
		else
			multiTxt.text = "COIN MULTIPLIER\nKa-ching!\nPrice: N/A" + "\nLevel: Maxed Out!";
		*/
		
		RectTransform bazookaTxtTF = bazookaTxt.transform as RectTransform;
		bazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform TRBazookaTxtTF = TRBazookaTxt.transform as RectTransform;
		TRBazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform RFBazookaTxtTF = RFBazookaTxt.transform as RectTransform;
		RFBazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform SNBazookaTxtTF = SNBazookaTxt.transform as RectTransform;
		SNBazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform shieldTxtTF = shieldTxt.transform as RectTransform;
		shieldTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		/* Disabled scripts #8: Currently unused item for survival mode
		RectTransform multiTxtTF = multiTxt.transform as RectTransform;
		multiTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);
		*/

		Button[] buttons = GameObject.Find ("Canvas").GetComponentsInChildren<Button> ();
		foreach (Button button in buttons) {
			if (button.name.Contains ("Shield")) 
			{
				if (gm.shieldLevel < 3)
					button.interactable = true;
				else
					button.interactable = false;
			}

			/* Disabled scripts #8: Currently unused item for survival mode
			else if (button.name.Contains ("Multiplier")) {
				if (gm.coinMultiplier < 10)
					button.interactable = true;
				else
					button.interactable = false;
			}
			*/

			/* Disabled scripts #9: Old script dictates once the items' level is over certain amount, said button is rendered non-interactable
			else if (button.name.Contains ("_Bazooka"))
			{
				if (gm.bazookaLevel < 3)
					button.interactable = true;
				else
					button.interactable = false;
			}
			else if (button.name.Contains ("TR-Bazooka")) 
			{
				if (gm.TRBazookaLevel < 3)
					button.interactable = true;
				else
					button.interactable = false;
			}
			else if (button.name.Contains ("RF-Bazooka")) 
			{
				if (gm.RFBazookaLevel < 3)
					button.interactable = true;
				else
					button.interactable = false;
			}
			*/
		}

	}

	void InitUI ()
	{
		equippedWeaponsText.text = " Slot 1: " + gm.slot1 + "\n Slot 2: " + gm.slot2;

		stateManager.bazookaLevel = gm.bazookaLevel;
		stateManager.coinMultiplier = gm.coinMultiplier;
		stateManager.points = gm.points;
		stateManager.TRBazookaLevel = gm.TRBazookaLevel;
		stateManager.RFBazookaLevel = gm.RFBazookaLevel;
		stateManager.SNBazookaLevel = gm.SniperBazookaLevel;
		stateManager.shieldLevel = gm.shieldLevel;
		stateManager.equippedWeapon = gm.equippedBazookaInGunScript;
		stateManager.highScore = gm.highScore;

		// pricing
		bazookaPrice = gm.bazookaPrices [gm.bazookaLevel];
		/* Disabled scripts #8: Currently unused item for survival mode
		multiPrice = gm.multiPrices [gm.coinMultiplier]; */
		RFBazookaPrice = gm.RFBazookaPrices [gm.RFBazookaLevel];
		TRBazookaPrice = gm.TRBazookaPrices [gm.TRBazookaLevel];
		SNBazookaPrice = gm.SniperBazookaPrices [gm.SniperBazookaLevel];
		shieldPrice = gm.shieldPrices [gm.shieldLevel];

		int ScrWid = Screen.width;
		int ScrHgh = Screen.height;
		int SDx = Mathf.RoundToInt (ScrWid / 6);
		int SDy = Mathf.RoundToInt (ScrHgh / 6);

		RectTransform menuBGRT = GameObject.Find ("Image_BG").GetComponent<RectTransform> ();
		menuBGRT.sizeDelta = new Vector2 (ScrWid, ScrHgh);

		RectTransform shopPanel = GameObject.Find ("Shop Panel").GetComponent<RectTransform> ();
		shopPanel.anchoredPosition = new Vector2 (0, ScrHgh * 0.4375f);
		shopPanel.sizeDelta = new Vector2 (ScrWid / 2, ScrHgh * 0.75f);

		GameObject storeGO = GameObject.Find ("Text_Store_Title");
		RectTransform storeTF = storeGO.transform as RectTransform;
		storeTF.anchoredPosition = new Vector2 (0, ScrHgh / 2.5f);

		Text coinTxt = GameObject.Find ("Text_Coins_Info").GetComponent<Text> ();
		coinTxt.text = "" + gm.points;
		RectTransform coinTxtTF = coinTxt.transform as RectTransform;
		RectTransform coinImg = GameObject.Find ("Image_Coin").transform as RectTransform;

		RectTransform bazookaTF = GameObject.Find ("Button_Bazooka_Store").transform as RectTransform;
		bazookaTF.GetComponent<Image> ().sprite = bazookaSprites [gm.bazookaLevel];

		RectTransform RFBazookaTF = GameObject.Find ("Button_RF-Bazooka_Store").transform as RectTransform;
		RFBazookaTF.GetComponent<Image> ().sprite = RFBazookaSprites [gm.RFBazookaLevel];

		RectTransform TRBazookaTF = GameObject.Find ("Button_TR-Bazooka_Store").transform as RectTransform;
		TRBazookaTF.GetComponent<Image> ().sprite = TRBazookaSprites [gm.TRBazookaLevel];

		RectTransform SNBazookaTF = GameObject.Find ("Button_SN-Bazooka_Store").transform as RectTransform;
		SNBazookaTF.GetComponent<Image> ().sprite = SNBazookaSprites [gm.SniperBazookaLevel];

		RectTransform shieldTF = GameObject.Find ("Button_Shield_Store").transform as RectTransform;
		shieldTF.GetComponent<Image> ().sprite = shieldSprites[gm.shieldLevel];

		/* Disabled scripts #8: Currently unused item for survival mode
		RectTransform multiTF = GameObject.Find ("Button_Multiplier_Store").transform as RectTransform;
		multiTF.GetComponent<Image> ().sprite = multiSprites[gm.coinMultiplier - 1];
		*/

		Text bazookaTxt = GameObject.Find ("Text_Bazooka_Store").GetComponent<Text> ();
		bazookaTxt.fontSize = 36;

		Text RFBazookaTxt = GameObject.Find ("Text_RF-Bazooka_Store").GetComponent<Text> ();
		RFBazookaTxt.fontSize = 36;

		Text TRBazookaTxt = GameObject.Find ("Text_TR-Bazooka_Store").GetComponent<Text> ();
		TRBazookaTxt.fontSize = 36;

		Text SNBazookaTxt = GameObject.Find ("Text_SN-Bazooka_Store").GetComponent<Text> ();
		SNBazookaTxt.fontSize = 36;

		Text shieldTxt = GameObject.Find ("Text_Shield_Store").GetComponent<Text> ();
		shieldTxt.fontSize = 36;

		/* Disabled scripts #8: Currently unused item for survival mode
		Text multiTxt = GameObject.Find ("Text_Multiplier_Store").GetComponent<Text> ();
		multiTxt.fontSize = 36;
		*/

		if (gm.bazookaLevel < 3)
			bazookaTxt.text = "BAZOOKA\nUseful for wiping out enemies!\nPrice: " + bazookaPrice + "\nLevel: " + (gm.bazookaLevel + 1);
		else
			bazookaTxt.text = "BAZOOKA\nUseful for wiping out enemies!\nPrice: N/A" + "\nLevel: Maxed Out!";

		if (gm.TRBazookaLevel < 3)
			TRBazookaTxt.text = "TRIPLE BAZOOKA\nTriple treat!\nPrice: " + TRBazookaPrice + "\nLevel: " + (gm.TRBazookaLevel + 1);
		else
			TRBazookaTxt.text = "TRIPLE BAZOOKA\nTriple treat!\nPrice: N/A" + "\nLevel: Maxed Out!";
		
		if (gm.RFBazookaLevel < 3)
			RFBazookaTxt.text = "RAPID-FIRED BAZOOKA\nYou can rapidly pull the trigger!\nPrice: " + RFBazookaPrice + "\nLevel: " + (gm.RFBazookaLevel + 1);
		else
			RFBazookaTxt.text = "RAPID-FIRED BAZOOKA\nYou can rapidly pull the trigger!\nPrice: N/A" + "\nLevel: Maxed Out!";

		if (gm.SniperBazookaLevel < 3)
			SNBazookaTxt.text = "SNIPER BAZOOKA\nThis bad boy can shoot through yer enemies!\nPrice: " + SNBazookaPrice + "\nLevel: " + (gm.SniperBazookaLevel+ 1);
		else
			SNBazookaTxt.text = "SNIPER BAZOOKA\nThis bad boy can shoot through yer enemies!\nPrice: N/A" + "\nLevel: Maxed Out!";

		if (gm.shieldLevel < 3)
			shieldTxt.text = "SHIELD\nProtecting your hero since 2018!\nPrice: " + shieldPrice + "\nLevel: " + (gm.shieldLevel + 1);
		else
			shieldTxt.text = "SHIELD\nProtecting your hero since 2018!\nPrice: N/A" + "\nLevel: Maxed Out!";

		/* Disabled scripts #8: Currently unused item for survival mode
		if (gm.coinMultiplier < 10)
			multiTxt.text = "COIN MULTIPLIER\nKa-ching!\nPrice: " + multiPrice + "\nLevel: " + (gm.coinMultiplier + 1);
		else
			multiTxt.text = "COIN MULTIPLIER\nKa-ching!\nPrice: N/A" + "\nLevel: Maxed Out!";
		*/
		
		RectTransform bazookaTxtTF = bazookaTxt.transform as RectTransform;
		bazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform TRBazookaTxtTF = TRBazookaTxt.transform as RectTransform;
		TRBazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform RFBazookaTxtTF = RFBazookaTxt.transform as RectTransform;
		RFBazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform SNBazookaTxtTF = SNBazookaTxt.transform as RectTransform;
		SNBazookaTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		RectTransform shieldTxtTF = shieldTxt.transform as RectTransform;
		shieldTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);

		/* Disabled scripts #8: Currently unused item for survival mode
		RectTransform multiTxtTF = multiTxt.transform as RectTransform;
		multiTxtTF.sizeDelta = new Vector2 (ScrWid / 4, SDy);
		*/

		Button[] buttons = GameObject.Find("Canvas").GetComponentsInChildren<Button> ();
		for (int i = 0; i < buttons.Length; i++) {
			RectTransform buttTF = buttons [i].transform as RectTransform;

			int leftPadding = Mathf.RoundToInt ((ScrWid / 2) * 0.390625f);

			if (buttTF.name.Contains ("Store")) {
				HorizontalLayoutGroup horizontalLayout = buttTF.GetComponent<HorizontalLayoutGroup> ();
				horizontalLayout.padding.left = leftPadding;
			}

			// auto-size menu UIs
			buttTF.sizeDelta = new Vector2 (SDx, SDy);

			// auto-position menu UIs
			if (buttTF.name.Contains ("_Start")) {
				buttTF.anchoredPosition = new Vector2 ((buttTF.anchoredPosition.x - (SDx + 20) * i), buttTF.anchoredPosition.y);
				buttTF.anchorMax = new Vector2 (1, 0);
				buttTF.anchorMin = new Vector2 (1, 0);
				buttTF.pivot = new Vector2 (1, 0);
			}
			else if (buttTF.name.Contains ("_Bazooka")) {
				buttTF.anchoredPosition = new Vector2 (buttTF.anchoredPosition.x - (bazookaTxtTF.sizeDelta.x / 2), (storeTF.anchoredPosition.y - ((SDy + 20) * (i-1))));
				/* Disabled scripts #9: Old script dictates once the item level is over certain amount, said button is rendered non-interactable
				if (gm.bazookaLevel < 3)
					buttTF.GetComponent<Button> ().interactable = true;
				else
					buttTF.GetComponent<Button> ().interactable = false;
				*/
			}
			else if (buttTF.name.Contains ("TR-Bazooka")) {
				buttTF.anchoredPosition = new Vector2 (buttTF.anchoredPosition.x - (TRBazookaTxtTF.sizeDelta.x / 2), (storeTF.anchoredPosition.y - ((SDy + 20) * (i-1))));
				/* Disabled scripts #9: Old script dictates once the item level is over certain amount, said button is rendered non-interactable
				if (gm.TRBazookaLevel < 3)
					buttTF.GetComponent<Button> ().interactable = true;
				else
					buttTF.GetComponent<Button> ().interactable = false;
				*/
			}
			else if (buttTF.name.Contains ("RF-Bazooka")) {
				buttTF.anchoredPosition = new Vector2 (buttTF.anchoredPosition.x - (RFBazookaTxtTF.sizeDelta.x / 2), (storeTF.anchoredPosition.y - ((SDy + 20) * (i-1))));
				/* Disabled scripts #9: Old script dictates once the item level is over certain amount, said button is rendered non-interactable
				if (gm.RFBazookaLevel < 3)
					buttTF.GetComponent<Button> ().interactable = true;
				else
					buttTF.GetComponent<Button> ().interactable = false;
				*/
			}
			else if (buttTF.name.Contains ("SN-Bazooka")) {
				buttTF.anchoredPosition = new Vector2 (buttTF.anchoredPosition.x - (SNBazookaTxtTF.sizeDelta.x / 2), (storeTF.anchoredPosition.y - ((SDy + 20) * (i-1))));
				/* Disabled scripts #9: Old script dictates once the item level is over certain amount, said button is rendered non-interactable
				if (gm.RFBazookaLevel < 3)
					buttTF.GetComponent<Button> ().interactable = true;
				else
					buttTF.GetComponent<Button> ().interactable = false;
				*/
			}
			else if (buttTF.name.Contains ("Shield")) {
				buttTF.anchoredPosition = new Vector2 (buttTF.anchoredPosition.x - (shieldTxtTF.sizeDelta.x / 2), (storeTF.anchoredPosition.y - ((SDy + 20) * (i-1))));
				if (gm.shieldLevel < 3)
					buttTF.GetComponent<Button> ().interactable = true;
				else
					buttTF.GetComponent<Button> ().interactable = false;
			}

			/* Disabled scripts #8: Currently unused item for survival mode
			else if (buttTF.name.Contains ("Multiplier")) {
				buttTF.anchoredPosition = new Vector2 (buttTF.anchoredPosition.x - (multiTxtTF.sizeDelta.x / 2), (storeTF.anchoredPosition.y - ((SDy + 20) * (i-1))));
				if (gm.coinMultiplier < 10)
					buttTF.GetComponent<Button> ().interactable = true;
				else
					buttTF.GetComponent<Button> ().interactable = false;
			}
			*/

			Text[] buttTxts = buttTF.GetComponentsInChildren<Text> ();
			foreach (Text txt in buttTxts) {
				txt.fontSize = Mathf.RoundToInt (buttTF.sizeDelta.x / 5f);
			}
		}

		Text[] texts = GameObject.Find ("Canvas").GetComponentsInChildren<Text> ();
		for (int i = 0; i < texts.Length; i++) {
			RectTransform textTF = texts [i].transform as RectTransform;
			if (textTF.name.Contains ("_Bazooka"))
				textTF.anchoredPosition = new Vector2 (textTF.anchoredPosition.x + (bazookaTF.sizeDelta.x * 0.6f), (storeTF.anchoredPosition.y - ((SDy + 20) * i)));
			else if (textTF.name.Contains ("TR-Bazooka"))
				textTF.anchoredPosition = new Vector2 (textTF.anchoredPosition.x + (TRBazookaTF.sizeDelta.x * 0.6f), (storeTF.anchoredPosition.y - ((SDy + 20) * i)));
			else if (textTF.name.Contains ("RF-Bazooka"))
				textTF.anchoredPosition = new Vector2 (textTF.anchoredPosition.x + (RFBazookaTF.sizeDelta.x * 0.6f), (storeTF.anchoredPosition.y - ((SDy + 20) * i)));
			else if (textTF.name.Contains ("SN-Bazooka"))
				textTF.anchoredPosition = new Vector2 (textTF.anchoredPosition.x + (SNBazookaTF.sizeDelta.x * 0.6f), (storeTF.anchoredPosition.y - ((SDy + 20) * i)));
			else if (textTF.name.Contains ("Shield"))
				textTF.anchoredPosition = new Vector2 (textTF.anchoredPosition.x + (shieldTF.sizeDelta.x * 0.6f), (storeTF.anchoredPosition.y - ((SDy + 20) * i)));
			/* Disabled scripts #8: Currently unused item for survival mode
			else if (textTF.name.Contains ("Multiplier"))
				textTF.anchoredPosition = new Vector2 (textTF.anchoredPosition.x + (multiTF.sizeDelta.x * 0.6f), (storeTF.anchoredPosition.y - ((SDy + 20) * i)));
			*/
		}
	}

	public void OnStart () {
		SceneManager.LoadScene ("Survival");
		gm.wave = 0;
	}

	public void EquipNotifyOpen ()
	{
		notifyText.text = gm.equippedBazookaInGunScript + " equip to slot:";
		equipNotify.gameObject.SetActive (true);
	}

	public void EquipNotifyClose ()
	{
		equipNotify.gameObject.SetActive (false);
	}

	public void EquipToSlot (int slot)
	{
		if (slot == 1) {
			if (gm.slot2 != gm.equippedBazookaInGunScript) {
				gm.slot1 = gm.equippedBazookaInGunScript;
			} 
			else
				Debug.Log ("Bazooka already assigned to Slot 2!");
		}
		else if (slot == 2) {
			if (gm.slot1 != gm.equippedBazookaInGunScript) {
				gm.slot2 = gm.equippedBazookaInGunScript;
			} 
			else
				Debug.Log ("Bazooka already assigned to Slot 1!");
		}

		stateManager.slot1 = gm.slot1;
		stateManager.slot2 = gm.slot2;

		equippedWeaponsText.text = " Slot 1: " + gm.slot1 + "\n Slot 2: " + gm.slot2;

		EquipNotifyClose ();
	}

	public void BuyBazooka () {
		gm.BuyBazooka ();
		ReinitUI ();
	}

	public void BuyTRBazooka () {
		gm.BuyTRBazooka ();
		ReinitUI ();
	}

	public void BuyRFBazooka () {
		gm.BuyRFBazooka ();
		ReinitUI ();
	}

	public void BuySNBazooka () {
		gm.BuySniperBazooka ();
		ReinitUI ();
	}

	public void BuyMulti () {
		gm.BuyMultiplier ();
		ReinitUI ();
	}

	public void BuyShield () {
		gm.BuyShield ();		
		ReinitUI ();
	}

	public void ResetState ()
	{
		stateManager.GoReset ();
		ReinitUI ();

		equippedWeaponsText.text = " Slot 1: " + gm.slot1 + "\n Slot 2: " + gm.slot2;
	}
}