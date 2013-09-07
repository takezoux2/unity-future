using UnityEngine;
using System.Collections;
using GeishaTokyo.Concurrent.Factory;
using System.Threading;

public class Fibonacci : MonoBehaviour
{
	string message = "Calculating Fibonacci(20)";
	
	string message2 = "Waiting heavy method";
	
	// Use this for initialization
	void Start ()
	{
		var f = AsyncFactory.Start<long>(() => {
			return CalcFibonacci(40);
		});
		StartCoroutine(f.CoComplete( r => {
			message = "Answer = " + r.Result;
		}));
		
		var f2 = AsyncFactory.Start<string>(() => {
			return HeavyMethod();
		});
		StartCoroutine(f2.CoComplete(r => {
			message2 = r.Result;
		}));
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void OnGUI(){
		GUILayout.BeginVertical();
		GUILayout.Label(message);
		GUILayout.Label(message2);
		GUILayout.EndVertical();
	}
	
	long CalcFibonacci(int n){
		
		if(n <= 1) return 1;
		else {
			return CalcFibonacci(n - 1) + CalcFibonacci(n-2);
		}
	}
	
	/// <summary>
	/// Simulate heavy method.
	/// </summary>
	/// <returns>
	/// The method.
	/// </returns>
	string HeavyMethod(){
		for(int i = 0;i < 5;i++)
		{
			Thread.Sleep(System.TimeSpan.FromSeconds(1));
			Debug.Log("Processing");
		}
		return "done";
	}
	
}

