
using System;
using System.Collections;


namespace GeishaTokyo.Concurrent{

	public interface Future<T> : Result<T> {
		/// <summary>
		/// Check future received a result or error.
		/// </summary>
		/// <value>
		/// <c>true</c> already received a result or error <c>false</c> still processing.
		/// </value>
		bool Done{get;}
		
		/// <summary>
		/// Wait until receive a result.Then return a result.
		/// 
		/// <exception cref="NotAllowedOperationException">be thrown when this future receives an error.</exception>
		/// </summary>
		T Get(); 
		/// <summary>
		/// Wait until receive a result or timeout.Then return a result.
		/// 
		/// </summary>
		/// <param name='timeoutSec'>
		/// Timeout seconds.
		/// </param>
		/// <exception cref="TimeoutException">throws if timeout</exception>
		/// <exception cref="NotAllowedOperationException">throws when this future receives an error.</exception>
		/// </summary>
		T Get(float timeoutSec);
		/// <summary>
		/// Wait until receive a result.
		/// </summary>
		Future<T> Wait();
		/// <summary>
		/// Wait until receive a result or timeout.
		/// </summary>
		/// <param name='timeoutSec'>
		/// Timeout seconds.
		/// </param>
		/// <exception cref="TimeoutException">throws if timeout</exception>
		Future<T> Wait(float timeoutSec);
		
		/// <summary>
		/// Register callback which be called when complete.
		/// </summary>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		void OnComplete(Action<Result<T>> callback);
		/// <summary>
		/// Register callback which be called only when success.
		/// </summary>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		void OnSuccess(Action<T> callback);
		/// <summary>
		/// Register callback which be called only when error.
		/// </summary>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		void OnError(Action<Exception> callback);
		
		/// <summary>
		/// Map by the specified mapFunc.
		/// </summary>
		/// <param name='mapFunc'>
		/// Map func.
		/// </param>
		/// <typeparam name='To'>
		/// The 1st type parameter.
		/// </typeparam>
		Future<To> Map<To>(MapFunc<T,To> mapFunc);
		/// <summary>
		/// Recover when error occured.
		/// </summary>
		/// <param name='recoverFunc'>
		/// Recover func.
		/// </param>
		Future<T> Recover(MapFunc<Exception,T> recoverFunc);
		/// <summary>
		/// Join two futures.
		/// </summary>
		/// <param name='future'>
		/// Future.
		/// </param>
		/// <typeparam name='T2'>
		/// The 1st type parameter.
		/// </typeparam>
		Future<Tuple2<T,T2>> Join<T2>(Future<T2> future);
		
		#region For unity
		/// <summary>
		/// Wait using Coroutine.
		/// </summary>
		/// <example>
		/// StartCoroutine(future.CoWait(r => { ... }));
		/// </example>
		/// <returns>
		/// The complete.
		/// </returns>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		IEnumerator CoComplete(Action<Result<T>> callback);
		/// <summary>
		/// Cos the complete.
		/// </summary>
		/// <returns>
		/// The complete.
		/// </returns>
		/// <param name='timeoutSec'>
		/// Timeout seconds.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		IEnumerator CoComplete(float timeoutSec, Action<Result<T>> callback);
		#endregion
	}
	
	public delegate To MapFunc<From,To>(From v);
	
	
}
