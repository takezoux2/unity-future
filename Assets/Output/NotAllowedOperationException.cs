using System;

namespace GeishaTokyo.Concurrent
{
	public class NotAllowedOperationException : Exception
	{
		public NotAllowedOperationException (string message) : base(message)
		{
			
		}
	}
}

