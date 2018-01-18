using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonsClick : MonoBehaviour {

	public GameObject itemVFX;

	private bool isMenu = false;




	public void Demo ()
	{
		GameObject curButton = GameObject.Find ( "Demo" );
		curButton.SetActive ( false );
		ItemInfo.isDemo = false;
	}


	public void Hint ()
	{
		Item[] winPotentialItem = ItemInfo.FindPotentialWinItem ();

		if ( winPotentialItem != null )
		{
			itemVFX.transform.position = winPotentialItem [ 1 ].objectInGame.transform.position;
			itemVFX.GetComponent<ParticleSystem>().Play();
		}
	}


	public void Menu ()
	{
		isMenu = true;
	}


	public void DeveloperCLick ()
	{
		Application.OpenURL ( "https://vk.com/id7094353" ); 
	}


	void OnGUI ()
	{
		if ( isMenu ) 
		{
			GUI.Box ( new Rect ( 0, 0, 500, 400 ), "" );

			if ( GUI.Button( new Rect ( 120, 150, 100, 50 ), "Menu" ) ) 
			{
				isMenu = false;	
				SceneManager.LoadScene( "Menu" );
			}

			if ( GUI.Button( new Rect ( 250, 150, 100, 50 ), "CANCEL" ) ) 
			{
				isMenu = false;
			}
		}
	}




}