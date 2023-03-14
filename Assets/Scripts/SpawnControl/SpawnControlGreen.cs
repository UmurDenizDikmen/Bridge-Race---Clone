using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControlGreen : MonoBehaviour
{
    public GameObject CollectablesGreenObject;
    
    private void Start()
    {
        InvokeRepeating("GreenSpawnController",0f,2f);
    }
    private void GreenSpawnController()
    {
         if(transform.childCount < 1)
         {
            GameObject obj = Instantiate(CollectablesGreenObject,transform.position,transform.rotation);
            obj.transform.SetParent(transform); 
         }
    }
}
