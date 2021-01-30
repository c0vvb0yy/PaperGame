using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public bool SeesPlayer;
    public Transform Player;
    public Rigidbody Body;

    private Vector3 Home;
    public float MaxDist;

    // Start is called before the first frame update
    void Start()
    {
        Home = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(SeesPlayer)
        {
            transform.LookAt(Player);
            Body.velocity += transform.forward * MoveSpeed * Time.deltaTime;
        }
        else
        {
            float dist = Vector3.Distance(transform.position, Home);
            if(dist > MaxDist)
            {
                transform.LookAt(Home);
                Body.velocity += transform.forward * MoveSpeed * Time.deltaTime;
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
}
