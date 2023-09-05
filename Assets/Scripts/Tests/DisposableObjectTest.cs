using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class DisposableGameObjectTest : MonoBehaviour
{
    private Stack<DisposableGameObject> DisposableSquares; 

    public GameObject Instance; 

    public Pool ObjectPool;

    public void Start()
    {
        this.ObjectPool = new Pool(this.Instance, 10, new Vector2(0, 0), 5);
    }

    public void Update()
    {
        DisposableGameObject gameObject = this.ObjectPool.GetObject();

        for (; !gameObject.IsNull; gameObject = this.ObjectPool.GetObject());

        this.ObjectPool.UpdateDisposer();
    }
}

public class DisposableSquare : DisposableGameObject    
{
    private SpriteRenderer Sprite;

    public override void Reset()
    {
        this.Sprite.color = Color.cyan;
    }

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