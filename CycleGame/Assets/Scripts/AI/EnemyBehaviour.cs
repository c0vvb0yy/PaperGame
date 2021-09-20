using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    public float MoveSpeed = 70f;
    public bool SeesPlayer;
    public Transform Player;
    public Rigidbody Body;

    public int ExperiencePoints;

    //private Camera camera;

    private Vector3 home;
    public Vector3 PlayerDirection;
    public float MaxDist;

    public GameObject[] Enemies;

    public UnityEvent Defeat;
    
    // Start is called before the first frame update
    void Start()
    {
        home = transform.position;
        //camera = FindObjectOfType<Camera>();
        Defeat.AddListener(DisposeEnemy);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(SeesPlayer)
        {
            PlayerDirection = (Player.position - this.transform.position).normalized;
            //Body.AddRelativeForce(PlayerDirection.normalized * MoveSpeed, ForceMode.Force);
            Body.velocity = PlayerDirection * (MoveSpeed * Time.deltaTime);
        }
        else
        {
            float dist = Vector3.Distance(transform.position, home);
            if(dist > MaxDist)
            {
                Vector3 homeDirection = (home - this.transform.position).normalized;
                Body.velocity = homeDirection * (MoveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            SeesPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            SeesPlayer = false;
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        PlayerManager.SavePos();
        CompanionManager.SavePos();
        BattleSystem.LoadEnemies(this, SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("BattleArena", LoadSceneMode.Additive);
    }

    private void DisposeEnemy()
    {
        //Play defeat Animation
        Destroy(this.gameObject, 0.5f);
    }
}
