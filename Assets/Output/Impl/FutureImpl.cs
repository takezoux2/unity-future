using System;

using GeishaTokyo.Concurrent;
using UnityEngine;
using System.Collections.Generic;

namespace GeishaTokyo.Concurrent.Impl
{
	public class FutureImpl<T> : Future<T>,FutureCall<T>
	{
		public static float DefaultTimeoutSec = 10.0f;
		public static int WaitSleepMilliSec = 100;
		
		T result;
		Exception error;
		volatile bool done;
		volatile bool success;
		
		
		public FutureImpl ()
		{
			done = false;
			success = false;
		}
		
		public void SetResult (T v)
		{
			if(done) throw new NotAllowedOperationException("Can't set result after done");
			else{
				result = v;
				success = true;
				done = true;
				FireComplete();
			}
		}

		public void SetError (Exception e)
		{
			if(done) throw new NotAllowedOperationException("Can't set result after done");
			else{
				error = e;
				success = false;
				done = true;
				FireComplete();
			}
		}



		public T Result {
			get {
				if(!done){
					throw new NotAllowedOperationException("Can't get result before done");
				}
				if(!success){
					throw new NotAllowedOperationException("Can't get result from not succeeded future");
				}
				return result;
			}
		}

		public Exception Error {
			get {
				
				if(!done){
					throw new NotAllowedOperationException("Can't get error before done");
				}
				if(success){
					throw new NotAllowedOperationException("Can't get result from succeeded future");
				}
				return error;
			}
		}

		public bool Done {
			get {
				return done;
			}
		}
		
		public bool Success {
			get {
				return done && success;
			}
		}
		
		
		

		public bool HasError {
			get {
				return done && !success;
			}
		}
		public Future<T> Wait ()
		{
			while(!done){
				System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(WaitSleepMilliSec));
			}
			return this;
		}
		
		public Future<T> Wait (float timeoutSec)
		{
			DateTime start = DateTime.Now;
			TimeSpan sleep = TimeSpan.FromMilliseconds(WaitSleepMilliSec);
			while(!done && (DateTime.Now - start).TotalSeconds < timeoutSec){
				System.Threading.Thread.Sleep(sleep);
			}
			if(!done){
				throw new TimeoutException("Wait timeout");
			}else{
				return this;
			}
		}
		
		

		public T Get ()
		{
			Wait();
			if(result == null){
				throw new NotAllowedOperationException("Can't get error from failed future");
			}else{
				return result;
			}
		}

		public T Get (float timeoutSec)
		{
			Wait(timeoutSec);
			if(result == null){
				throw new NotAllowedOperationException("Can't get error from failed future");
			}else{
				return result;
			}
		}
		
		void FireComplete(){
			foreach(var callback in completeCallbacks){
				callback(this);
			}
		}
		
		List<Action<Result<T>>> completeCallbacks = new List<Action<Result<T>>>();

		public void OnComplete (Action<Result<T>> callback)
		{
			if(this.done){
				callback(this);
			}else{
				completeCallbacks.Add(callback);
			}
		}
		
		
		public void OnSuccess (Action<T> callback)
		{
			OnComplete( obj => {
				if(this.Success){
					callback(this.result);
				}
			});
		}

		public void OnError (Action<Exception> callback)
		{
			OnComplete(obj => {
				if(this.HasError){
					callback(this.error);
				}
			});
		}

		public Future<To> Map<To> (MapFunc<T, To> mapFunc)
		{
			var f = new FutureImpl<To>();
			OnComplete (obj => {
				if(obj.Success){
					f.SetResult(mapFunc(result));
				}else{
					f.SetError(error);
				}
			});
			return f;
		}

		public Future<T> Recover (MapFunc<Exception, T> recoverFunc)
		{
			var f = new FutureImpl<T>();
			OnComplete(obj => {
				if(obj.Success){
					f.SetResult(result);
				}else{
					f.SetResult(recoverFunc(error));
				}
			});
			return f;
		}
		
		public Future<Tuple2<T, T2>> Join<T2> (Future<T2> future)
		{
			var f = new FutureImpl<Tuple2<T, T2>>();
			this.OnComplete( f1 => {
				if(future.Done){
					if(future.Success && this.Success){
						f.SetResult(Tuples.Tuple(this.result,future.Get()));
					}else if(future.Success && this.HasError){
						f.SetError(this.error);
					}else if(future.HasError && this.Success){
						f.SetError(this.error);
					}else{
						f.SetError(new MultiException(this.error,future.Error));
					}
				}	
			});
			future.OnComplete(f2 => {
				if(this.Done){
					if(future.Success && this.Success){
						f.SetResult(Tuples.Tuple(this.result,future.Get()));
					}else if(future.Success && this.HasError){
						f.SetError(this.error);
					}else if(future.HasError && this.Success){
						f.SetError(this.error);
					}else{
						f.SetError(new MultiException(this.error,future.Error));
					}
				}	
			});
			
			return f;
		}
		
		
		
		public System.Collections.IEnumerator CoComplete (Action<Result<T>> callback)
		{
			while(!done){
				yield return null;
			}
			callback(this);
			
		}
		
		public System.Collections.IEnumerator CoComplete (float timeoutSec, Action<Result<T>> callback)
		{
			
			DateTime start = DateTime.Now;
			while(!done && (DateTime.Now - start).TotalSeconds < timeoutSec){
				yield return null;
			}
			if(!done){
				callback(new ErrorResult<T>(new TimeoutException("Time out on CoComplete")));
			}else{
				callback(this);
			}
		}
		
	}
}

