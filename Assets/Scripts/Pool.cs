using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class DisposableGameObject
{
	public int ID { get; private set; }

	public float CreationTime { get; private set; } 

	public float DisposeTime { get; set; } 

	public GameObject Obj { get; private set;}

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

		private set {}
	}

	public virtual void Initialize()
	{
		this.Obj.SetActive(false);
	}

	public virtual void Reset()
	{
		this.Obj.SetActive(false);
	}

	public DisposableGameObject(GameObject obj, float creationTime, float disposeTime)
	{
		this.Obj = obj;
		this.CreationTime = creationTime;
		this.DisposeTime = disposeTime;
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

	private void Initialize()
	{	
		for (int x = 0;  x < this.Count; x++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(this.Instance, this.SpawnPosition, Quaternion.identity);
			
			obj.SetActive(false);

			DisposableGameObject disposableObj = new DisposableGameObject(obj, Time.time, this.AutoDisposeTime); 

			disposableObj.Initialize();
	
			this.ObjectQueue.Enqueue(disposableObj);		
		}
	}

	public DisposableGameObject GetObject()
	{ 
		if (this.ObjectQueue.Count < 1)
			return new DisposableGameObject(null , -1, -1);
	
		DisposableGameObject obj = this.ObjectQueue.Dequeue();

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

	public void UpdateDisposer()
	{
		DisposableGameObject[] gameObjects = new DisposableGameObject[this.Allocated.Count];

		this.Allocated.Values.CopyTo(gameObjects, 0);

		foreach (DisposableGameObject gameObject in gameObjects)
			if(gameObject.ShouldDispose)
				this.Dispose(gameObject);	
	}

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
			Destroy(gameObject.Obj);

		for (GameObject obj = this.ObjectQueue.Dequeue().Obj; this.ObjectQueue.Count > 0; obj = this.ObjectQueue.Dequeue().Obj)
			Destroy(obj);
	}
}

