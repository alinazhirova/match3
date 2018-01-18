using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RecordsHandle : MonoBehaviour {
	

	void OnGUI ()
	{
		SaveLoad.Load ();
		float position = 0f;

		for ( int i = 0; i < SaveLoad.globalRecordScore.Count; i++ )
		{
			string info = "Player" + ( i + 1 ) + "    Score: " + SaveLoad.globalRecordScore[ i ] + "    Date: " +  SaveLoad.globalRecordDate[ i ];
			position = position + 25f;

			GUI.Box ( new Rect ( 20, position, 400, 20 ), info );
		}
	}


}
