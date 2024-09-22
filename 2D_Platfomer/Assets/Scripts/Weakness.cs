using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    [SerializeField] Monster monster;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>(); //이러면 부모에있는 몬스터 자동 참조
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttackPoint"))
        {
            Debug.Log("몬스터 약점 공격받음");
            monster.ChangeToDeathState();
        }
    }


}
