using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
	
	public GameObject standardItem;
	public GameObject standardVFX;

	public Text scoreText;



	// Use this for initialization
	void Start () 
	{	
		SaveLoad.Load ();

		for ( int i = 0; i < ItemInfo.globalCount; i++ ) 
		{
			int resourceID = Random.Range ( 0, ItemInfo.typeCount );
			GameObject curObj = Instantiate ( standardItem ) as GameObject;

			Sprite curSprite = Resources.Load<Sprite>( "Sprites/" + ItemInfo.spriteResource [ resourceID ] );
			curObj.GetComponent<SpriteRenderer>().sprite = curSprite;

			string curName = "Item_" + i;
			curObj.name = curName;		
			curObj.transform.position = new Vector2( ItemInfo.DEFAULT_X, ItemInfo.DEFAULT_Y );

			GameObject curVfx = GameObject.Instantiate( standardVFX ) as GameObject;
			curVfx.name = "Vfx_" + i;	
			curVfx.GetComponent<ParticleSystem>().Pause();

			Item curItem = new Item ( i, resourceID, curObj, false, false );
			ItemInfo.all_Items[ i ] = curItem;
		}


		int itemCount = 0;	

		for ( int i = 0; i < ItemInfo.line; i++ ) 
		{
			for ( int j = 0; j < ItemInfo.column; j++ ) 
			{			
				ItemInfo.gameField[ i, j ] = ItemInfo.all_Items[ itemCount ];
				ItemInfo.all_Items[ itemCount ].isUsedInField = true;

				float curItemX = ItemInfo.START_X + ItemInfo.DELTA_X * j;
				float curItemY = ItemInfo.START_Y + ItemInfo.DELTA_Y * i;	

				GameObject curObj = ItemInfo.all_Items[ itemCount ].objectInGame;
				curObj.transform.position = new Vector2( curItemX, curItemY );

				itemCount++;
			}
		}


		while ( ItemInfo.FindPotentialWinItem() == null || 
				ItemInfo.FindWinItem( 0, ItemInfo.line, 0, ItemInfo.column ).Count > 0 )  		
		{
			for ( int i = 0; i < ItemInfo.activeCount; i++ ) 
			{
				int resourceID = Random.Range ( 0, ItemInfo.typeCount );
				ItemInfo.all_Items [ i ].type = resourceID;

				GameObject curObj = ItemInfo.all_Items[ i ].objectInGame;
				Sprite curSprite = Resources.Load<Sprite>( "Sprites/" + ItemInfo.spriteResource [ resourceID ] );
				curObj.GetComponent<SpriteRenderer>().sprite = curSprite;
			}
		}

	}


	void Update ()
	{
		scoreText.text = "Score: " + System.Environment.NewLine + ItemInfo.score;
	}

}
