using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unscrewer : MonoBehaviour {

	public float speed = 5.0f;

	private Vector3 targetOffset;

	private bool resting;

	private List<screwBase> screwList;
	private List<screwBase> unscrewList;

	private Vector3 target;

	private const float screwTime = 0.25f;	// time in seconds

	private bool invokeDelay = true;

	private AudioSource screwdriverAudio;

	void Start() {
		resting = false;
		screwList = new List<screwBase> ();
		unscrewList = new List<screwBase> ();
		screwdriverAudio = GetComponent<AudioSource>();
	}

	public void addScrew(screwBase a){
		Debug.Log ("Adding Screw");
		screwList.Add (a);

	}

	public void addUnscrew(screwBase a){
		Debug.Log ("Adding Unscrew");
		unscrewList.Add (a);

	}

	private void popScrew(){
		Debug.Log ("Popping Screw");
		if(screwList.Count > 0)
			screwList.RemoveAt (0);
		invokeDelay = true;
	}

	private void popUnscrew(){
		Debug.Log ("Popping Unscrew");
		if(unscrewList.Count > 0)
			unscrewList.RemoveAt (0);
		invokeDelay = true;
	}

	void moveToTarget (){
		//goto target y.
		Vector3 newPos = transform.position;
		newPos += (target - newPos).normalized * speed * Time.deltaTime * 
		Mathf.Clamp((target - newPos).magnitude, 0.0f,1.0f);
		newPos.z = -3f;
		//if close to target y, goto target x.
		transform.position = newPos;
	}



	// Update is called once per frame
	void FixedUpdate () {
		if (resting) {
			target = new Vector3 (-15, 0, 0);
			if (unscrewList.Count != 0 || screwList.Count != 0)
				resting = false;
		} else {
			if (unscrewList.Count == 0 && screwList.Count == 0) {
				resting = true;
			} else if (unscrewList.Count != 0){
				//can we even do anything?
				if (!unscrewList [0].isModifiable ()) {
					popUnscrew ();
				} else {
					target = unscrewList [0].transform.position;
					target.z = transform.position.z; //clamp z
					if (Vector3.Distance (target, transform.position) < 0.1f) {
						unscrewList [0].unscrew ();
						if (invokeDelay) {
							screwdriverAudio.Play();
							Invoke ("popUnscrew", 0.25f);
							invokeDelay = false;
						}
					}
				}
			} else if (screwList.Count != 0){
				//can we even do anything?
				if (!screwList [0].isModifiable ()) {
					popScrew ();
				} else {
					target = screwList [0].transform.position;
					target.z = transform.position.z; //clamp z
					if (Vector3.Distance (target, transform.position) < 0.1f) {
						screwList [0].screw ();
						if (invokeDelay) {
							screwdriverAudio.Play();
							Invoke ("popScrew", 0.25f);
							invokeDelay = false;
						}
					}
				}
			}
		}
		moveToTarget ();
	}
}
