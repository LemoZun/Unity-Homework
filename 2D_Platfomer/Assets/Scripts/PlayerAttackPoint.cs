using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPoint : MonoBehaviour
{
    [SerializeField] float bounceForce;
    [SerializeField] Rigidbody2D playerRb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Weakness"))
        {
            Debug.Log("�� ���� ����");
            playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }

    }
}
