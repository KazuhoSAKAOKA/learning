using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public Transform patrolRoute;

    public List<Transform> locations;
    private int locationIndex = 0;
    private NavMeshAgent agent;
    private int _lives = 3;

    public int EnemyLives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            if(_lives <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = this.GetComponent<NavMeshAgent>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    void InitializePatrolRoute()
    {
        locations = new List<Transform>();
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    void MoveToNextPatrolLocation()
    {
        agent.destination = locations[locationIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            locationIndex = (locationIndex + 1) % locations.Count;
            MoveToNextPatrolLocation();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            agent.destination = player.position;
            Debug.Log("encount player. Attack");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.name== "Player")
        {
            Debug.Log("player out of range");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives--;
            Debug.Log("Enemy hit. Lives: " + EnemyLives);
        }
    }
}
