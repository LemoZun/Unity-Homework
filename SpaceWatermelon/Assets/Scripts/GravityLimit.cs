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

        // 중력 중심에 대해 수직인 방향
        Vector2 perpendicular = Vector2.Perpendicular(directionToCenter).normalized;
        // 수직 방향으로 궤도 유지 힘 추가
        rb.AddForce(perpendicular * orbitForce);   //포스모드 변경도 고려사항
    }

}
