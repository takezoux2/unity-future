using System;

namespace GeishaTokyo.Concurrent
{
	public interface FutureCall<T> : Future<T>
	{
		
		void SetResult(T v);
		void SetError(Exception e);
	}
}

