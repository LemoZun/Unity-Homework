using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{

    private Camera mainCam;
    [SerializeField] LayerMask groundMask;
    [SerializeField] NavMeshAgent playerAgent;

    Coroutine rayRoutine;


    private void Awake()
    {
        playerAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        mainCam = Camera.main;

        //if(rayRoutine == null)
        //{
        //    rayRoutine = StartCoroutine(RayRoutine());
        //}

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("클릭됨");
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundMask))
            {
                CheckNavi(hit);
            }
        }
    }
    

    IEnumerator RayRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while(true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("클릭됨");
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f, groundMask))
                {
                    CheckNavi(hit);
                }
            }
            yield return delay;

        }

    }
    private void CheckNavi(RaycastHit _hit)
    {
        if (NavMesh.SamplePosition(_hit.point, out NavMeshHit navhit, 0.1f, NavMesh.AllAreas)) // AllAreas : 네비메시의 모든 영역을 포함
        {
            playerAgent.SetDestination(navhit.position);
        }
    }

}
