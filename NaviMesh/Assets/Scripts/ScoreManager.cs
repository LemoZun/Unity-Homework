using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Action onScroeChanged;
    [SerializeField] int score;
    public int Score {  get { return score; } }
    

    private void Awake()
    {
        score = 0;

    }
    public void AddScore()
    {
        score += 10;
        onScroeChanged?.Invoke();
        Debug.Log($"현재 점수 : {score}");
    }
}
