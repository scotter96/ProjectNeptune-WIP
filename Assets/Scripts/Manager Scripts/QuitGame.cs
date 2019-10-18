using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour 
{
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape))
			Application.Quit ();
		else if (Input.GetKeyUp (KeyCode.Return))
			SceneManager.LoadScene ("Level1");
	}
}
