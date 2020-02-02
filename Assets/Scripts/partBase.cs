using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partBase : MonoBehaviour {
	
	public Vector3 snapPoint;

	private Vector3 snapPointOriginal;

	public List<screwBase> screws;

	public List<GameObject> obscureObjects;

	public bool removed;
	public bool removeable;
	public bool obscured;

	public bool rusted = true;

	//parents the class
	private Transform robotFrame;

	void Start()
	{
		
		
		GetComponentsInChildren<screwBase> (screws);
		removeable = testRemoveable ();
		obscured = testObscured ();
		robotFrame = GameObject.FindWithTag("Player").transform;
		Debug.Log(robotFrame.gameObject.name);
		snapPointOriginal = transform.position - robotFrame.position;
		snapPoint = snapPointOriginal + robotFrame.position;
	}

	void Update()
	{
		snapPoint = snapPointOriginal + robotFrame.position;
		if(!removed){
			transform.position = snapPoint;
		}
		if(rusted)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.8113208f, 0.4368972f, 0.3023318f);
		} else {
			GetComponent<SpriteRenderer>().color = Color.white;
		}
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