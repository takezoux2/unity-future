using System;

using System.Threading;
using GeishaTokyo.Concurrent.Impl;

namespace GeishaTokyo.Concurrent.Factory
{
	public delegate T Function0<T>();
	public static class AsyncFactory
	{
		/// <summary>
		/// Start the function using ThreadPool.
		/// </summary>
		/// <param name='function'>
		/// Function.
		/// </param>
		/// <typeparam name='T'>
		/// Result type.
		/// </typeparam>
		public static Future<T> Start<T>(Function0<T> function){
			var f = new FutureImpl<T>();
			ThreadPool.QueueUserWorkItem( WorkerMethod<T>,
				Tuples.Tuple<FutureImpl<T>,Function0<T>>(f,function));
			
			return f;
		}
			
		static void WorkerMethod<T>(object threadContext){
			var t = (Tuple2<FutureImpl<T>,Function0<T>>)threadContext;
			try{
				var result = t.V2();
				t.V1.SetResult(result);
			}catch(Exception e){
				t.V1.SetError(e);
			}
			
		}
		
		
	}
}

