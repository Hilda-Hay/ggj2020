using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clawScript : MonoBehaviour {

	public Transform chain;
	public Transform hook;

	public float speed = 5.0f;

	public Unscrewer us;

	private Vector3 target = new Vector3(0,7,0);
	private Vector3 chain0Pos;
	private Vector3 hookPos;

	private List<partBase> storage;
	private List<partBase> toStorage;
	private List<partBase> outOfStorage;

	private GameObject hookedObj;

	public bool resting = true;

	public void sendToStorage(partBase a){
		toStorage.Add (a);
		foreach (screwBase b in a.screws)
			us.addUnscrew (b);
	}

	public bool attachFromStorage(partBase a){
		//simple check to see if it is in storage
		if (storage.Contains (a)) {
			outOfStorage.Add (a);
			return true;
		} else {
			return false;
		}
	}

	//send the claw to go to the position.
	private void travelTo(){
		Vector3 newPos = transform.position;
		if (transform.position.x + speed * Time.deltaTime < target.x) {
			newPos.x += speed * Time.deltaTime;
		} else if (transform.position.x - speed * Time.deltaTime > target.x) {
			newPos.x -= speed * Time.deltaTime;
		}
		transform.position = newPos;
	}

	private void updateChain (){
		Vector3 newPos = chain.position;
		Vector3 offsetPos = hookPos;
		if (offsetPos.y + speed * Time.deltaTime < target.y) {
			newPos.y += speed * Time.deltaTime;
		} else if (offsetPos.y - speed * Time.deltaTime > target.y) {
			newPos.y -= speed * Time.deltaTime;
		}
		//if close to target y, goto target x.
		chain.position = newPos;
		hookPos = hook.position;
	}

	private Vector3 storageSlotFromIndex(int i){
		//Debug.LogFormat ("i:{0} -> {1}", i, new Vector3 ((i % 4) * (11f / 4f), Mathf.Floor (i / 4) * (11f / 4f), 0f));
		Vector3 outvec = new Vector3 (
			(i % 4) * 3 + 2,
			4 - Mathf.Floor (i / 4) * 3
			, 0f);
		return outvec;
	}

	// Use this for initialization
	void Start () {
		storage = new List<partBase> ();
		storage.Capacity = 4 * 4; // 4 by 4 slots for things
		outOfStorage = new List<partBase> ();
		toStorage = new List<partBase> ();
		chain0Pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!resting) {
			if (outOfStorage.Count == 0 && toStorage.Count == 0) {
				//no items left.
				resting = true;
			} else {
				//check to storage.
				if (toStorage.Count != 0) {
					//can we send this to storage?
					if (toStorage [0].testObscured ()) {
						toStorage.RemoveAt (0);
					} else {
						//send something to storage.
						//pick up thing.
						if (hookedObj == null) {
							target = toStorage [0].transform.position;
							target.z = hookPos.z;
							if (Vector3.Distance (target, hookPos) < 0.5f) {
								//try and remove object.
								if (toStorage [0].testRemoveable ()) {
									Debug.Log ("Removed object, going to storage");
									hookedObj = toStorage [0].gameObject;
									toStorage [0].removed = true;
								}
							}
						} else {
							//keep the object attached to end of chain.
							hookedObj.transform.position = hookPos;
							//find where to put the thing.
							target = storageSlotFromIndex (storage.Count);
							target.z = hookPos.z;
							//put thing in storage.
							if (Vector3.Distance (target, hookPos) < 0.5f) {
								hookedObj.transform.position = target;
								storage.Add (toStorage [0]);
								toStorage.RemoveAt (0);
								hookedObj = null;
							}
						}
					}
				} else {
					//can we attach this?
					if (outOfStorage [0].testObscured ()) {
						outOfStorage.RemoveAt (0);
					} else {
						//take something from storage.
						if (hookedObj == null) {
							//nothing hooked. find object in storage.
							target = outOfStorage [0].transform.position;
							target.z = hookPos.z;
							if (Vector3.Distance (target, hookPos) < 0.5f) {
								hookedObj = outOfStorage [0].gameObject;
								storage.Remove (outOfStorage [0]);
							}
						} else {
							//keep the object attached to end of chain.
							hookedObj.transform.position = hookPos;
							//find where to put the thing.
							target = outOfStorage [0].snapPoint;
							target.z = hookPos.z;
							//put thing in storage.
							if (Vector3.Distance (target, hookPos) < 0.5f) {
								hookedObj.transform.position = outOfStorage [0].snapPoint;
								outOfStorage [0].removed = false;
								hookedObj = null;
								foreach (screwBase a in outOfStorage[0].screws)
									us.addScrew (a);
								outOfStorage.RemoveAt (0);
							}
						}
					}
				}
			}
		} else {
			target = new Vector3 (0, 4, 0);
			if (outOfStorage.Count != 0 || toStorage.Count != 0)
				resting = false;
		}
		updateChain ();
		travelTo ();
	}
}
