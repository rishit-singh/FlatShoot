using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D RigidBody;

    public Transform Transform;

    protected void ClampX(float min, float max)
    {
        if (this.RigidBody.position.x < min)
            this.RigidBody.position = new Vector2(min, this.RigidBody.position.y);
        else if (this.RigidBody.position.x > max)
            this.RigidBody.position = new Vector2(max, this.RigidBody.position.y);
    }

    protected void ClampY(float min, float max)
    {
        if (this.RigidBody.position.y < min)
            this.RigidBody.position = new Vector2(this.RigidBody.position.x, min);
        else if (this.RigidBody.position.y > max)
            this.RigidBody.position = new Vector2(this.RigidBody.position.x, max);
    }
    

    public void Start()
    {
    }

    public void Update()
    {
        if (Input.GetKey("space"))
            this.RigidBody.AddForce(new Vector2(0.0f, 5.0f));
        
        if (Input.GetKey("a"))
            this.RigidBody.AddForce(new Vector2(-3.0f, 0.0f));
        
        if (Input.GetKey("d"))
            this.RigidBody.AddForce(new Vector2(3.0f, 0.0f));
        
        if (Input.GetKey("s"))
            this.RigidBody.AddForce(new Vector2(0.0f, -3.0f));

        this.ClampX(GameState.Instance.BoundsMin.x, GameState.Instance.BoundsMax.x);
        this.ClampY(GameState.Instance.BoundsMin.y, GameState.Instance.BoundsMax.y);
    }

    public void OnCollisionEnter2D()
    {
        Debug.Log("Collision called.");


        // if (collision.gameObject.name == "Platform")
        // {
        //     Debug.Log("Collided.");
        // }
    }
}
