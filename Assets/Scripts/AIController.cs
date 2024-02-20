using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Vector3 randTargetLocation;
    private int dimension;
    private NavMeshAgent agent;
    private int multiplier = 6;

    public int distanceThreshold = 30;
    public GameObject player;
    private Vector3 targLocation;

    bool reachedLoc = false;
    void Start()
    {
        dimension = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>()._mazeWidth - 1;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        StartCoroutine(MoveDelay());
    }

    private void Update()
    {
        agent.destination = targLocation;

        //Checks if AI is near within a certain distance from the player, assuming it can search for the player
        if(player != null )
        {
            if (Vector3.Distance(transform.position, player.transform.position) < distanceThreshold)
            {
                targLocation = player.transform.position;
            }
        }
        else
        {
            player = GameObject.Find("Player");
        }
        
        
        //is it around the estimated target, generate a new place to go to
        if(Vector3.Distance(transform.position, targLocation) < 1 && reachedLoc == false)
        {
            reachedLoc = true;
            Debug.Log("At my target now!");
            targLocation = GenerateRandLocation();
        }

    }

    //Delay in movement because of the countdown in the beginning
    private IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(3);
        targLocation = GenerateRandLocation();
        
    }
    //Create random vector that can be used as a location for enemies to go to
    private Vector3 GenerateRandLocation()
    {
        randTargetLocation = new Vector3(Random.Range(0, dimension), 0, Random.Range(0, dimension));
        randTargetLocation *= multiplier;
        reachedLoc = false;
        //Debug.Log(randTargetLocation);
        return randTargetLocation;
    }
}
