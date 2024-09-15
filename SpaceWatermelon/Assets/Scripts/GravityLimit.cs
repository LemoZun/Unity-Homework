using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GravityLimit : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float orbitForce;
    private bool inGravityZone = false;
    private bool OnEarth = false;
    [SerializeField] Rigidbody2D rb;
    private Transform centerOfGravity;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerOfGravity = GameObject.FindWithTag("GravityZone").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!OnEarth && collision.CompareTag("GravityZone"))
        {
            inGravityZone = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Earth"))
        {
            OnEarth = true; 
        }
    }



    private void FixedUpdate()
    {
        if(inGravityZone)
        {
            if(!OnEarth) 
                AddOrbitForce();
            
            LimitSpeed();


        }

    }

    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void AddOrbitForce()
    {
        Vector2 directionToCenter = centerOfGravity.position - transform.position;
        float distanceToCenter = directionToCenter.magnitude;

        
        float orbitStrength = orbitForce / distanceToCenter;

        // �߷� �߽ɿ� ���� ������ ����
        Vector2 perpendicular = Vector2.Perpendicular(directionToCenter).normalized;
        // ���� �������� �˵� ���� �� �߰�
        rb.AddForce(perpendicular * orbitForce);   //������� ���浵 �������
    }

}
