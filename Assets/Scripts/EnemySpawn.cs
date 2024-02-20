using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    private List<Vector3> enemyPositions;
    private int dimension;
    private int multiplier = 6;
    void Start()
    {
        dimension = (GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>()._mazeWidth - 1) * multiplier;
        enemyPositions = new List<Vector3>() { new Vector3(0, 3, 0), new Vector3(0, 3, dimension), new Vector3(dimension, 3, 0), new Vector3(dimension, 3, dimension) };

        //Spawn 4 enemies, one at each corner
        for (int i = 0; i < enemyPositions.Count; i++)
        {
            Instantiate(enemyPrefab, enemyPositions[i], Quaternion.identity, transform);
        }
    }
}
