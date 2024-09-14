using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PooledPlanet : MonoBehaviour
{
    [SerializeField] PlanetPool planetPool;
    [SerializeField] int phase;
    Coroutine planetLifeRoutine;
    bool hasCollided = false;

    private void Start()
    {
        GameObject planetPoolObject = GameObject.FindGameObjectWithTag("PlanetPool");
        if (planetPoolObject != null)
        {
            planetPool = planetPoolObject.GetComponent<PlanetPool>();
        }
        else
        {
            Debug.LogError("PlanetPool이 없습니다.");
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer && !hasCollided)
        {
            PooledPlanet otherPlanet = collision.gameObject.GetComponent<PooledPlanet>();
            if (otherPlanet != null && !otherPlanet.hasCollided)
            {
                hasCollided = true;
                otherPlanet.hasCollided = true;

                Vector2 midPosition = CalculateMid(gameObject.transform.position, collision.gameObject.transform.position);
                PooledPlanet nextPlanet = Instantiate(planetPool.planetPool[phase + 1], midPosition, Quaternion.identity);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
        //if (collision.gameObject.tag == tag)
        //{

        //    Vector2 midPosition = CalculateMid(gameObject.transform.position, collision.gameObject.transform.position);
        //    PooledPlanet nextPlanet = Instantiate(planetPool.planetPool[phase + 1], midPosition, Quaternion.identity);

        //    Destroy(collision.gameObject);
        //    Destroy(gameObject);

        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("GravityZone"))
        {

        }
    }



    Vector2 CalculateMid(Vector2 _planet1, Vector2 _planet2)
    {

        return (_planet1 + _planet2) / 2;
    }

    //IEnumerator PlanetLifeRoutine()
    //{
    //    WaitForSeconds delay = new WaitForSeconds(5f);
    //    yield return delay;

    //    if (!isCollide)
    //    {
    //        gameObject.SetActive(false);
    //        yield return null;
    //    }
    //}
}
