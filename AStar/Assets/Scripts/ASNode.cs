using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASNode
{
    public Vector2Int pos;  // 현재 정점의 위치
    public ASNode parent;   // 현재 정점을 탐색한 정점을 부모로 둠
    public int f;           // 예상 최종 거리 : f = g + h
    public int g;           // 걸린 거리
    public int h;           // 예상되는 남은 거리(휴리스틱)

    public ASNode(Vector2Int pos, ASNode parent, int g, int h)
    {
        this.pos = pos;
        this.parent = parent;
        this.f = g + h;
        this.g = g;
        this.h = h;
    }
}
