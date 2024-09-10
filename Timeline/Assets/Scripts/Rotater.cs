using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] float speed;
    private void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.Self);
    }

}
