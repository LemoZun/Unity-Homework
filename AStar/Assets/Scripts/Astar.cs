using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] List<Vector2Int> path;
    [SerializeField] float overLapCircleRadius;
    [SerializeField] float offSet = 0.5f; // 경로의 시작과 끝 위치 조정.. 해보려 했지만 잘 안된다

    public const int CostStraight = 10;
    public const int CostDiagonal = 14;

    // 방향들의 벡터
    static Vector2Int[] direction =
    {
        new Vector2Int( 0,  1), // 상
        new Vector2Int( 0, -1), // 하
        new Vector2Int(-1,  0), // 좌
        new Vector2Int( 1,  0), // 우
        new Vector2Int(-1,  1), // 좌상
        new Vector2Int( 1,  1), // 우상
        new Vector2Int(-1, -1), // 좌하
        new Vector2Int( 1, -1), // 우하
    };

    private void Update()
    {
        //Vector2Int start = new Vector2Int(Mathf.FloorToInt((int)startPos.position.x + offSet), Mathf.FloorToInt((int)startPos.position.y + offSet));
        //Vector2Int end = new Vector2Int(Mathf.FloorToInt((int)endPos.position.x + offSet), Mathf.FloorToInt((int)endPos.position.y + offSet));

        Vector2Int start = new Vector2Int((int)startPos.position.x, (int)startPos.position.y);
        Vector2Int end = new Vector2Int((int)endPos.position.x, (int)endPos.position.y);

        bool isSuccess = AStar(start, end, out path, overLapCircleRadius);
        if (isSuccess)
        {
            Debug.Log("경로 탐색 성공!");
        }
        else
        {
            Debug.Log("경로 탐색 실패!");
        }

        if (path != null && path.Count > 1)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 from = new Vector3(path[i].x + offSet, path[i].y+ offSet, 0);
                Vector3 to = new Vector3(path[i + 1].x + offSet, path[i + 1].y+ offSet, 0);
                Debug.DrawLine(from, to, Color.red);
            }
        }
    }

    public static bool AStar(Vector2Int start, Vector2Int end, out List<Vector2Int> path, float radius)
    {
        // 0. 사전 세팅
        List<ASNode> openList = new List<ASNode>(); // 탐색할 정점의 후보들을 보관하는 리스트
        Dictionary<Vector2Int, bool> closeSet = new Dictionary<Vector2Int, bool>(); // 탐색이 완료된 정점들을 보관
        path = new List<Vector2Int>(); // 경로를 보관할 리스트

        // 처음으로 탐색할 정점을 openList에 추가한다
        openList.Add(new ASNode(start, null, 0, Heuristic(start, end)));

        while(openList.Count > 0) // 탐색할 노드가 남아있는 동안 반복
        {
            // 1. 다음으로 탐색할 정점을 선택한다
            //    F가 가장 낮은 정점을 선택한다
            //    F가 같다면 H가 가장 낮은 정점을 선택한다  
            ASNode nextNode = NextNode(openList);

            // 2. 선택된 정점을 탐색했다고 표시하기 위해
            //    openList에서 제거하고 closeSet에 추가
            openList.Remove(nextNode); // 다음으로 탐색할 정점을 탐색할 후보들에서 제거
            closeSet.Add(nextNode.pos, true); // 탐색을 완료한 정점셋에 추가한다. 탐색을 했다는 표시인 true로 갱신

            // 3. 다음으로 탐색할 정점이 도착지인경우 
            //    경로 탐색에 성공했으니 path를 반환하고 종료한다.
            if(nextNode.pos == end)
            {
                ASNode curNode = nextNode;
                // 역으로 경로를 추적한다
                // ASNode에 저장된 parent를 따라가서 추적한다
                while(curNode != null)
                {
                    path.Add(curNode.pos); 
                    curNode = curNode.parent;
                }

                path.Reverse(); //경로를 뒤집어 올바른 순서로 변경한다
                return true;    // 경로 탐색에 성공했다고 true 반환
            }

            // 4. 주변 정점들의 점수를 계산한다
            for(int i = 0; i < direction.Length; i++)
            {
                Vector2Int pos = nextNode.pos + direction[i]; // 각 방향들에 따른 위치 계산

                // 탐색하면 안되는 경우는 제외한다.
                // 4-1. 이미 탐색한 정점일 경우
                if(closeSet.ContainsKey(pos))
                    continue;
                // 4-2. 가지 못하는 지형일경우
                // tilemap.HasTile : 타일맵을 분석하는 방법
                // Physics.Overlap : 충돌체의 존재 여부를 확인하는 방법
                // Physics.Raycast : 중간에 장애물이 없는지 레이캐스트로 확인하는 방법
                if(Physics2D.OverlapCircle(pos,0.2f) != null)
                    continue;
                // 4-3 대각선 움직임 처리
                // 4-3-a. 대각선 중 두 방향 모두 막힌 경우
                if (
                    i>=4 && // i가 4 이상일경우 대각선 방향으로 위 방향배열에 정의됨 
                    Physics2D.OverlapCircle(new Vector2(pos.x,nextNode.pos.y), radius) != null
                    &&
                    Physics2D.OverlapCircle(new Vector2(nextNode.pos.x, pos.y), radius) != null
                    )
                {
                    continue;
                }
                // 4-3-b. 대각선 중에서도 둘 다 뚫려 있어야 허용
                if (
                    i >= 4 && // i가 4 이상일경우 대각선 방향으로 위 방향배열에 정의됨 
                    Physics2D.OverlapCircle(new Vector2(pos.x, nextNode.pos.y), radius) != null
                    ||
                    Physics2D.OverlapCircle(new Vector2(nextNode.pos.x, pos.y), radius) != null
                    )
                {
                    continue;
                }

                // 4-4 점수 계산
                int g; // 이동 비용
                // 직선으로 움직인 경우
                if(pos.x == nextNode.pos.x || pos.y == nextNode.pos.y )
                {
                    g = nextNode.g + CostStraight;
                }
                // 대각선으로 움직인 경우 
                else
                {
                    g = nextNode.g + CostDiagonal;
                }
                int h = Heuristic(pos, end);
                int f = g + h;

                // 4-5 정점의 점수 갱신이 필요한 경우
                ASNode findNode = FindNode(openList, pos); // openList 내의 각 위치에 해당하는 노드를 찾기 위함
                // 점수가 없었던 경우 점수를 갱신함
                if(findNode == null) 
                {
                    openList.Add(new ASNode(pos, nextNode, g, h)); // 노드가 없었다면 새로 추가한다
                }
                // 기존 노드보다 f값이 낮다면 갱신한다
                else if(findNode.f > f)
                {
                    findNode.f = f;
                    findNode.g = g;
                    findNode.h = h;
                    findNode.parent = nextNode;
                }
            }
        }

        // 찾지 못했을 경우
        path = null;
        return false;
    }

    // 휴리스틱 : 최상의 경로를 추정하는 순위값
    //           휴리스틱에 의해 경로탐색의 효율이 결정됨
    public static int Heuristic(Vector2Int start, Vector2Int end)
    {
        int xSize = Mathf.Abs(start.x - end.x); // x축의 거리 계산
        int ySize = Mathf.Abs(start.y - end.y); // y축의 거리 계산

        // 맨해튼 거리 : 직선을 통해 이동하는 거리
        // return xSize + ySize;

        // 유클리드 거리 : 대각선을 통해 이동하는 거리
        // return (int)Vector2Int.Distance(start, end);

        // 타일맵 거리 : 직선과 대각선을 통해 이동하는 거리
        int straightCount = Mathf.Abs(xSize - ySize); // 직선 거리
        int diagonalCount = Mathf.Max(xSize,ySize) - straightCount; // 대각선 거리
        return CostStraight * straightCount + CostDiagonal * diagonalCount; // 총 비용 반환
    }

    public static ASNode NextNode(List<ASNode> openList)
    {
        // F가 가장 낮은 정점을 선택, 같다면 H가 가장 낮은 정점을 선택한다
        int curF = int.MaxValue; // 최소 F값을 찾기 위해 초기값을 MaxValue로 설정
        int curH = int.MinValue; // 최소 H값을 찾기 위해 초기값을 MaxValue로 설정
        ASNode minNode = null;   // 최소 F값을 가진 노드를 저장

        for(int i = 0; i < openList.Count; i++) //openList에서 최소 F값을 가진 노드를 탐색
        {
            // 탐색한 노드중 현재 저장된 f값보다 더 낮은 f값을 찾았을 시
            // 해당 노드의 f,h값들을 저장하고 minNode에 노드를 저장
            if (curF > openList[i].f) 
            {
                curF = openList[i].f;
                curH = openList[i].h;
                minNode = openList[i];
            }
            // 탐색한 노드가 현재 저장된 F값과 F값이 같고 H값을 비교했을때 H값이 더 낮을경우
            // 해당 노드의 f,h값들을 저장하고 minNode에 노드를 저장
            else if (curF == openList[i].f && curH > openList[i].h)
            {
                // curF = openList[i].f; //f값이 같으니 생략해도 무방하다
                curH = openList[i].h;
                minNode = openList[i];
            }
        }
        //반복문을 통해 찾은 노드를 반환
        return minNode; 
    }
    public static ASNode FindNode(List<ASNode> openList, Vector2Int pos)
    {
        for(int i = 0;i < openList.Count; i++)
        {
            if (openList[i].pos == pos)
            {
                return openList[i];
            }
        }
        return null;
    }
}
