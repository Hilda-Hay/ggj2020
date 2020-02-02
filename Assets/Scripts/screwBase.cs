using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screwBase : MonoBehaviour {

	public bool screwed = true;

	void Start()
	{
		//make sure is just above parent
		Vector3  pos = transform.parent.position;
		pos.x = transform.position.x;
		pos.y = transform.position.y;
		pos.z = pos.z - 0.5f;
		//Debug.LogFormat ("A {0} < B{1} | {2}", pos, transform.parent.position, transform.parent.gameObject.name);
		transform.position = pos;
	}

	public bool isModifiable()
	{
		return GetComponentInParent<partBase> ().testScrewable ();
	}

	public void unscrew()
	{
		//make invisible
		GetComponent<SpriteRenderer>().enabled = false;
		screwed = false;
	}

	public void screw()
	{
		//make invisible
		GetComponent<SpriteRenderer>().enabled = true;
		screwed = true;
	}
}
