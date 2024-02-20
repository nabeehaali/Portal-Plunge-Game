using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    //[SerializeField]
    public int _mazeWidth;

    //[SerializeField]
    public int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    //public NavMeshSurface surface;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for(int i = 0; i < _mazeWidth; i++)
        {
            for(int j = 0; j < _mazeDepth; j++)
            {
                _mazeGrid[i, j] = Instantiate(_mazeCellPrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        //GetComponent<NavMeshSurface>().BuildNavMesh();
        transform.localScale = new Vector3(6, 6, 6);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
        
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisteCells = GetUnvistedCells(currentCell);

        return unvisteCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvistedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;
        //left and right cells
        if(x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if(cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }
        if(x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if(cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }
        //front + back cells
        if(z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
