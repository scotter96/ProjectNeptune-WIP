using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pauser : MonoBehaviour 
{
	bool paused = false;

	GameObject pausePanel;

	int exitDelay;

	StoreApproach storeApproach;
	GM gm;

	void Awake ()
	{
		Time.timeScale = 1;

		if (SceneManager.GetActiveScene ().name == "Survival") {
			gm = GameObject.Find ("GM").GetComponent<GM> ();
			pausePanel = GameObject.Find ("PausePanel");
			storeApproach = GameObject.Find ("ShopTower").GetComponent<StoreApproach> ();

			RectTransform pausePanelRT = pausePanel.GetComponent<RectTransform> ();
			pausePanelRT.sizeDelta = new Vector2 (Screen.width / 2.5f, Screen.height / 4);

			RectTransform textRT = pausePanel.transform.GetChild (0).GetComponent<RectTransform> ();
			textRT.sizeDelta = new Vector2 (0, 0);
			textRT.anchoredPosition = new Vector2 (0, -16);

			RectTransform[] RTs = pausePanelRT.GetComponentsInChildren<RectTransform> ();
			foreach (RectTransform rt in RTs) {
				if (rt.name.Contains ("Yes")) {
					rt.sizeDelta = new Vector2 (0, 0);
					rt.anchoredPosition = new Vector2 (-96, -48);
				} else if (rt.name.Contains ("No")) {
					rt.sizeDelta = new Vector2 (0, 0);
					rt.anchoredPosition = new Vector2 (96, -48);
				}
			}

			pausePanel.SetActive (false);
			exitDelay = 4;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			if (SceneManager.GetActiveScene ().name != "Menu") {
				paused = !paused;
				SetPanel ();
			}
			else
				Application.Quit ();
		}
	}

	public void OnBackYes ()
	{
		exitDelay = 0;
		StartCoroutine ("BackToMenu");
	}

	public void OnBackNo ()
	{
		paused = !paused;
		SetPanel ();
	}

	void SetPanel ()
	{
		if (paused) {
			Time.timeScale = 0;
			pausePanel.SetActive (true);

			if (storeApproach.isApproached)
				storeApproach.SetShopMenu (false);
		}
		else {
			Time.timeScale = 1;
			pausePanel.SetActive (false);

			if (storeApproach.isApproached)
				storeApproach.SetShopMenu (true);
		}
	}

	IEnumerator BackToMenu ()
	{
		yield return new WaitForSeconds (exitDelay);

		if (gm.highScore < gm.wave)
			gm.highScore = gm.wave;
		
		SceneManager.LoadScene ("Menu");
	}
}
