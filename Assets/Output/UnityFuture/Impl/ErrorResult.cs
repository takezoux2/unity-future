using System;

using GeishaTokyo.Concurrent;

namespace GeishaTokyo.Concurrent.Impl
{
	public class ErrorResult<T> : Result<T>
	{
		
		Exception e;
		public ErrorResult (Exception e)
		{
			this.e = e;
		}
		
		
		
		public T Result {
			get {
				throw new NotAllowedOperationException("This is an error result");
			}
		}

		public Exception Error {
			get {
				return e;
			}
		}

		public bool Success {
			get {
				return false;
			}
		}

		public bool HasError {
			get {
				return true;
			}
		}
	}
}

