using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    [SerializeField] Monster monster;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>(); //�̷��� �θ��ִ� ���� �ڵ� ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttackPoint"))
        {
            Debug.Log("���� ���� ���ݹ���");
            monster.ChangeToDeathState();
        }
    }


}
