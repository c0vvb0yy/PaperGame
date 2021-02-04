using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float MoveSpeed = 70f;
    public bool SeesPlayer;
    public Transform Player;
    public Rigidbody Body;

    private Vector3 Home;
    public Vector3 PlayerDirection;
    public float MaxDist;

    // Start is called before the first frame update
    void Start()
    {
        Home = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(SeesPlayer)
        {
            PlayerDirection = (Player.position - this.transform.position).normalized;
            //Body.AddRelativeForce(PlayerDirection.normalized * MoveSpeed, ForceMode.Force);
            Body.velocity = PlayerDirection * MoveSpeed * Time.deltaTime;
        }
        else
        {
            float dist = Vector3.Distance(transform.position, Home);
            if(dist > MaxDist)
            {
                Vector3 homeDriection = (Home - this.transform.position).normalized;
                Body.velocity = homeDriection * MoveSpeed * Time.deltaTime;
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
