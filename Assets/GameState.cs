using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set;}

    public Vector2 BoundsMin;
    public Vector2 BoundsMax;

    public void Awake()
    {
        GameState.Instance = this;
    }
}
