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

    public ObjectPool SquarePool;

    public Stack<GameObject> Allocated;

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
        this.SquarePool = new ObjectPool(this.Square, 64, new Vector3(0, 0, 0));
        this.Allocated = new Stack<GameObject>();

        Debug.Log("Pool created.");
    }

    public void Update()
    {
        for (int x = 0; x < GameState.Instance.MovementForces.Length; x++)
            if (Input.GetKey(Keys[x]))
                this.RigidBody.AddForce(GameState.Instance.MovementForces[x]);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = this.SquarePool.GetObject();

            if (obj != null)
            {
                Rigidbody2D rigidBody;

                if ((rigidBody = obj.GetComponent<Rigidbody2D>()) != null)
                   rigidBody.gravityScale = 1;

                this.Allocated.Push(obj);
            }
        } 

        if (Input.GetMouseButtonDown(1) && this.Allocated.Count > 0)
            this.SquarePool.Dispose(this.Allocated.Pop());

        this.ClampX(GameState.Instance.BoundsMin.x, GameState.Instance.BoundsMax.x);
        this.ClampY(GameState.Instance.BoundsMin.y, GameState.Instance.BoundsMax.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision called.");
    }
}

 