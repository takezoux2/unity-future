
using System;
using System.Collections;


namespace GeishaTokyo.Concurrent{

	public interface Future<T> : Result<T> {
		
		bool Done{get;}
		
		T Get(); // = Result
		T Get(float timeoutSec);
		
		Future<T> Wait();
		Future<T> Wait(float timeoutSec);
		
		void OnComplete(Action<Result<T>> callback);
		void OnSuccess(Action<T> callback);
		void OnError(Action<Exception> callback);
		
		
		Future<To> Map<To>(MapFunc<T,To> mapFunc);
		Future<T> Recover(MapFunc<Exception,T> recoverFunc);
		
		Future<Tuple2<T,T2>> Join<T2>(Future<T2> future);
		
		#region For unity
		IEnumerator CoComplete(Action<Result<T>> callback);
		IEnumerator CoComplete(float timeoutSec, Action<Result<T>> callback);
		#endregion
	}
	
	public delegate To MapFunc<From,To>(From v);
	
	
}
