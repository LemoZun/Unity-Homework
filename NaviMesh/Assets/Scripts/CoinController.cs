using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public ScoreManager scoreManager;


    private void Start()
    {
        GameObject scoreManagerObject = GameObject.FindWithTag("ScoreManager");
        if(scoreManagerObject != null)
            Debug.Log($"스코어 매니저 태그 찾음 {scoreManagerObject.name}");

        scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        if (scoreManager == null)
            Debug.LogError("스코어 매니저 참조 불가");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거됨");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어와 트리거됨");
            scoreManager.AddScore();
            Destroy(gameObject);
        }
    }
}
