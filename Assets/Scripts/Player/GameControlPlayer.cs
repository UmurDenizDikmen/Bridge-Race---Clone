using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using DG.Tweening;



public class GameControlPlayer : MonoBehaviour
{

    [Header("Lists")]
    public List<GameObject> CollectablesStack = new List<GameObject>();
    [Header("GameObjects")]
    public GameObject WinPanel;
    public GameObject InsPanel;
    public GameObject StackPoint;
    public GameObject[] ReturnDoors;
    public GameObject[] SpawnPointEndGame;
    public GameObject[] FirsSecondThirdPlace;
    public GameObject[] CollectableBlueParent;
    [Header("Transforms")]
    [SerializeField] private Transform spawnArea;
    [SerializeField] private Transform spawnArea2;
    public Transform Enemy1;
    public Transform Enemy2;
    [Header("OtherSettings")]
   
    Rigidbody rg;
  
    public static GameControlPlayer instance;
    Vector3 FirstPosition;
    Animator anim;
    public Material BlueMaterial;
    
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        FirstPosition = StackPoint.transform.localPosition;
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CollectablesBlue":
                other.transform.gameObject.GetComponent<Renderer>().material = BlueMaterial;
                other.transform.SetParent(transform);
                CollectablesStack.Add(other.gameObject);
                other.transform.DOLocalJump(StackPoint.transform.localPosition, 1, 1, 1).OnComplete(() =>
                  {
                      other.transform.gameObject.GetComponent<TrailRenderer>().enabled = false;
                  });
                other.transform.rotation = StackPoint.transform.rotation;
                StackPoint.transform.position += new Vector3(0, 0.2f, 0);
                other.tag = "Untagged";
                break;

            case "Brick":
            case "BrickFinal":
               if (CollectablesStack.Count != 0 && other.transform.gameObject.GetComponent<Renderer>().material.color != transform.GetChild(1).GetComponent<Renderer>().material.color)
                {

                    other.transform.gameObject.GetComponent<Renderer>().enabled = true;
                    other.transform.gameObject.GetComponent<Renderer>().material.color = transform.GetChild(1).GetComponent<Renderer>().material.color;
                    Destroy(CollectablesStack[^1]);
                    CollectablesStack.Remove(CollectablesStack[^1]);
                    StackPoint.transform.position -= new Vector3(0, 0.2f, 0);
                }
               break;
            case "Dead":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "Finish":
                GameObject Door3 = GameObject.FindWithTag("DoorMovement3");
                Door3.GetComponent<BoxCollider>().isTrigger = true;
                CollectableBlueParent[0].SetActive(false);
                CollectableBlueParent[1].SetActive(true);
                 break;
            case "Finish2":

                GameObject Door1 = GameObject.FindWithTag("DoorMovement");
                Door1.GetComponent<BoxCollider>().isTrigger = true;
                CollectableBlueParent[0].SetActive(false);
                CollectableBlueParent[1].SetActive(true);
                break;
            case "Finish3":

                GameObject Door2 = GameObject.FindWithTag("DoorMovement2");
                Door2.GetComponent<BoxCollider>().isTrigger = true;
                break;
            case "NoReturnPoint":
                ReturnDoors[0].SetActive(true);
                ReturnDoors[1].SetActive(true);
                break;
            case "Finish4":

                WinPanel.SetActive(true);
                FirsSecondThirdPlace[0].SetActive(true);
                FirsSecondThirdPlace[1].SetActive(true);
                FirsSecondThirdPlace[2].SetActive(true);
                Playermovement1.instance.speed = 0f;
                transform.position = SpawnPointEndGame[0].transform.position;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.GetComponent<Animator>().enabled = false;
                if (EnemyMovement.instance.CollectablesStackOnMe.Count > EnemyMovement2.instance.CollectablesStackOnMe2.Count)
                {
                    Enemy2.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    Enemy2.position = SpawnPointEndGame[2].transform.position;
                    Enemy2.rotation = Quaternion.Euler(0, 0, 0);
                    Enemy1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    Enemy1.position = SpawnPointEndGame[1].transform.position;
                    Enemy1.rotation = Quaternion.Euler(0, 0, 0);
                    Enemy1.gameObject.GetComponent<Animator>().enabled = false;
                    Enemy2.gameObject.GetComponent<Animator>().enabled = false;
                }
                else
                {
                    Enemy2.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    Enemy2.position = SpawnPointEndGame[2].transform.position;
                    Enemy2.position = SpawnPointEndGame[1].transform.position;
                    Enemy2.rotation = Quaternion.Euler(0, 0, 0);
                    GameObject Enemyfirst = GameObject.FindWithTag("Enemy1");
                    Enemyfirst.GetComponent<NavMeshAgent>().enabled = false;
                    Enemy1.position = SpawnPointEndGame[2].transform.position;
                    Enemy1.rotation = Quaternion.Euler(0, 0, 0);
                    Enemy2.gameObject.GetComponent<Animator>().enabled = false;
                    Enemy1.gameObject.GetComponent<Animator>().enabled = false;


                }
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "DoorMovement":
            case "DoorMovement2":
            case "DoorMovement3":
                MoveDoor(collision.gameObject);
                break;
            case "Enemy2":
                HandleEnemy2Collision();
                break;
            case "Enemy1":
                HandleEnemy1Collision();
                break;
            case "Floor":
                GameObject Door1 = GameObject.FindWithTag("DoorMovement");
                Door1.transform.position = Door1.transform.gameObject.GetComponent<DoorPos1>().DoorPosFirst;
                GameObject Door2 = GameObject.FindWithTag("DoorMovement2");
                Door2.transform.position = Door2.transform.gameObject.GetComponent<DoorPos2>().DoorPosSecond;
                GameObject Door3 = GameObject.FindWithTag("DoorMovement3");
                Door3.transform.position = Door3.transform.gameObject.GetComponent<DoorPos3>().DoorPosThird;
                break;
        }
    }
    private void MoveDoor(GameObject door)
    {
        door.transform.position += new Vector3(0, 0, -.364f) * CollectablesStack.Count;
    }
    private void HandleEnemy1Collision()
    {
        if (CollectablesStack.Count < EnemyMovement.instance.CollectablesStackOnMe.Count)
        {
            Death();
        }
    }
    private void HandleEnemy2Collision()
    {
        if (CollectablesStack.Count < EnemyMovement2.instance.CollectablesStackOnMe2.Count)
        {
            Death();
        }
    }

    private void Death()
    {
        rg.AddRelativeForce(Vector3.up * 4f, ForceMode.Impulse);
        anim.Play("death");
        DestroyChildObjects();
        CollectablesStack.Clear();
    }

    private void DestroyChildObjects()
    {
        int startIndex = 3;
        Transform parentObject = gameObject.transform;
        int childnumber = parentObject.childCount;
        for (int i = childnumber - 1; i >= startIndex; i--)
        {

            Transform Object = parentObject.GetChild(i);
            Object.DOJump(transform.position + new Vector3(Random.Range(-3f, 3f), 0.1f, Random.Range(-3f, 3f)), 3, 1, 1f);
            Object.gameObject.GetComponent<Renderer>().material.color = Color.grey;
            Object.gameObject.transform.tag = "CollectablesBlue";
            Object.gameObject.transform.SetParent(null);
            StackPoint.transform.localPosition = FirstPosition;
        }
        CollectablesStack.Clear();
    }



    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
