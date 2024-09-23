using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] List<Vector2Int> path;
    [SerializeField] float overLapCircleRadius;
    [SerializeField] float offSet = 0.5f; // ����� ���۰� �� ��ġ ����.. �غ��� ������ �� �ȵȴ�

    public const int CostStraight = 10;
    public const int CostDiagonal = 14;

    // ������� ����
    static Vector2Int[] direction =
    {
        new Vector2Int( 0,  1), // ��
        new Vector2Int( 0, -1), // ��
        new Vector2Int(-1,  0), // ��
        new Vector2Int( 1,  0), // ��
        new Vector2Int(-1,  1), // �»�
        new Vector2Int( 1,  1), // ���
        new Vector2Int(-1, -1), // ����
        new Vector2Int( 1, -1), // ����
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
            Debug.Log("��� Ž�� ����!");
        }
        else
        {
            Debug.Log("��� Ž�� ����!");
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
        // 0. ���� ����
        List<ASNode> openList = new List<ASNode>(); // Ž���� ������ �ĺ����� �����ϴ� ����Ʈ
        Dictionary<Vector2Int, bool> closeSet = new Dictionary<Vector2Int, bool>(); // Ž���� �Ϸ�� �������� ����
        path = new List<Vector2Int>(); // ��θ� ������ ����Ʈ

        // ó������ Ž���� ������ openList�� �߰��Ѵ�
        openList.Add(new ASNode(start, null, 0, Heuristic(start, end)));

        while(openList.Count > 0) // Ž���� ��尡 �����ִ� ���� �ݺ�
        {
            // 1. �������� Ž���� ������ �����Ѵ�
            //    F�� ���� ���� ������ �����Ѵ�
            //    F�� ���ٸ� H�� ���� ���� ������ �����Ѵ�  
            ASNode nextNode = NextNode(openList);

            // 2. ���õ� ������ Ž���ߴٰ� ǥ���ϱ� ����
            //    openList���� �����ϰ� closeSet�� �߰�
            openList.Remove(nextNode); // �������� Ž���� ������ Ž���� �ĺ��鿡�� ����
            closeSet.Add(nextNode.pos, true); // Ž���� �Ϸ��� �����¿� �߰��Ѵ�. Ž���� �ߴٴ� ǥ���� true�� ����

            // 3. �������� Ž���� ������ �������ΰ�� 
            //    ��� Ž���� ���������� path�� ��ȯ�ϰ� �����Ѵ�.
            if(nextNode.pos == end)
            {
                ASNode curNode = nextNode;
                // ������ ��θ� �����Ѵ�
                // ASNode�� ����� parent�� ���󰡼� �����Ѵ�
                while(curNode != null)
                {
                    path.Add(curNode.pos); 
                    curNode = curNode.parent;
                }

                path.Reverse(); //��θ� ������ �ùٸ� ������ �����Ѵ�
                return true;    // ��� Ž���� �����ߴٰ� true ��ȯ
            }

            // 4. �ֺ� �������� ������ ����Ѵ�
            for(int i = 0; i < direction.Length; i++)
            {
                Vector2Int pos = nextNode.pos + direction[i]; // �� ����鿡 ���� ��ġ ���

                // Ž���ϸ� �ȵǴ� ���� �����Ѵ�.
                // 4-1. �̹� Ž���� ������ ���
                if(closeSet.ContainsKey(pos))
                    continue;
                // 4-2. ���� ���ϴ� �����ϰ��
                // tilemap.HasTile : Ÿ�ϸ��� �м��ϴ� ���
                // Physics.Overlap : �浹ü�� ���� ���θ� Ȯ���ϴ� ���
                // Physics.Raycast : �߰��� ��ֹ��� ������ ����ĳ��Ʈ�� Ȯ���ϴ� ���
                if(Physics2D.OverlapCircle(pos,0.2f) != null)
                    continue;
                // 4-3 �밢�� ������ ó��
                // 4-3-a. �밢�� �� �� ���� ��� ���� ���
                if (
                    i>=4 && // i�� 4 �̻��ϰ�� �밢�� �������� �� ����迭�� ���ǵ� 
                    Physics2D.OverlapCircle(new Vector2(pos.x,nextNode.pos.y), radius) != null
                    &&
                    Physics2D.OverlapCircle(new Vector2(nextNode.pos.x, pos.y), radius) != null
                    )
                {
                    continue;
                }
                // 4-3-b. �밢�� �߿����� �� �� �շ� �־�� ���
                if (
                    i >= 4 && // i�� 4 �̻��ϰ�� �밢�� �������� �� ����迭�� ���ǵ� 
                    Physics2D.OverlapCircle(new Vector2(pos.x, nextNode.pos.y), radius) != null
                    ||
                    Physics2D.OverlapCircle(new Vector2(nextNode.pos.x, pos.y), radius) != null
                    )
                {
                    continue;
                }

                // 4-4 ���� ���
                int g; // �̵� ���
                // �������� ������ ���
                if(pos.x == nextNode.pos.x || pos.y == nextNode.pos.y )
                {
                    g = nextNode.g + CostStraight;
                }
                // �밢������ ������ ��� 
                else
                {
                    g = nextNode.g + CostDiagonal;
                }
                int h = Heuristic(pos, end);
                int f = g + h;

                // 4-5 ������ ���� ������ �ʿ��� ���
                ASNode findNode = FindNode(openList, pos); // openList ���� �� ��ġ�� �ش��ϴ� ��带 ã�� ����
                // ������ ������ ��� ������ ������
                if(findNode == null) 
                {
                    openList.Add(new ASNode(pos, nextNode, g, h)); // ��尡 �����ٸ� ���� �߰��Ѵ�
                }
                // ���� ��庸�� f���� ���ٸ� �����Ѵ�
                else if(findNode.f > f)
                {
                    findNode.f = f;
                    findNode.g = g;
                    findNode.h = h;
                    findNode.parent = nextNode;
                }
            }
        }

        // ã�� ������ ���
        path = null;
        return false;
    }

    // �޸���ƽ : �ֻ��� ��θ� �����ϴ� ������
    //           �޸���ƽ�� ���� ���Ž���� ȿ���� ������
    public static int Heuristic(Vector2Int start, Vector2Int end)
    {
        int xSize = Mathf.Abs(start.x - end.x); // x���� �Ÿ� ���
        int ySize = Mathf.Abs(start.y - end.y); // y���� �Ÿ� ���

        // ����ư �Ÿ� : ������ ���� �̵��ϴ� �Ÿ�
        // return xSize + ySize;

        // ��Ŭ���� �Ÿ� : �밢���� ���� �̵��ϴ� �Ÿ�
        // return (int)Vector2Int.Distance(start, end);

        // Ÿ�ϸ� �Ÿ� : ������ �밢���� ���� �̵��ϴ� �Ÿ�
        int straightCount = Mathf.Abs(xSize - ySize); // ���� �Ÿ�
        int diagonalCount = Mathf.Max(xSize,ySize) - straightCount; // �밢�� �Ÿ�
        return CostStraight * straightCount + CostDiagonal * diagonalCount; // �� ��� ��ȯ
    }

    public static ASNode NextNode(List<ASNode> openList)
    {
        // F�� ���� ���� ������ ����, ���ٸ� H�� ���� ���� ������ �����Ѵ�
        int curF = int.MaxValue; // �ּ� F���� ã�� ���� �ʱⰪ�� MaxValue�� ����
        int curH = int.MinValue; // �ּ� H���� ã�� ���� �ʱⰪ�� MaxValue�� ����
        ASNode minNode = null;   // �ּ� F���� ���� ��带 ����

        for(int i = 0; i < openList.Count; i++) //openList���� �ּ� F���� ���� ��带 Ž��
        {
            // Ž���� ����� ���� ����� f������ �� ���� f���� ã���� ��
            // �ش� ����� f,h������ �����ϰ� minNode�� ��带 ����
            if (curF > openList[i].f) 
            {
                curF = openList[i].f;
                curH = openList[i].h;
                minNode = openList[i];
            }
            // Ž���� ��尡 ���� ����� F���� F���� ���� H���� �������� H���� �� �������
            // �ش� ����� f,h������ �����ϰ� minNode�� ��带 ����
            else if (curF == openList[i].f && curH > openList[i].h)
            {
                // curF = openList[i].f; //f���� ������ �����ص� �����ϴ�
                curH = openList[i].h;
                minNode = openList[i];
            }
        }
        //�ݺ����� ���� ã�� ��带 ��ȯ
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
