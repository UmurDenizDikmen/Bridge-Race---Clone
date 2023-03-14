using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Linq;



public class EnemyMovement : MonoBehaviour
{
    [Header("OtherSettings")]
    public NavMeshAgent agent;
    Rigidbody rg;
    Animator anim;
    public float rangeCollectable;
    public float rangeBrick;
    public int Counter = 0;
    public float range;
    public float spawnInterval;
    public static EnemyMovement instance;
    [Header("GameObjects")]

    public GameObject StackPoint;
    public GameObject[] CollectableRedParent;

    public GameObject[] Floor;
    [Header("Lists")]

    public List<GameObject> CollectablesStack2 = new List<GameObject>();
    public List<GameObject> CollectablesStackOnMe = new List<GameObject>();

    [Header("Transforms")]
    public Transform[] UpPoint;
    Vector3 FirstPosition;
    public Material RedMaterial;
    public void MoveEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeCollectable);
        foreach (Collider obj in hitColliders)
        {
            if (obj.gameObject.CompareTag("CollectablesRed"))
            {

                if (!CollectablesStack2.Contains(obj.gameObject))
                {
                    CollectablesStack2.Add(obj.gameObject);

                }
            }
        }
        anim.SetBool("isWalking", true);
        agent.SetDestination(CollectablesStack2[0].transform.position);
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rg = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        instance = this;
        FirstPosition = StackPoint.transform.localPosition;
    }
    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeCollectable);
        foreach (Collider obj in hitColliders)
        {
            if (obj.gameObject.CompareTag("CollectablesRed"))
            {

                if (!CollectablesStack2.Contains(obj.gameObject))
                {
                    CollectablesStack2.Add(obj.gameObject);

                }
            }
        }
        CollectablesStack2 = CollectablesStack2.Where(item => item != null).ToList();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Finish":

                Counter++;
                Destroy(other.gameObject);
                CollectableRedParent[0].SetActive(false);
                CollectableRedParent[1].SetActive(true);
                CollectablesStack2.Clear();
                MoveEnemy();
                break;
            case "Finish3":
                agent.SetDestination(UpPoint[1].transform.position);
                break;
            case "CollectablesRed":
                other.transform.gameObject.GetComponent<Renderer>().material = RedMaterial;
                other.transform.SetParent(transform);
                other.transform.DOLocalJump(StackPoint.transform.localPosition, 1, 1, 1).OnComplete(() =>
                {
                    other.transform.gameObject.GetComponent<TrailRenderer>().enabled = false;
                });
                CollectablesStack2.Remove(other.gameObject);
                CollectablesStackOnMe.Add(other.gameObject);
                StackPoint.transform.position += new Vector3(0, 0.2f, 0);
                other.transform.rotation = StackPoint.transform.rotation;
                other.tag = "Untagged";
                if (!agent.enabled) return;
                agent.SetDestination(CollectablesStack2[0].transform.position);
                if (CollectablesStackOnMe.Count > Random.Range(1, 11))
                {
                    if (!agent.enabled) return;
                    agent.SetDestination(UpPoint[Counter].transform.position);
                }
                break;
            case "Brick":
                if (CollectablesStackOnMe.Count != 0 && other.transform.gameObject.GetComponent<Renderer>().material.color != transform.GetChild(1).GetComponent<Renderer>().material.color)
                {

                    other.transform.gameObject.GetComponent<Renderer>().enabled = true;
                    other.transform.gameObject.GetComponent<Renderer>().material.color = transform.GetChild(1).GetComponent<Renderer>().material.color;
                    Destroy(CollectablesStackOnMe[^1]);
                    CollectablesStackOnMe.Remove(CollectablesStackOnMe[^1]);
                    StackPoint.transform.position -= new Vector3(0, 0.2f, 0);
                    if (CollectablesStackOnMe.Count == 0)
                    {
                        agent.SetDestination(Floor[0].transform.position);
                    }
                }
                break;
            case "BrickFinal":
                if (CollectablesStackOnMe.Count != 0 && other.transform.gameObject.GetComponent<Renderer>().material.color != transform.GetChild(1).GetComponent<Renderer>().material.color)
                {

                    other.transform.gameObject.GetComponent<Renderer>().enabled = true;
                    other.transform.gameObject.GetComponent<Renderer>().material.color = transform.GetChild(1).GetComponent<Renderer>().material.color;
                    Destroy(CollectablesStackOnMe[^1]);
                    CollectablesStackOnMe.Remove(CollectablesStackOnMe[^1]);
                    StackPoint.transform.position -= new Vector3(0, 0.2f, 0);
                    if (CollectablesStackOnMe.Count == 0)
                    {
                        agent.SetDestination(Floor[1].transform.position);
                    }
                }
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":

                if (CollectablesStackOnMe.Count < GameControlPlayer.instance.CollectablesStack.Count)
                {
                    anim.SetBool("isWalking", false);
                    agent.enabled = false;
                    rg.isKinematic = false;
                    rg.AddRelativeForce(Vector3.up * 3f, ForceMode.Impulse);
                    Invoke("EnableAgentAgain", 1f);
                    int startIndex = 3;
                    Transform parentObject = gameObject.transform;
                    int childnumber = parentObject.childCount;
                    for (int i = childnumber - 1; i >= startIndex; i--)
                    {

                        parentObject.GetChild(i).DOJump(collision.transform.position + new Vector3(Random.Range(-3f, 3f), 0.1f, Random.Range(-3f, 3f)), 3, 1, 1f);
                        parentObject.GetChild(i).gameObject.GetComponent<Renderer>().material.color = Color.grey;
                        parentObject.GetChild(i).gameObject.transform.tag = "CollectablesBlue";
                        parentObject.GetChild(i).gameObject.transform.SetParent(null);

                    }
                    CollectablesStackOnMe.Clear();
                    StackPoint.transform.localPosition = FirstPosition;
                }

                break;
            case "Enemy2":
                if (CollectablesStackOnMe.Count < EnemyMovement2.instance.CollectablesStackOnMe2.Count)
                {
                    anim.SetBool("isWalking", false);
                    agent.enabled = false;
                    rg.isKinematic = false;
                    rg.AddRelativeForce(Vector3.up * 3f, ForceMode.Impulse);
                    Invoke("EnableAgentAgain", 1f);
                    int startIndex = 3;
                    Transform parentObject = gameObject.transform;
                    int childnumber = parentObject.childCount;
                    for (int i = childnumber - 1; i >= startIndex; i--)
                    {

                        parentObject.GetChild(i).DOJump(collision.transform.position + new Vector3(Random.Range(-3f, 3f), 0.1f, Random.Range(-3f, 3f)), 3, 1, 1f);
                        parentObject.GetChild(i).gameObject.GetComponent<Renderer>().material.color = Color.grey;
                        parentObject.GetChild(i).gameObject.transform.tag = "CollectablesBlue";
                        parentObject.GetChild(i).gameObject.transform.SetParent(null);
                    }
                    CollectablesStackOnMe.Clear();
                    StackPoint.transform.localPosition = FirstPosition;
                }
                break;
        }
    }
    void EnableAgentAgain()
    {
        agent.enabled = true;
        rg.isKinematic = true;
        MoveEnemy();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeCollectable);
    }

}


