using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] TextMeshProUGUI scoreText;


    private void Awake()
    {
        scoreText.text = "Scroe : 0";
        scoreManager.onScroeChanged += UpdateScroe;
    }
    public void UpdateScroe()
    {

        scoreText.text = $"Scroe : {scoreManager.Score}";
    }
}
