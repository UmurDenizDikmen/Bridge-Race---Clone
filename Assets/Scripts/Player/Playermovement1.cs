using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement1 : MonoBehaviour
{
    public FloatingJoystick joystick;
    public float speed;
    Animator anim;
    public static Playermovement1 instance;
    Vector3 direction;
    public GameObject bg;
    public bool isGameStart = false;
    public GameObject InsPanel;
    public List<GameObject> Enemies = new List<GameObject>();
    void Start()
    {
        anim = GetComponent<Animator>();
        instance = this;
    }
 
    void GameStart()
    {
       if(Input.GetMouseButtonDown(0)&&isGameStart == false)
       {
            isGameStart = true;
            InsPanel.SetActive(false);
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[0].GetComponent<EnemyMovement>().MoveEnemy();
                Enemies[1].GetComponent<EnemyMovement2>().MoveEnemy();
            }
       }
    }
    void Update()
    {
        GameStart();
        if(isGameStart == true)
        {
         if (Input.GetMouseButton(0))
         {
            Move();
            anim.SetBool("isRunning", true);
         }
         else
         {
            anim.SetBool("isRunning", false);
         }
        }
      
    }
    private void Move()
    {
        if(bg.activeInHierarchy)
        {
         direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
         transform.position += -direction * speed * Time.fixedDeltaTime;
         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-direction), 2f * Time.fixedDeltaTime);
        }
    
    }
}
