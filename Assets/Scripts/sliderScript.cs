using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderScript : MonoBehaviour
{

    public Transform frame;

    private Vector3 frameOffset;
    // Start is called before the first frame update
    void Start()
    {
        frameOffset = transform.position - frame.position;
    }

 private Vector3 screenPoint;
 private Vector3 offset;
 
 void OnMouseDown()
 {
     screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
     offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

 }
 
 void OnMouseDrag()
 {
     Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    curPosition.x = transform.position.x;
    curPosition.z = transform.position.z;
    transform.position = curPosition;
    frame.position = transform.position - frameOffset;
 
 }
}
