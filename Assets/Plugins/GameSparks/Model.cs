using System;
using System.Collections;


namespace GameSparksApi
{
	public class EventVal
	{
		private string key;
		private object val;
		
		public EventVal (string key, string val)
		{
			this.key = key;
			this.val = val;
		}
		
		public EventVal (string key, int val)
		{
			this.key = key;
			this.val = val;
		}
		
		public string getKey(){
			return key;
		}
		
		public object getVal(){
			return val;
		}
	}
	
	public abstract class GSResponse{
		public Hashtable errors{get; set;}
		public GSResponse(Hashtable response){
			this.errors = (Hashtable)response["error"];	
		}
	}
}

