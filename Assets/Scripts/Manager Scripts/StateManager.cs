using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour {

	Text notifier;
	GM gm;

	public int points;
	public int bazookaLevel;
	public int TRBazookaLevel;
	public int RFBazookaLevel;
	public int SNBazookaLevel;
	public int shieldLevel;
	public int coinMultiplier;
	public int highScore;

	public string equippedWeapon;
	public string slot1, slot2;

	string lastEquippedWeapon;
	string lastSlot1, lastSlot2;

	int lastPoints;
	int lastBazookaLevel;
	int lastTRBazookaLevel;
	int lastRFBazookaLevel;
	int lastSNBazookaLevel;
	int lastShieldLevel;
	int lastCoinMultiplier;
	int lastHighScore;

	void Awake () 
	{
		gm = GetComponent<GM> ();
		notifier = GameObject.Find ("Notifier").GetComponent<Text> ();

		#if UNITY_EDITOR
		Debug.Log ("App data path: " + Application.persistentDataPath);
		#endif

		if (SceneManager.GetActiveScene().name == "Menu")
			notifier.text = "";
		
		GoLoad ();
	}

	void Update ()
	{
		if (SceneManager.GetActiveScene ().name == "Menu") {
			if (notifier == null)
				notifier = GameObject.Find ("Notifier").GetComponent<Text> ();
		}

		if (lastPoints != points || lastBazookaLevel != bazookaLevel || lastCoinMultiplier != coinMultiplier
			|| lastTRBazookaLevel != TRBazookaLevel || lastRFBazookaLevel != RFBazookaLevel 
			|| lastSNBazookaLevel != SNBazookaLevel || lastShieldLevel != shieldLevel
			|| lastEquippedWeapon != equippedWeapon || lastSlot1 != slot1 || lastSlot2 != slot2 
			|| lastHighScore != highScore)
		{
			GoSave ();
			notifier.text = "Game saved !";
			Invoke ("StopNotifier", 1);

			if (points < 0 || lastPoints < 0) {
				points = 0;
				lastPoints = 0;
			}
			if (bazookaLevel < 0 || lastBazookaLevel < 0) {
				if (gm.isSurvival && !gm.isStory)
					bazookaLevel = 1;
				if (gm.isStory && !gm.isSurvival)
					bazookaLevel = 0;
				
				lastBazookaLevel = 0;
			}
			if (coinMultiplier < 1 || lastCoinMultiplier < 1) {
				coinMultiplier = 1;
				lastCoinMultiplier = 1;
			}
			if (TRBazookaLevel < 0 || lastTRBazookaLevel < 0) {
				TRBazookaLevel = 0;
				lastTRBazookaLevel = 0;
			}
			if (RFBazookaLevel < 0 || lastRFBazookaLevel < 0) {
				RFBazookaLevel = 0;
				lastRFBazookaLevel = 0;
			}
			if (SNBazookaLevel < 0 || lastSNBazookaLevel < 0) {
				SNBazookaLevel = 0;
				lastSNBazookaLevel = 0;
			}
			if (shieldLevel < 0 || lastShieldLevel < 0) {
				shieldLevel = 0;
				lastShieldLevel = 0;
			}
			if (equippedWeapon == null || lastEquippedWeapon == null) {
				equippedWeapon = "Basic";
				lastEquippedWeapon = "Basic";
			}
			if (slot1 == null) {
				slot1 = "Basic";
				lastSlot1 = "Basic";
			}
			if (highScore < 0 || lastHighScore < 0) {
				highScore = 0;
				lastHighScore = 0;
			}
		}
	}

	void GoSave () 
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/saveData.nep");
		SaveData data = new SaveData ();

		data.points = points;
		data.bazookaLevel = bazookaLevel;
		data.coinMultiplier = coinMultiplier;
		data.TRBazookaLevel = TRBazookaLevel;
		data.RFBazookaLevel = RFBazookaLevel;
		data.SNBazookaLevel = SNBazookaLevel;
		data.shieldLevel = shieldLevel;
		data.equippedWeapon = equippedWeapon;
		data.slot1 = slot1;
		data.slot2 = slot2;
		data.highScore = highScore;

		lastPoints = data.points;
		lastBazookaLevel = data.bazookaLevel;
		lastCoinMultiplier = data.coinMultiplier;
		lastTRBazookaLevel = data.TRBazookaLevel;
		lastRFBazookaLevel = data.RFBazookaLevel;
		lastSNBazookaLevel = data.SNBazookaLevel;
		lastShieldLevel = data.shieldLevel;
		lastEquippedWeapon = data.equippedWeapon;
		lastSlot1 = data.slot1;
		lastSlot2 = data.slot2;
		lastHighScore = data.highScore;

		bf.Serialize (file, data);
		file.Close ();
	}

	void GoLoad ()
	{
		if (File.Exists (Application.persistentDataPath + "/saveData.nep")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/saveData.nep", FileMode.Open);
			SaveData data = (SaveData) bf.Deserialize (file);
			file.Close ();

			points = data.points;
			bazookaLevel = data.bazookaLevel;
			coinMultiplier = data.coinMultiplier;
			TRBazookaLevel = data.TRBazookaLevel;
			RFBazookaLevel = data.RFBazookaLevel;
			SNBazookaLevel = data.SNBazookaLevel;
			shieldLevel = data.shieldLevel;
			equippedWeapon = data.equippedWeapon;
			slot1 = data.slot1;
			slot2 = data.slot2;
			highScore = data.highScore;

			lastPoints = data.points;
			lastBazookaLevel = data.bazookaLevel;
			lastCoinMultiplier = data.coinMultiplier;
			lastTRBazookaLevel = data.TRBazookaLevel;
			lastRFBazookaLevel = data.RFBazookaLevel;
			lastSNBazookaLevel = data.SNBazookaLevel;
			lastShieldLevel = data.shieldLevel;
			lastEquippedWeapon = data.equippedWeapon;
			lastSlot1 = data.slot1;
			lastSlot2 = data.slot2;
			lastHighScore = data.highScore;

			gm.points = points;
			gm.bazookaLevel = bazookaLevel;
			gm.coinMultiplier = coinMultiplier;
			gm.TRBazookaLevel = TRBazookaLevel;
			gm.RFBazookaLevel = RFBazookaLevel;
			gm.SniperBazookaLevel = SNBazookaLevel;
			gm.shieldLevel = shieldLevel;
			gm.equippedBazookaInGunScript = equippedWeapon;
			gm.slot1 = slot1;
			gm.slot2 = slot2;
			gm.highScore = highScore;

			if (SceneManager.GetActiveScene().name == "Menu") {
				notifier.text = "Save found!";			
				Invoke ("StopNotifier", 3);
			}
		} 
		else {
			points = 0;

			if (gm.isSurvival && !gm.isStory)
				bazookaLevel = 1;
			if (gm.isStory && !gm.isSurvival)
				bazookaLevel = 0;
			
			coinMultiplier = 1;
			TRBazookaLevel = 0;
			RFBazookaLevel = 0;
			SNBazookaLevel = 0;
			shieldLevel = 0;
			equippedWeapon = "Basic";
			slot1 = "Basic";
			slot2 = null;
			highScore = 0;

			if (SceneManager.GetActiveScene().name == "Menu") {
				notifier.text = "Save NOT found!";			
				Invoke ("StopNotifier", 3);
			}
		}
	}

	public void GoReset ()
	{
		points = 0;

		if (gm.isSurvival && !gm.isStory)
			bazookaLevel = 1;
		if (gm.isStory && !gm.isSurvival)
			bazookaLevel = 0;
		
		coinMultiplier = 1;
		TRBazookaLevel = 0;
     	RFBazookaLevel = 0;
		SNBazookaLevel = 0;
		shieldLevel = 0;
		equippedWeapon = "Basic";
		slot1 = "Basic";
		slot2 = null;
		highScore = 0;

		lastPoints = 0;

		if (gm.isSurvival && !gm.isStory)
			lastBazookaLevel = 1;
		if (gm.isStory && !gm.isSurvival)
			lastBazookaLevel = 0;
		
		lastCoinMultiplier = 1;
		lastTRBazookaLevel = 0;
		lastRFBazookaLevel = 0;
		lastSNBazookaLevel = 0;
		lastShieldLevel = 0;
		lastEquippedWeapon = "Basic";
		lastSlot1 = "Basic";
		lastSlot2 = null;
		lastHighScore = 0;

		gm.points = points;
		gm.bazookaLevel = bazookaLevel;
		gm.coinMultiplier = coinMultiplier;
		gm.TRBazookaLevel = TRBazookaLevel;
		gm.RFBazookaLevel = RFBazookaLevel;
		gm.SniperBazookaLevel = SNBazookaLevel;
		gm.shieldLevel = shieldLevel;
		gm.equippedBazookaInGunScript = equippedWeapon;
		gm.slot1 = slot1;
		gm.slot2 = slot2;
		gm.highScore = highScore;

		File.Delete (Application.persistentDataPath + "/saveData.nep");
		if (SceneManager.GetActiveScene().name == "Menu") {
			notifier.text = "Save resetted!";
			Invoke ("StopNotifier", 3);
		}
	}

	void StopNotifier ()
	{
		if (SceneManager.GetActiveScene().name == "Menu")
			notifier.text = "";
	}
}

[Serializable]
class SaveData
{
	public int points;
	public int bazookaLevel;
	public int TRBazookaLevel;
	public int RFBazookaLevel;
	public int SNBazookaLevel;
	public int shieldLevel;
	public int coinMultiplier;
	public int highScore;

	public string equippedWeapon;
	public string slot1, slot2;
}
