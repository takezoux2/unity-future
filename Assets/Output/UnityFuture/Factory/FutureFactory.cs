using System;

using GeishaTokyo.Concurrent.Impl;

namespace GeishaTokyo.Concurrent.Factory
{
	public static class FutureFactory
	{
		
		public static FutureCall<T> NewFuture<T>(){
			return new FutureImpl<T>();
		}
		
		public static Future<T> Success<T>(T result){
			var f = new FutureImpl<T>();
			f.SetResult(result);
			return f;
		}
		
		public static Future<T> Error<T>(Exception e){
			var f = new FutureImpl<T>();
			f.SetError(e);
			return f;
		}
		
		
	}
}

