using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject wallPrefab;
    private Vector2 wallSize = new Vector2(1, 1);

    private int[,] map =
    {
        {1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,1,0,0,0,0,1},
        {1,1,1,0,1,0,0,0,0,1},
        {1,0,0,0,0,0,1,1,0,1},
        {1,0,0,0,0,0,1,0,0,1},
        {1,0,0,0,0,0,1,0,0,1},
        {1,0,0,0,0,0,1,0,0,1},
        {1,0,1,0,0,0,1,0,1,1},
        {1,0,1,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1},
    };

    private void Start()
    {
        //wallSize = GetComponent<Vector2>(wallPrefab.transform.localScale.x,wallPrefab.transform.localScale.y);

        GenerateMap();
    }


    private void GenerateMap()
    {
        for(int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0; j < map.GetLength(0); j++)
            {
                if(map[i,j] != 0)
                {
                    Vector3 position = new Vector3(j * wallSize.x-4.5f, 1, i * wallSize.y-4.5f); // À§Ä¡¸¦ ¸ÂÃß±âÀ§ÇØ ÁÂÇ¥¿¡¼­ 4.5¸¦ »©ÁÜ
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
                }

            }
        }
    }
}
