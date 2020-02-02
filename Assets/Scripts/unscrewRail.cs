using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unscrewRail : MonoBehaviour
{

    public Transform screwBit;

    void FixedUpdate()
    {
        Vector3 newVec = transform.position;
        newVec.y = screwBit.position.y;
        transform.position = newVec;
    }
}
