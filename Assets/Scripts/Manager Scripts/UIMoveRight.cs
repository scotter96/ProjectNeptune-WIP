using UnityEngine;
using System.Collections;

public class UIMoveRight : MonoBehaviour 
{
	PlayerControl PC;

	void Start ()
	{
		PC = GameObject.FindWithTag ("Player").GetComponent<PlayerControl> ();
	}

	public void OnMouse ()
	{
		PC.jump = true;
	}
}
