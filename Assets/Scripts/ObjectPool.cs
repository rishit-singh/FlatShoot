using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public struct DisposableGameObject
{
	public float CreationTime { get; private set; } 

	public float DisposeTime;

	public GameObject Obj { get; private set;}

	public bool ShouldDispose { get { return ((Time.time + this.DisposeTime) >= this.CreationTime); } private set {}  }

	public DisposableGameObject(GameObject obj, float creationTime, float disposeTime)
	{
		this.Obj = obj;
		this.CreationTime = creationTime;
		this.DisposeTime = disposeTime;
	}
}

public class ObjectPool
{
    private Queue<GameObject> ObjectQueue;

	private Dictionary<int, DisposableGameObject> Allocated;

	public GameObject Instance { get; private set; } 

	public Vector3 SpawnPosition { get; private set; }
	
	public int Count { get; private set; } 

	public int AutoDisposeTime { get; set; }

	private void Initialize()
	{	
		for (int x = 0;  x < this.Count; x++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(this.Instance, this.SpawnPosition, Quaternion.identity);
			
			obj.SetActive(false);

			this.ObjectQueue.Enqueue(obj);		
		}
	}

	public GameObject GetObject()
	{ 
		if (this.ObjectQueue.Count < 1)
			return null;

		GameObject obj = this.ObjectQueue.Dequeue();	

		obj.SetActive(true);
		this.Allocated.Add(obj.GetInstanceID(), new DisposableGameObject(obj, Time.time, this.AutoDisposeTime));
		
		return obj;
	}
 
	public bool Dispose(GameObject obj)
	{
		try
		{
			obj.SetActive(false);
			this.ObjectQueue.Enqueue(obj);

			this.Allocated.Remove(obj.GetInstanceID());
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	} 

    public ObjectPool(GameObject instance, int count, Vector3 spawnPosition, float autoDisposeTime = -1)
    {
		this.ObjectQueue = new Queue<GameObject>();
		this.Allocated = new Dictionary<int, DisposableGameObject>();

		this.Instance = instance;
		this.Count = count;
		this.SpawnPosition = spawnPosition;
	
		this.Initialize();
    }
}
