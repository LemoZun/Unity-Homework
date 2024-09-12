using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject planePrefab;
    [SerializeField] NavMeshSurface NavMeshSurface;

    private Vector2 wallSize = new Vector2(1, 1);

    Coroutine twoSecondDelayRoutine;

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

    private string[,] stringMap;


    private void Awake()
    {
        

    }

    private void Start()
    {
        stringMap = MapParse();
        GenerateMap(stringMap);
        // �̷��� ���� ���� �� �ִϸ��̼� ���� �� ����ũ�� �Ѵ�
        NavMeshSurface.BuildNavMesh();
        if (twoSecondDelayRoutine == null)
        {
            twoSecondDelayRoutine = StartCoroutine(DelayTwoSecondRoutine());
        }
    }


    private void GenerateMap()
    {
        
        for(int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0; j < map.GetLength(0); j++)
            {
                if(map[i,j] != 0)
                {
                    Vector3 position = new Vector3(i * wallSize.x-4.5f, -5, j * wallSize.y-4.5f); // ��ġ�� ���߱����� ��ǥ���� 4.5�� ����
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);

                    Animator spawnStarter = wall.GetComponent<Animator>();
                    if (spawnStarter != null)
                        spawnStarter.SetTrigger("gameStart");
                }

            }
        }
    }


    private void GenerateMap(string[,] _map)
    {
        Vector3 position;

        int mapHeight = _map.GetLength(0);
        int mapWidth = _map.GetLength(1);

        float planeScaleZ = mapHeight / 10f;
        float planeScaleX = mapWidth / 10f; 

        //�ٴ� �����
        GameObject plane = Instantiate(planePrefab, new Vector3(0,0,0), Quaternion.identity);
        plane.transform.localScale = new Vector3(planeScaleX, 1, planeScaleZ);

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0;x < map.GetLength(0); x++)
            {
                if (_map[y, x] == "wall")
                {
                    position = new Vector3(y * wallSize.x - 4.5f, -5f, x * wallSize.y - 4.5f); // ��ġ�� ���߱����� ��ǥ���� 4.5�� ����
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);

                    Animator spawnStarter = wall.GetComponent<Animator>();
                    if (spawnStarter != null)
                        spawnStarter.SetTrigger("gameStart");
                }
                else if(_map[y, x] == "coin")
                {
                    position = new Vector3(y * wallSize.x - 4.5f, 0.2f, x * wallSize.y - 4.5f); // ��ġ�� ���߱����� ��ǥ���� 4.5�� ����
                    GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
                }
                else if (_map[y,x] == "player")
                {
                    position = new Vector3(y * wallSize.x - 4.5f, 1f, x * wallSize.y - 4.5f); // ��ġ�� ���߱����� ��ǥ���� 4.5�� ����
                    GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
                }
            }
        }
    }



    private IEnumerator DelayTwoSecondRoutine()
    {
        yield return new WaitForSeconds(3f);
        
        NavMeshSurface.UpdateNavMesh(NavMeshSurface.navMeshData);
        Debug.Log("�׺�޽� ������Ʈ �Ϸ�");
    }

    private string LoadCSV()
    {
        string path = $"{Application.dataPath}/CSV";

        if (!Directory.Exists(path))
        {
            Debug.LogError("��ΰ� �����ϴ�.");
            return null;
        }

        if (!File.Exists(path + "/SampleMap.csv"))
        {
            Debug.LogError("������ �����ϴ�");
            return null;
        }

        string file = File.ReadAllText(path + "/SampleMap.csv");
        Debug.Log(file);
        return file;
    }

    private string[,] MapParse()
    {
        string[] lines = LoadCSV().Split('\n');  // {"wall, wall ,wall..." , "wall, 0, 0, coin, ..."} ... 
        int mapHeight = lines.Length;
        int mapWidth = lines[0].Split(',').Length;
        string[,] map = new string[mapHeight, mapWidth];

        if(lines == null)
        {
            Debug.LogError("�Ľ̰��� �߸���");
            return null;
        }

        

        for(int y = 0; y < mapHeight; y++)
        {
            string[] line = lines[y].Split(","); // ���ڿ� �迭 ����
            for(int x=0; x< mapWidth; x++)
            {
                map[y,x] = line[x].Trim(); // �ڵ��ϼ����� ���� Trim , ������ �������شٰ� �Ѵ�. 
            }
        }

        return map;

        /*
        for(int i=0; i < mapHeight; i++)
        {
            for(int j=0; j<mapWidth; j++)
            {
                Debug.Log($"map[{i},{j}] = {map[i, j]}");
            }
        }
        */
    }

}
