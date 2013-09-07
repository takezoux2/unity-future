using System;

using SharpUnit;
using GeishaTokyo.Concurrent.Factory;
namespace GeishaTokyo.Concurrent.Test
{
	public class FutureMorphingTest : TestCase
	{
		[UnitTest]
		public void TestMap(){
			var f = FutureFactory.NewFuture<string>();
			
			var f2 = f.Map<bool>( str => {
				return str == "true";
			});
			
			f.SetResult("true");
			
			Assert.True(f2.Result);
		}
		
		[UnitTest]
		public void TestRecover(){
			var f = FutureFactory.NewFuture<string>();
			
			var f2 = f.Recover(e => {
				return "recovered";
			});
			
			f.SetError(new Exception());
			
			Assert.Equals("recovered",f2.Result);
		}
		
		[UnitTest]
		public void TestJoin(){
			var f = FutureFactory.NewFuture<string>();
			var other = FutureFactory.NewFuture<int>();
			
			var f2 = f.Join<int>(other);
			
			f.SetResult("ok");
			Assert.False(f2.Done);
			other.SetResult(1);
			Assert.True(f2.Done);
			
			Tuple2<string,int> r = f2.Result;
			
			Assert.Equals("ok",r.V1);
			Assert.Equals(1,r.V2);
		}
		
		
		[UnitTest]
		public void TestJoinError(){
			var f = FutureFactory.NewFuture<string>();
			var other = FutureFactory.NewFuture<int>();
			
			var f2 = f.Join<int>(other);
			
			f.SetResult("ok");
			Assert.False(f2.Done);
			var e = new Exception();
			other.SetError(e);
			Assert.True(f2.Done);
			
			Assert.True(f2.HasError);
			Assert.Equals(e,f2.Error);
		}
		
		[UnitTest]
		public void TestSelect(){
			
			
			// success x success
			{
				var f1 = FutureFactory.NewFuture<string>();
				var f2 = FutureFactory.NewFuture<string>();
				
				var sel = f1.Select(f2);
				
				Assert.False(sel.Done);
				f1.SetResult("ok1");
				Assert.True(sel.Done);
				f2.SetResult("ok2");
				Assert.True(sel.Done);
				Assert.Equals("ok1",sel.Result);
			}
			
			
			// error x success
			{
				var f1 = FutureFactory.NewFuture<string>();
				var f2 = FutureFactory.NewFuture<string>();
				
				var sel = f1.Select(f2);
				
				Assert.False(sel.Done);
				f1.SetError(new Exception());
				Assert.False(sel.Done);
				f2.SetResult("ok2");
				Assert.True(sel.Done);
				Assert.Equals("ok2",sel.Result);
			}
			
			
			// success x error
			{
				var f1 = FutureFactory.NewFuture<string>();
				var f2 = FutureFactory.NewFuture<string>();
				
				var sel = f1.Select(f2);
				
				Assert.False(sel.Done);
				f1.SetResult("ok1");
				Assert.True(sel.Done);
				f2.SetError(new Exception());
				Assert.True(sel.Done);
				Assert.Equals("ok1",sel.Result);
			}
			// error x error
			{
				var f1 = FutureFactory.NewFuture<string>();
				var f2 = FutureFactory.NewFuture<string>();
				
				var sel = f1.Select(f2);
				
				Assert.False(sel.Done);
				f1.SetError(new Exception());
				Assert.False(sel.Done);
				f2.SetError(new Exception());
				Assert.True(sel.Done);
				Assert.Equals(new MultiException(f1.Error,f2.Error),sel.Error);
			}
			
		}
		
	}
}

