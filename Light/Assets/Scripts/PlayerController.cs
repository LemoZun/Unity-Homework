using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform camTransform;
    [SerializeField] CinemachineVirtualCamera zoomCam;
    [SerializeField] CinemachineVirtualCamera fpsCam;
    private bool zoomMode;

    private void Awake()
    {
        zoomMode = false;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        if(camTransform == null )
            camTransform = Camera.main.transform;

    }

    private void Update()
    {
        Move();
        Rotate();
        ChangeZoomInZoomOut();
    }


    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3 (x, 0, z);
        if (dir.sqrMagnitude > 1)
        {
            dir.Normalize();
        }
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, camTransform.rotation.eulerAngles.y, 0);
    }

    private void ChangeZoomInZoomOut()
    {

        if(Input.GetMouseButtonDown(1) && zoomMode == false)
        {
            zoomMode = true;
            zoomCam.Priority = 15;
        }
        
        if(Input.GetMouseButtonUp(1) && zoomMode == true)
        {
            zoomMode = false;
            zoomCam.Priority = 0;
        }
    }

}
