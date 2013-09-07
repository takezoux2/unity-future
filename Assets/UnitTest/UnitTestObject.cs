using UnityEngine;
using System.Collections;
using GeishaTokyo.Concurrent;
using SharpUnit;

public class UnitTestObject : MonoBehaviour
{
	
	Future<string> future;
	bool started = false;
	public Future<string> Future{
		set{
			this.future = value;
			if(started){
				CallFuture();
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		started = true;
		if(future != null){
			CallFuture();
		}
	}
	
	void CallFuture()
	{
		StartCoroutine(future.CoComplete( r => {
			Debug.Log ("Coroutine is called!");
			// to check to be able to call Unity methods.
			GameObject.Find("TestRunner");
			Assert.True(r.Success);
		}));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

