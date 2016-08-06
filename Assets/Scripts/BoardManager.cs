using UnityEngine;
using System; //for Serializable
using System.Collections.Generic; //for list
using Random = UnityEngine.Random; //Select Unity Engine Random 

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }

    }

    //Game board size
    public int columns = 8;
    public int rows = 8;

    //Control how many objects are there in level
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    //GameObject
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    //for fold hierarchy objects
    private Transform boardHolder;
    //Chasing objects position
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for(int x = 1; x< columns -1; x++)
        {
            for(int y = 1; y < rows -1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Setup floorTiles and outerWallTitles
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for(int x=-1; x<columns + 1; x++)
        {
            for(int y = -1; y <rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                    //Make outerWall objects
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                //Make instance GameObject, Quarternion.identity => no rotate
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //Set parent
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    //Make Random Position
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        //prevent duplication
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    //Spawn Layout Objects using randompositon
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Count how many object in level
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }

    }

    //Set up Scene and called by GameManger
    public void SetupScene(int level)
    {
        //Making floor tile map
        BoardSetup();
        //Setting list clear and build list attribute
        InitialiseList();
        //Making Random Objects
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //Control enemy count by level log function
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Exit prefab
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

    }


}
