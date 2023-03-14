using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControllerBlue : MonoBehaviour
{
    public GameObject CollectablesBlueObject;
     void Start()
    {
        InvokeRepeating("BlueSpawnController",0f,2f);
    }
    void BlueSpawnController()
    {
         if(transform.childCount < 1)
         {
              GameObject obj = Instantiate(CollectablesBlueObject,transform.position,transform.rotation);
              obj.transform.SetParent(transform);
         }
    }

}
 
 