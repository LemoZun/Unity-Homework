using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASNode
{
    public Vector2Int pos;  // ���� ������ ��ġ
    public ASNode parent;   // ���� ������ Ž���� ������ �θ�� ��
    public int f;           // ���� ���� �Ÿ� : f = g + h
    public int g;           // �ɸ� �Ÿ�
    public int h;           // ����Ǵ� ���� �Ÿ�(�޸���ƽ)

    public ASNode(Vector2Int pos, ASNode parent, int g, int h)
    {
        this.pos = pos;
        this.parent = parent;
        this.f = g + h;
        this.g = g;
        this.h = h;
    }
}
