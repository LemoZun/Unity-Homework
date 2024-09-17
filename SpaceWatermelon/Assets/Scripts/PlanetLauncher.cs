using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLauncher : MonoBehaviour
{
    [SerializeField] float launchFore;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject planetPrefab;
    [SerializeField] Transform launchPosition;
    [SerializeField] GameObject launchChecker;
    Coroutine launchRoutine;
    bool launchPossible = true;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if(launchRoutine == null)
            launchRoutine = StartCoroutine(LaunchRoutine());
        launchChecker.SetActive(true);
    }

    private void OnDisable()
    {
        if(launchRoutine != null)
            StopCoroutine(launchRoutine);
    }
    private void Update()
    {
        float x = Input.GetAxis("Mouse X");
        transform.Rotate(0,0,-x*rotateSpeed*Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && launchPossible)
        {
            LaunchPlanet();
        }
        //if(Input.GetMouseButtonDown(0))
        //{
        //    LaunchPlanet();
        //}
    }

    private void LaunchPlanet()
    {
        Vector2 dir = transform.up;
        GameObject planet = Instantiate(planetPrefab, launchPosition.position,Quaternion.identity);
        Rigidbody2D rb = planet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * launchFore, ForceMode2D.Impulse);
        launchPossible = false;
        launchChecker.SetActive(false);
        Debug.Log("Charging");
    }

    IEnumerator LaunchRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(2f);

        while (true)
        {
            yield return delay;

            if(!launchPossible)
            {
                launchPossible = true;
                launchChecker.SetActive(true);
                Debug.Log("Charging Complete");
            }
        }



    }
}
