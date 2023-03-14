using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPos1 : MonoBehaviour
{
    public Vector3 DoorPosFirst;
    private void Start()
    {
        DoorPosFirst = transform.position;
    }
}