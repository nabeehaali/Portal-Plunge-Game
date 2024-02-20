using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    public GameObject warpPortalPrefab;
    public GameObject timePortalPrefab;
    public GameObject ghostPortalPrefab;

    public List<GameObject> allPortals;
    private List<Vector3> randPositions;
    private List<Vector3> selectedRandPositions;
    private int multiplier = 6;
    void Start()
    {
        randPositions = new List<Vector3>() { new Vector3(10*multiplier, 0, 3*multiplier), new Vector3(10*multiplier, 0, 17*multiplier), new Vector3(3*multiplier, 0, 10*multiplier), new Vector3(17*multiplier, 0, 10*multiplier) };
        selectedRandPositions = GenerateRandLocation();
        allPortals = new List<GameObject>();

        //spawn warp portals (random within options location)
        allPortals.Add(Instantiate(warpPortalPrefab, selectedRandPositions[0], Quaternion.identity, transform));
        allPortals.Add(Instantiate(warpPortalPrefab, selectedRandPositions[1], Quaternion.identity, transform));

        //spawn time portal (random within options location)
        int timePortalSlot = Random.Range(0, randPositions.Count);
        Vector3 timePortalLoc = randPositions[timePortalSlot];
        randPositions.RemoveAt(timePortalSlot);
        allPortals.Add(Instantiate(timePortalPrefab, timePortalLoc, Quaternion.identity, transform));

        //spawn enemy portal (random within options location)
        int enemyPortalSlot = Random.Range(0, randPositions.Count);
        Vector3 enemyPortalLoc = randPositions[enemyPortalSlot];
        randPositions.RemoveAt(enemyPortalSlot);
        allPortals.Add(Instantiate(ghostPortalPrefab, enemyPortalLoc, Quaternion.identity, transform));
    }

    List<Vector3> GenerateRandLocation()
    {
        List<Vector3> results = new List<Vector3>();
        
        //selecting first random point
        int randLocationIndex = Random.Range(0, randPositions.Count);
        Vector3 randLocation1 = randPositions[randLocationIndex];
        results.Add(randLocation1);
        randPositions.RemoveAt(randLocationIndex);

        //finds which one was selected, in order to get the second location that is across from it
        if(randLocation1.x == 10 * multiplier)
        {
            for(int i = 0; i < randPositions.Count; i++)
            {
                if(randPositions[i].x == 10 * multiplier)
                {
                    results.Add(randPositions[i]);
                    randPositions.RemoveAt(i);
                }
            }
        }
        else if(randLocation1.z == 10 * multiplier)
        {
            for (int i = 0; i < randPositions.Count; i++)
            {
                if (randPositions[i].z == 10 * multiplier)
                {
                    results.Add(randPositions[i]);
                    randPositions.RemoveAt(i);
                }
            }
        }
        else
        {
            Debug.Log("Nothing!");
        }

        return results;
    }

}
