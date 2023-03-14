using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControlRed : MonoBehaviour
{
    public GameObject CollectablesRedObject;
    
    private void Start()
    {
        InvokeRepeating("RedSpawnController",0f,2f);
    } 
    private void RedSpawnController()
    {
         if(transform.childCount < 1)
         {
              GameObject obj = Instantiate(CollectablesRedObject,transform.position,transform.rotation);
              obj.transform.SetParent(transform);
         }
    }
}