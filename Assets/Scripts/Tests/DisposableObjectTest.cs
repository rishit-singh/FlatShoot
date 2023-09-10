using System.Collections.Generic;
using UnityEngine;



public class DisposableGameObjectTest : MonoBehaviour
{
    private Stack<DisposableGameObject> DisposableSquares; 

    public GameObject Instance; 

    public Pool ObjectPool;

    public void Start()
    {
        DisposableSquare square = new DisposableSquare(this.Instance, Time.time, 5.0f);
        
        square.Obj.GetComponent<SpriteRenderer>().color = Color.green;

        this.ObjectPool = new Pool(square.Obj, 10, new Vector2(0, 0), 5);
    }

    public void Update()
    {
        DisposableGameObject gameObject = this.ObjectPool.GetObject(new Vector2(-5, -5));

        for (; !gameObject.IsNull; gameObject = this.ObjectPool.GetObject(new Vector2(-10, -10)));

        this.ObjectPool.UpdateDisposer(); // has to be called per frame to ensure autodispsol
    }
}

public class DisposableSquare : DisposableGameObject 
{
    private SpriteRenderer Sprite;

    /// <summary>
    /// Called by the pool on disposal.
    /// </summary>
    public override void Reset()
    {
    }

    /// <summary>
    /// Called by the pool before after instanciating.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        this.Sprite = this.Obj.GetComponent<SpriteRenderer>();
        
        this.Reset();
        
        this.Sprite.color = Color.green;
    }

    public DisposableSquare(GameObject instance, float creationTime, float disposeTime) 
        : base(instance, creationTime, disposeTime)
    {
    }
}
