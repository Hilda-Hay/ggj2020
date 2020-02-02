using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerBehavior : MonoBehaviour {

	public Unscrewer us;
	public clawScript cs;

	// Use this for initialization
	void Start () {
		cs.us = us;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			if (hit.collider != null) {
				partBase part = hit.transform.gameObject.GetComponent<partBase> ();
				if (part != null) {
					//hit an object with a part.
					if (part.removed) {
						//if part IS removed
						if(cs.resting)cs.attachFromStorage(part);

					} else {
						//if part IS NOT removed
						if(cs.resting)cs.sendToStorage(part);
					}
				}
			}
		}
	}
}
