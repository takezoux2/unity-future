using System;

namespace GeishaTokyo.Concurrent
{
	public class MultiException : Exception
	{
		Exception[] exceptions;
		
		public Exception[] Exceptions{
			get{ return exceptions;}
		}
		public MultiException (params Exception[] exceptions) : base("There are " + exceptions.Length  + " exceptions.")
		{
			this.exceptions = exceptions;
		}
		
		
	}
}

