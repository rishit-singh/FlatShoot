using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D RigidBody;

    public Transform Transform;

    public Animator Animator;

    public SpriteRenderer Renderer;

    public GameObject Square;

    public GameObject PlayerInstance;

    private Player Player;

    protected static string[] Keys = new string[] { 
        "w", "a", "s", "d", "space" 
    };

    /// <summary>
    /// Clamps the position of the current RigidBody if it goes above/below a certain X value
    /// </summary>
    /// <param name="min">Minimum X value.</param>
    /// <param name="max">Maximum X value.</param>
    protected void ClampX(float min, float max)
    {
        if (this.RigidBody.position.x < min)
            this.RigidBody.position = new Vector2(min, this.RigidBody.position.y);
        else if (this.RigidBody.position.x > max)
            this.RigidBody.position = new Vector2(max, this.RigidBody.position.y);
    }

    /// <summary>
    /// Clamps the position of the current RigidBody if it goes above/below a certain Y value
    /// </summary>
    /// <param name="min">Minimum Y value.</param>
    /// <param name="max">Maximum Y value.</param>
    protected void ClampY(float min, float max)
    {
        if (this.RigidBody.position.y < min)
            this.RigidBody.position = new Vector2(this.RigidBody.position.x, min);
        else if (this.RigidBody.position.y > max) 
            this.RigidBody.position = new Vector2(this.RigidBody.position.x, max); 
    }
    
    public void Start()
    {
        this.Player = new Player(this.PlayerInstance, 100); 
    }

    public void Update()
    {
        for (int x = 0; x < GameState.Instance.MovementForces.Length; x++)
            if (Input.GetKey(Keys[x]))
                this.Player.Move(GameState.Instance.MovementForces[x]);

        this.Player.Obj.transform.position = Util.ClampVectorX(this.Player.Obj.transform.position, GameState.Instance.BoundsMin.x, GameState.Instance.BoundsMax.x);
        this.Player.Obj.transform.position = Util.ClampVectorY(this.Player.Obj.transform.position, GameState.Instance.BoundsMin.y, GameState.Instance.BoundsMax.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision called.");
    }
}

 