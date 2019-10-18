using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public GameObject slot1GO, slot2GO;

	public RectTransform equipNotify;

	public Text notifyText;

	Image slot1Image, slot2Image;

	Button slot1Button, slot2Button;

	Gun gun;
	GM gm;

	[System.Serializable]
	public class ButtonSprites
	{
		public Sprite basicBazooka, tripleBazooka, rapidBazooka, sniperBazooka, specialBazooka, empty1, empty2;
	}
	public ButtonSprites buttonSprites;

	void Awake ()
	{
		gun = GameObject.Find ("Gun").GetComponent<Gun> ();
		gm = GameObject.Find ("GM").GetComponent<GM> ();

		slot1Button = slot1GO.GetComponent<Button> ();
		slot2Button = slot2GO.GetComponent<Button> ();

		slot1Image = slot1GO.GetComponent<Image> ();
		slot2Image = slot2GO.GetComponent<Image> ();

		InitEquipNotify ();
	}

	void InitEquipNotify ()
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
		InitSlot ();
	}

	void Update ()
	{
		if (gun == null && !gm.dead)
			gun = GameObject.Find ("Gun").GetComponent<Gun> ();
	}

	public void InitSlot ()
	{
		if (gm.slot1 == null) {
			slot1Image.sprite = buttonSprites.basicBazooka;
		}
		else if (gm.slot1 == "Basic")
			slot1Image.sprite = buttonSprites.basicBazooka;
		else if (gm.slot1 == "Triple")
			slot1Image.sprite = buttonSprites.tripleBazooka;
		else if (gm.slot1 == "Rapid-fire")
			slot1Image.sprite = buttonSprites.rapidBazooka;
		else if (gm.slot1 == "Sniper")
			slot1Image.sprite = buttonSprites.sniperBazooka;
		else if (gm.slot1 == "Special")
			slot1Image.sprite = buttonSprites.specialBazooka;
		

		if (gm.slot2 == null) {
			slot2Image.sprite = buttonSprites.empty2;
		}
		else if (gm.slot2 == "Basic")
			slot2Image.sprite = buttonSprites.basicBazooka;
		else if (gm.slot2 == "Triple")
			slot2Image.sprite = buttonSprites.tripleBazooka;
		else if (gm.slot2 == "Rapid-fire")
			slot2Image.sprite = buttonSprites.rapidBazooka;
		else if (gm.slot2 == "Sniper")
			slot2Image.sprite = buttonSprites.sniperBazooka;
		else if (gm.slot2 == "Special")
			slot2Image.sprite = buttonSprites.specialBazooka;
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
				InitSlot ();
			}
			else
				Debug.Log ("Bazooka already assigned to Slot 2!");
		} 
		else if (slot == 2) {
			if (gm.slot1 != gm.equippedBazookaInGunScript) {
				gm.slot2 = gm.equippedBazookaInGunScript;
				InitSlot ();
			}
			else
				Debug.Log ("Bazooka already assigned to Slot 1!");
		}

		EquipNotifyClose ();
	}

	public void EquipFromSlot (int slot)
	{
		if (slot == 1 && gm.slot1 != null)
			gun.ChangeWeapon (gm.slot1);
		else if (slot == 2 && gm.slot2 != null)
			gun.ChangeWeapon (gm.slot2);
	}
}
