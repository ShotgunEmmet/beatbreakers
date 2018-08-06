using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GSApi 
{
	void FireMessageReceived (Hashtable message);
	String sessionId{ get; set; }
}
	
public class GSMessageReceivedEventArgs : EventArgs
{
	public GSMessageReceivedEventArgs (Hashtable message)
	{
		Message = message;
	}

	public Hashtable Message { get; private set; }
}
	 


