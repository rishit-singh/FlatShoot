using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public struct DisposableGameObject
{
	public int ID { get; private set; }

	public float CreationTime { get; private set; } 

	public float DisposeTime;

	public GameObject Obj { get; private set;}

	public bool ShouldDispose 
	{ 
		get { return (Time.time >= (this.CreationTime + this.DisposeTime) && this.DisposeTime >= 0); } 

		private set {}
	}
 
	public DisposableGameObject(GameObject obj, float creationTime, float disposeTime)
	{
		this.Obj = obj;
		this.CreationTime = creationTime;
		this.DisposeTime = disposeTime;
		this.ID = this.Obj.GetInstanceID();
	}
}


public class PoolObject
{
	public DisposableGameObject Obj;

	public virtual void Reset()
	{}

	public PoolObject(DisposableGameObject obj) 
	{
		this.Obj = obj; 
	} 
}

/// <summary>
/// Manages a pool of Objects
/// </summary>
public class ObjectPool
{
    private Queue<PoolObject> ObjectQueue;

	private Dictionary<int, PoolObject> Allocated;

	public GameObject Instance { get; private set; } 

	public Vector3 SpawnPosition { get; private set; }
	
	public int Count { get; private set; } 

	public float AutoDisposeTime { get; set; }

	private void Initialize()
	{	
		for (int x = 0;  x < this.Count; x++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(this.Instance, this.SpawnPosition, Quaternion.identity);
			
			obj.SetActive(false);

			this.ObjectQueue.Enqueue(new PoolObject(new DisposableGameObject(obj, Time.time, this.AutoDisposeTime)));		
		}
	}

	public PoolObject GetObject()
	{ 
		if (this.ObjectQueue.Count < 1)
			return new PoolObject(new DisposableGameObject());

		GameObject obj = this.ObjectQueue.Dequeue().Obj.Obj;	

		PoolObject poolObject;

		obj.SetActive(true);
	
		this.Allocated.Add(obj.GetInstanceID(), 
							(poolObject = new PoolObject(new DisposableGameObject(obj, Time.time, this.AutoDisposeTime))));
		
		return poolObject;
	}
 
	public bool Dispose(PoolObject obj)
	{
		try
		{
			obj.Reset();
			this.ObjectQueue.Enqueue(obj);
			this.Allocated.Remove(obj.Obj.ID);
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	}

	public void UpdateDisposer()
	{
		PoolObject[] gameObjects = new PoolObject[this.Allocated.Count];

		this.Allocated.Values.CopyTo(gameObjects, 0);

		foreach (PoolObject gameObject in gameObjects)
			if(gameObject.Obj.ShouldDispose)
				this.Dispose(gameObject);	
	}

    public ObjectPool(GameObject instance, int count, Vector3 spawnPosition, float autoDisposeTime = -1)
    {
		this.ObjectQueue = new Queue<PoolObject>();
		this.Allocated = new Dictionary<int, PoolObject>();

		this.Instance = instance;
		this.Count = count;
		this.SpawnPosition = spawnPosition;
		this.AutoDisposeTime = autoDisposeTime;

		this.Initialize();
	}
}

