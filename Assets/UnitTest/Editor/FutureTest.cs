using System;

using SharpUnit;
using GeishaTokyo.Concurrent.Factory;
using System.Threading;

namespace GeishaTokyo.Concurrent.Test
{
	public class FutureTest : TestCase
	{
		
		[UnitTest]
		public void TestBeforeDone(){
			FutureCall<string> f = FutureFactory.NewFuture<string>();
			
			// Test properties
			Assert.False(f.Done);
			Assert.False(f.Success);
			Assert.False(f.HasError);
			
			// Test wait
			Assert.ExpectException(new TimeoutException("Wait timeout"));
			f.Wait(0.5f);
			
		}
		
		
		[UnitTest]
		public void TestSetResult(){
			FutureCall<string> f = FutureFactory.NewFuture<string>();
			
			
			string result = "Succeess!";
			f.SetResult(result);
			
			// Test properties after set result
			Assert.True(f.Done);
			Assert.True(f.Success);
			Assert.False(f.HasError);
			
			Assert.Equal(result,f.Result);
			Assert.ExpectException(new NotAllowedOperationException("Can't get result from succeeded future"));
			var e = f.Error;
			
			Assert.False(true,"Never called" + e);
		}
		
		[UnitTest]
		public void TestSetError(){
			var f = FutureFactory.NewFuture<string>();
			
			Exception e = new Exception("Fail!");
			f.SetError(e);
			
			// Test properties after set error
			Assert.True(f.Done);
			Assert.False(f.Success);
			Assert.True(f.HasError);
			
			
			Assert.Equals(e,f.Error);
			Assert.ExpectException(new NotAllowedOperationException("Can't get result from not succeeded future"));
			var r = f.Result;
			
			Assert.False(true,"Never called" + r);
			
		}
		
		[UnitTest]
		public void TestCallbacks(){
			var f = FutureFactory.NewFuture<string>();
			string resultString = "Success!";
			bool[] calls = new bool[3];
			f.OnComplete( r => {
				Assert.True(r.Success);
				Assert.False(r.HasError);
				Assert.Equals(resultString,r.Result);
				calls[0] = true;
			});
			f.OnSuccess( r => {
				
				Assert.Equals(resultString,r);
				calls[1] = true;
			});
			
			f.OnError( e => {
				Assert.False(true,"Never called");
				calls[2] = false;
			});
			
			// inform result
			f.SetResult(resultString);
			
			Assert.True(calls[0]);
			Assert.True(calls[1]);
			Assert.False(calls[2]);
		}
		[UnitTest]
		public void TestCallbacksSetAfterDone(){
			var f = FutureFactory.NewFuture<string>();
			string resultString = "Success!";
			
			// inform result
			f.SetResult(resultString);
			
			
			bool[] calls = new bool[3];
			f.OnComplete( r => {
				Assert.True(r.Success);
				Assert.False(r.HasError);
				Assert.Equals(resultString,r.Result);
				calls[0] = true;
			});
			f.OnSuccess( r => {
				
				Assert.Equals(resultString,r);
				calls[1] = true;
			});
			
			f.OnError( e => {
				Assert.False(true,"Never called");
				calls[2] = false;
			});
			
			
			Assert.True(calls[0]);
			Assert.True(calls[1]);
			Assert.False(calls[2]);
		}
	}
}

