using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
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
}
