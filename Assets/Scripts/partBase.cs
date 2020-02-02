using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partBase : MonoBehaviour {
	
	public Vector3 snapPoint;

	public List<screwBase> screws;

	public List<GameObject> obscureObjects;

	public bool removed;
	public bool removeable;
	public bool obscured;

	void Start()
	{
		snapPoint = transform.position;
		GetComponentsInChildren<screwBase> (screws);
		removeable = testRemoveable ();
		obscured = testObscured ();
	}

	public bool testScrewable()
	{
		//if we are removed, cannot screw.
		if (removed)
			return false;
		//if we are obscured, cannot screw.
		return !testObscured ();
	}

	public bool testRemoveable()
	{
		//check to see if all screws have been screwed
		foreach (screwBase a in screws)
			if (a.screwed)
				return false;	//return false if any are screwed.
		return !testObscured();
	}

	public bool testObscured()
	{
		//checks to make sure that all the obscure objects have been removed.
		foreach (GameObject a in obscureObjects)
			if(!a.GetComponent<partBase>().removed)
				return true;
		return false;
	}
}