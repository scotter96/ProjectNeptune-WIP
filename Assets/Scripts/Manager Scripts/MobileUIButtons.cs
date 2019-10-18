using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileUIButtons : MonoBehaviour 
{
	Gun gun;
	PlayerControl PC;

	public void Start ()
	{
		gun = GameObject.FindWithTag ("Player").GetComponentInChildren<Gun> ();
		PC = GameObject.FindWithTag ("Player").GetComponent<PlayerControl> ();
	}

	public void OnShoot ()
	{
		Debug.Log (gun.transform.parent.name);
		Debug.Log (gun.name);
		gun.update = true;
	}

	public void OnJump ()
	{
		if (PC.grounded)
			PC.jump = true;
	}

	public void OnLeft ()
	{
		PC.h--;
		if (PC.h <= -1)
			PC.h = -1;
	}

	public void OnRight ()
	{
		PC.h++;
		if (PC.h >= 1)
			PC.h = 1;
	}

	public void OnRelease ()
	{
		PC.h = 0;
	}

	void Update ()
	{
		if (!gun.UIShoot && gun.update == true)
			gun.UIShoot = true;
		else if (gun.UIShoot)
			gun.UIShoot = false;
		
		if (PC.grounded)
			GameObject.Find ("Button_Jump").GetComponent<Button>().interactable = true;
		else if (PC.grounded == false)
			GameObject.Find ("Button_Jump").GetComponent<Button>().interactable = false;
	}
}
