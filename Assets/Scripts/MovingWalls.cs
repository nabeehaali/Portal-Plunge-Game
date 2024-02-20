using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWalls : MonoBehaviour
{
    public int numberWalls;
    public Material DoorCol;
    public GameObject player;
    public GameObject goUI;

    public List<GameObject> wallOptions;
    void Start()
    {
        foreach(GameObject fragment in GameObject.FindGameObjectsWithTag("Fragment")) { 
            fragment.SetActive(false);
        }
        player.SetActive(false);
        StartCoroutine(WallsDelay());
    }

    //Gather all the wall gamobjects and choose a random amount of them to be breakable and destroy the walls in the same location (since they are layered on top of each other)
    private IEnumerator WallsDelay()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        { 
            wallOptions.Add(wall);
        }
        
        for (int i = 0; i < numberWalls; i++)
        {
            GameObject chosenWall = wallOptions[Random.Range(0, wallOptions.Count)];
            chosenWall.tag = "Door";
            chosenWall.transform.GetChild(0).GetComponent<MeshRenderer>().material = DoorCol;
            wallOptions.Remove(chosenWall);

            foreach (GameObject go in wallOptions)
            {
                if (go.transform.position == chosenWall.transform.position)
                {
                    Destroy(go);
                }
            }

            
        }

        //spawn the stars
        yield return new WaitForSeconds(1);
        for(int i = 0; i < GameObject.Find("FragmentsSpawn").gameObject.transform.childCount; i++)
        {
            GameObject.Find("FragmentsSpawn").gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        //spawn the player
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        StartCoroutine(showText());
    }

    //show text for small amount of time
    private IEnumerator showText()
    {
        goUI.SetActive(true);
        yield return new WaitForSeconds(3);
        goUI.SetActive(false);
    }
}
