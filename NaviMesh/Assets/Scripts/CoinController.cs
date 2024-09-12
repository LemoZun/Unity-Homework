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
            Debug.Log($"���ھ� �Ŵ��� �±� ã�� {scoreManagerObject.name}");

        scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        if (scoreManager == null)
            Debug.LogError("���ھ� �Ŵ��� ���� �Ұ�");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ʈ���ŵ�");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� Ʈ���ŵ�");
            scoreManager.AddScore();
            Destroy(gameObject);
        }
    }
}
