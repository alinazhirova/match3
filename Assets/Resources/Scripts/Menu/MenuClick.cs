using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuClick : MonoBehaviour {

	private bool isExit = false;



	public void GamePlay ()
	{
		SceneManager.LoadScene( "GamePlay" );
	}


	public void Info ()
	{
		SceneManager.LoadScene( "Info" );
	}


	public void Records ()
	{
		SceneManager.LoadScene( "Records" );
	}


	public void Exit ()
	{
		isExit = true;
	}


	void OnGUI ()
	{
		if ( isExit ) 
		{
			GUI.Box ( new Rect ( 0, 0, 500, 400 ), "" );

			if ( GUI.Button( new Rect ( 120, 150, 100, 50 ), "EXIT" ) ) 
			{
				isExit = false;	
				Application.Quit ();
			}

			if ( GUI.Button( new Rect ( 250, 150, 100, 50 ), "CANCEL" ) ) 
			{
				isExit = false;
			}
		}
	}




}
