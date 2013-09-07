using System;

namespace GeishaTokyo.Concurrent
{
	public static class Tuples{
		
	
		public static Tuple2<T1,T2> Tuple<T1,T2>(T1 v1,T2 v2){
			return new Tuple2<T1,T2>(v1,v2);
		}
		public static Tuple3<T1,T2,T3> Tuple<T1,T2,T3>(T1 v1,T2 v2,T3 v3){
			return new Tuple3<T1,T2,T3>(v1,v2,v3);
		}
	}
	
	
	public class Tuple2<T1,T2>
	{
		public T1 V1;
		public T2 V2;
		
		public Tuple2 (T1 v1,T2 v2)
		{
			this.V1 = v1;
			this.V2 = v2;
		}
	}
	
	public class Tuple3<T1,T2,T3>
	{
		public T1 V1;
		public T2 V2;
		public T3 V3;
		
		public Tuple3 (T1 v1,T2 v2,T3 v3)
		{
			this.V1 = v1;
			this.V2 = v2;
			this.V3 = v3;
		}
	}
}

