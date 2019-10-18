using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour 
{
	GM GC;

	public Sprite[] bazookaSprites, TRBazookaSprites, RFBazookaSprites, SNBazookaSprites;
	public Sprite[] multiSprites, shieldSprites;

	void Awake (){
		GC = GameObject.Find ("GM").GetComponent<GM> ();
	}

	void Update ()
	{		
		ConditionsCheck ();
	}

	public void OnBazookaPressed ()
	{
		GC.BuyBazooka ();

		ConditionsCheck ();
	}

	public void OnRFBazookaPressed ()
	{
		GC.BuyRFBazooka ();

		ConditionsCheck ();
	}

	public void OnTRBazookaPressed ()
	{
		GC.BuyTRBazooka ();

		ConditionsCheck ();
	}

	public void OnSNBazookaPressed ()
	{
		GC.BuySniperBazooka ();

		ConditionsCheck ();
	}
	public void OnShieldPressed ()
	{
		GC.BuyShield ();

		ConditionsCheck ();
	}

	/* Disabled scripts #5: Unused story items on store 
	public void On1UPPressed ()
	{
		if (GC.points >= 500 && GC.lives <= 8) {
			GC.On1UPBought ();
			GC.points -= 500;
			GC.UpdatePoint ();
		}

		ConditionsCheck ();
	}

	public void OnCoinMultiplier ()
	{
		if (GC.points >= 3000 && GC.coinMultiplier > 1) {
			GC.BuyMultiplier();
			GC.points -= 3000;
			GC.UpdatePoint ();
		}

		ConditionsCheck ();
	}
	*/

	void ConditionsCheck ()
	{
		RectTransform[] shopItems = GetComponentsInChildren<RectTransform>();

		foreach (RectTransform rt in shopItems) {			
			if (rt.name.StartsWith ("Bazooka")) {
				rt.gameObject.GetComponent<Image> ().sprite = bazookaSprites [GC.bazookaLevel];

				if (GC.bazookaLevel < 3) {
					if (GC.points < GC.bazookaPrices [GC.bazookaLevel])
						rt.gameObject.GetComponent<Button> ().interactable = false;
					else
						rt.gameObject.GetComponent<Button> ().interactable = true;
				}
			} 
			else if (rt.name.StartsWith ("Triple")) {
				rt.gameObject.GetComponent<Image> ().sprite = TRBazookaSprites [GC.TRBazookaLevel];

				if (GC.TRBazookaLevel < 3) {
					if (GC.points < GC.TRBazookaPrices [GC.TRBazookaLevel])
						rt.gameObject.GetComponent<Button> ().interactable = false;
					else
						rt.gameObject.GetComponent<Button> ().interactable = true;
				}
			} 
			else if (rt.name.StartsWith ("Rapid-Fire")) {
				rt.gameObject.GetComponent<Image> ().sprite = RFBazookaSprites [GC.RFBazookaLevel];

				if (GC.RFBazookaLevel < 3) {
					if (GC.points < GC.RFBazookaPrices [GC.RFBazookaLevel])
						rt.gameObject.GetComponent<Button> ().interactable = false;
					else
						rt.gameObject.GetComponent<Button> ().interactable = true;
				}
			} 
			else if (rt.name.StartsWith ("Sniper")) {
				rt.gameObject.GetComponent<Image> ().sprite = SNBazookaSprites [GC.SniperBazookaLevel];

				if (GC.SniperBazookaLevel < 3) {
					if (GC.points < GC.SniperBazookaPrices [GC.SniperBazookaLevel])
						rt.gameObject.GetComponent<Button> ().interactable = false;
					else
						rt.gameObject.GetComponent<Button> ().interactable = true;
				}
			}
			else if (rt.name.StartsWith ("Shield")) {
				rt.gameObject.GetComponent<Image> ().sprite = shieldSprites [GC.shieldLevel];

				if (GC.shieldLevel < 3) {
					if (GC.points < GC.shieldPrices [GC.shieldLevel])
						rt.gameObject.GetComponent<Button> ().interactable = false;
					else
						rt.gameObject.GetComponent<Button> ().interactable = true;
				} else
					rt.gameObject.GetComponent<Button> ().interactable = false;
			} 			
			else if (rt.name.StartsWith ("Price")) {
				Text thisText = rt.gameObject.GetComponent<Text> ();

				if (rt.parent.name.StartsWith ("Bazooka"))
				{
					thisText.text = "" + GC.bazookaPrices [GC.bazookaLevel];
					if (GC.bazookaLevel >= 3)
						thisText.text = "N/A";
				}
				else if (rt.parent.name.StartsWith ("Triple"))
				{
					thisText.text = "" + GC.TRBazookaPrices [GC.TRBazookaLevel];
					if (GC.TRBazookaLevel >= 3)
						thisText.text = "N/A";
				}
				else if (rt.parent.name.StartsWith ("Rapid-Fire"))
				{
					thisText.text = "" + GC.RFBazookaPrices [GC.RFBazookaLevel];
					if (GC.RFBazookaLevel >= 3)
						thisText.text = "N/A";
				}
				else if (rt.parent.name.StartsWith ("Sniper"))
				{
					thisText.text = "" + GC.SniperBazookaPrices [GC.SniperBazookaLevel];
					if (GC.SniperBazookaLevel >= 3)
						thisText.text = "N/A";
				}
				else if (rt.parent.name.StartsWith ("Shield"))
				{
					thisText.text = "" + GC.shieldPrices [GC.shieldLevel];
					if (GC.shieldLevel >= 3)
						thisText.text = "N/A";
				}
			}

			/* Disabled scripts #7: Currently unused items (also unfinished script below)
			else if (go.name == "CoinMButton") {
				go.GetComponent<Image> ().sprite = multiSprites[GC.coinMultiplier - 1];
			}
			
			else if (go.name == "1UPButton") {
				go.GetComponent<Image> ().sprite = sprites.extraLifeMaxed;
			}
			*/
		}
	}
}
