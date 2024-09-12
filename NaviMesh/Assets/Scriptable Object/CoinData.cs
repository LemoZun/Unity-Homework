using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName ="Coin")]
public class CoinData : ScriptableObject
{
    [SerializeField] int score;
    public int Score { get { return score; } }

    public Color color;

    public Vector3 objectSize;


}
