using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class IABUIManager : MonoBehaviourGUI
{
#if UNITY_ANDROID
	void OnGUI()
	{
		beginColumn();

		if( GUILayout.Button( "Initialize IAB" ) )
		{
			var key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApi6+DL/itNA4Qqog3d/wTk4tAxwxQRBF0VbmGv2ry2dhevo7Vi/z8gWTmr6aeQs3R24xCBOyYArceqymT0RKsFsmz7bh6IUc2rO6MftX1QGQdWXSyxYqZo4gOOeMeTYWmpNWubA2riL5hsx1R8mYgSemGW85WCEpRynI5T1yR3ioWoI8Fz3gNyQdcFiwYLn44t9tRX2fMNT3gdxLU4qMlVd+71ArZRyZI/zaYIIFCkRUvDpitYd3aPMzA7LZMGm96zCPd+zpNRptjjU5kG6ezA7JGKx4JJ3/CHKvyKzJrEo5loQLot5bBc+Rw/omyOQgd306P3g72qubFHlFLzFKbwIDAQAB";
			GoogleIAB.init( key );
		}


		if( GUILayout.Button( "Query Inventory" ) )
		{
			// enter all the available skus from the Play Developer Console in this array so that item information can be fetched for them
			var skus = new string[] {"android.test.purchased", "cp1"};
			GoogleIAB.queryInventory( skus );
		}


		if( GUILayout.Button( "Are subscriptions supported?" ) )
		{
			 Debug.Log( "subscriptions supported: " + GoogleIAB.areSubscriptionsSupported() );
		}


		if( GUILayout.Button( "Purchase Test Product" ) )
		{
			GoogleIAB.purchaseProduct( "cp1" );
		}


		if( GUILayout.Button( "Consume Test Purchase" ) )
		{
			GoogleIAB.consumeProduct( "cp1" );
		}


		if( GUILayout.Button( "Test Unavailable Item" ) )
		{
			GoogleIAB.purchaseProduct( "android.test.item_unavailable" );
		}


		endColumn( true );


		if( GUILayout.Button( "Purchase Real Product" ) )
		{
			GoogleIAB.purchaseProduct( "com.prime31.testproduct", "payload that gets stored and returned" );
		}


		if( GUILayout.Button( "Purchase Real Subscription" ) )
		{
			GoogleIAB.purchaseProduct( "com.prime31.testsubscription", "subscription payload" );
		}


		if( GUILayout.Button( "Consume Real Purchase" ) )
		{
			GoogleIAB.consumeProduct( "com.prime31.testproduct" );
		}


		if( GUILayout.Button( "Enable High Details Logs" ) )
		{
			GoogleIAB.enableLogging( true );
		}


		if( GUILayout.Button( "Consume Multiple Purchases" ) )
		{
			var skus = new string[] { "com.prime31.testproduct", "android.test.purchased" };
			GoogleIAB.consumeProducts( skus );
		}

		endColumn();
	}
#endif
}
