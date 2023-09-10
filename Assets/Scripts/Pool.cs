using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Maintains a disposable state based on creation and dispose time.
/// </summary>
public class DisposableGameObject
{
	public int ID { get; private set; }

	public float CreationTime { get; private set; } 

	public float DisposeTime { get; set; } 

	public GameObject Obj { get; protected set;}

	private Vector2 _Position;

	public bool IsNull 
	{ 
		get 
		{ 
			return (this.Obj == null &&
					this.CreationTime < 0 &&
					this.DisposeTime < 0); 
		} 
		private set {} 
	}


	public bool ShouldDispose 
	{ 
		get 
		{ 
			return (Time.time >= (this.CreationTime + this.DisposeTime) && 
					this.DisposeTime >= 0); 
		} 

		private set{}
	}

	public Vector2 Position 
	{  
		get => _Position; 
		set 
		{ 
			this._Position = value;
			this.Obj.GetComponent<Transform>().position = this._Position; 
		} 
	}

	/// <summary>
	/// Called on initialization.
	/// </summary>
	public virtual void Initialize()
	{
		this.Obj.SetActive(true);
	}

	/// <summary>
	/// Called on Reset
	/// </summary>
	public virtual void Reset()
	{
		this.Obj.SetActive(false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="creationTime"></param>
	/// <param name="disposeTime"></param>
	public DisposableGameObject(GameObject obj, float creationTime, float disposeTime)
	{
		this.Obj = obj;
		this.CreationTime = creationTime;
		this.DisposeTime = disposeTime;
	
		if (this.Obj == null)
			this.ID = -1;
		else
			this.ID = this.Obj.GetInstanceID();
	}
}

/// <summary>
/// Manages a pool of Objects
/// </summary>
public class Pool
{
    private Queue<DisposableGameObject> ObjectQueue;

	private Dictionary<int, DisposableGameObject> Allocated;

	public GameObject Instance { get; private set; } 

	public Vector2 SpawnPosition { get; private set; }
	
	public int Count { get; private set; } 

	public float AutoDisposeTime { get; set; }
	
	/// <summary>
	/// Allocates all the objects. 
	/// </summary>
	private void Initialize()
	{	
		for (int x = 0;  x < this.Count; x++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(this.Instance, this.SpawnPosition, Quaternion.identity);
			
			obj.SetActive(false);

			DisposableGameObject disposableObj = new DisposableGameObject(obj, Time.time, this.AutoDisposeTime); 
	
			this.ObjectQueue.Enqueue(disposableObj);		
		}
	}

	/// <summary>
	/// Fetches an object from the pool queue
	/// </summary>
	/// <returns>Fetched object.</returns>
	public DisposableGameObject GetObject()
	{ 
		if (this.ObjectQueue.Count < 1)
			return new DisposableGameObject(null , -1, -1);
	
		DisposableGameObject obj = this.ObjectQueue.Dequeue();

		obj.Initialize();

		this.Allocated.Add(obj.ID, obj);

		return obj;
	} 

	/// <summary>
	/// Fetches an object from the pool and sets its position to the given Vector2
	/// </summary>
	/// <param name="spawnPosition">Position to set.</param>
	/// <returns>Fetched object.</returns>
	public DisposableGameObject GetObject(Vector2 spawnPosition)
	{
		if (this.ObjectQueue.Count < 1)
			return new DisposableGameObject(null , -1, -1);
	
		DisposableGameObject obj = this.ObjectQueue.Dequeue();

		obj.Position = spawnPosition;
		
		obj.Initialize();

		this.Allocated.Add(obj.ID, obj);

		return obj;
	}
	
	public bool Dispose(DisposableGameObject obj)
	{
		try
		{
			obj.Reset();
			this.ObjectQueue.Enqueue(obj);
			this.Allocated.Remove(obj.ID);
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Disposes all the objects that have a disposable state.
	/// </summary>
	public void UpdateDisposer()
	{
		DisposableGameObject[] gameObjects = new DisposableGameObject[this.Allocated.Count];

		this.Allocated.Values.CopyTo(gameObjects, 0);

		foreach (DisposableGameObject gameObject in gameObjects)
			if(gameObject.ShouldDispose)
				this.Dispose(gameObject);	
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="instance">GameObject instance.</param>
	/// <param name="count">Number of objects to allocate.</param>
	/// <param name="spawnPosition">Default spawn position</param>
	/// <param name="autoDisposeTime">Auto dispose time.</param>
    public Pool(GameObject instance, int count, Vector3 spawnPosition, float autoDisposeTime = -1)
    {
		this.ObjectQueue = new Queue<DisposableGameObject>();
		this.Allocated = new Dictionary<int, DisposableGameObject>();

		this.Instance = instance;
		this.Count = count;
		this.SpawnPosition = spawnPosition;
		this.AutoDisposeTime = autoDisposeTime;

		this.Initialize();
	}
	
	~Pool()
	{
		DisposableGameObject[] gameObjects = new DisposableGameObject[this.Allocated.Count];

		this.Allocated.Values.CopyTo(gameObjects, 0);

		foreach (DisposableGameObject gameObject in gameObjects)
			UnityEngine.Object.Destroy(gameObject.Obj);

		for (GameObject obj = this.ObjectQueue.Dequeue().Obj; this.ObjectQueue.Count > 0; obj = this.ObjectQueue.Dequeue().Obj)
			UnityEngine.Object.Destroy(obj);
	}
}

