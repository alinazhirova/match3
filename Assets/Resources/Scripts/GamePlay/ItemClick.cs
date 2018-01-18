using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ItemClick : MonoBehaviour {

	private int EMPTY = -1;

	private Material itemMaterial;

	private float startTime = 0f;
	private float swapRate = 0.5f;

	private bool isSwapping = false;
	private bool isReverseSwapping = false;
	private bool isTopItemsMoving = false;
	private bool isOperationEnd = false;

	private bool isSwappingFirst = true;
	private bool isReverseSwappingFirst = true;
	private bool isTopItemsMovingFirst = true;

	private int oldLine = 0;
	private int oldColumn = 0;
	private int newLine = 0;
	private int newColumn = 0;

	private List<Item> winItem = null;

	private List<GameObject> movingItems = null;
	private List<Vector3> oldPosition = null;
	private List<Vector3> newPosition = null;

	private bool isSadness = false;
	private int itemID = 0;





	void DemoMode () 
	{
		Item[] winPotentialItem = ItemInfo.FindPotentialWinItem ();

		if ( winPotentialItem != null )
		{			
			ItemInfo.lastItemID = winPotentialItem [0].ID;
			itemID = winPotentialItem [1].ID;
			OnMouseDown ();
		} 
		else
		{
			isSadness = true;
		}
	}


	void Start () 
	{
		startTime = Time.time;
		DemoMode ();
	}


	void OnMouseEnter () 
	{
		itemMaterial = gameObject.GetComponent<SpriteRenderer> ().material;
		gameObject.GetComponent<SpriteRenderer>().material = new Material( Resources.Load<Material>( "Materials/add_material" ) );
	}


	void OnMouseExit ()
	{
		gameObject.GetComponent<SpriteRenderer>().material = new Material( itemMaterial );
	}


	void OnMouseDown ()
	{	
		if ( !ItemInfo.isBlocking )
		{
			if ( !ItemInfo.isDemo ) 
			{
				string itemName = gameObject.name;
				string newItemName = itemName.Remove (0, 5);
				itemID = Convert.ToInt32 (newItemName); 
			}

			GameObject itemFrame = GameObject.Find ("frame");

			if (ItemInfo.lastItemID == EMPTY) 
			{
				ItemInfo.lastItemID = itemID;

				itemFrame.GetComponent<SpriteRenderer> ().color = new Color (255f, 255f, 255f, 1f);
				itemFrame.transform.position = gameObject.transform.position;
			} 
			else if (ItemInfo.lastItemID != itemID) 
			{
				itemFrame.GetComponent<SpriteRenderer> ().color = new Color (255f, 255f, 255f, 0f);

				int[] oldPposition = new int[ 2 ];
				oldPposition = ItemInfo.GettingItemPositionByID (ItemInfo.lastItemID);

				oldLine = oldPposition [0];
				oldColumn = oldPposition [1];

				int[] newPposition = new int[ 2 ];
				newPposition = ItemInfo.GettingItemPositionByID (itemID);

				newLine = newPposition [0];
				newColumn = newPposition [1];

				ItemInfo.lastItemID = EMPTY;

				if (CheckingOnNearby (oldLine, oldColumn, newLine, newColumn))
				{
					ItemInfo.isBlocking = true;
					startTime = Time.time;
					isSwapping = true;

					if ( !ItemInfo.isDemo )
					{
						ItemInfo.score--;
					}
				}	
			} 
			else 
			{
				itemFrame.GetComponent<SpriteRenderer> ().color = new Color (255f, 255f, 255f, 0f);
				ItemInfo.lastItemID = EMPTY;
			}
		}
	}


	private bool CheckingOnNearby ( int lastLine, int lastColumn, int newLine, int newColumn )
	{
		if ( ( lastLine > 0 && newLine == lastLine - 1 && lastColumn == newColumn ) || 
			( lastLine < ItemInfo.line - 1 && newLine == lastLine + 1 && lastColumn == newColumn ) || 
			( lastColumn > 0 && newColumn == lastColumn - 1 && lastLine == newLine ) || 
			( lastColumn < ItemInfo.line - 1 && newColumn == lastColumn + 1 ) && lastLine == newLine )
		{			
			return true;
		}

		return false;
	}


	private int TopItemsDisplacement ( int curColumn )
	{				
		int[] indexes = GettingShiftInColumn( curColumn );

		if ( indexes != null )
		{
			int delta = indexes [ 1 ] - indexes [ 0 ] + 1;

			for ( int k = indexes [ 1 ]; k > 0; k-- ) 
			{
				if ( k >= delta )
				{
					AddInfoInAnimList ( k - delta, curColumn, k, curColumn );
					ItemInfo.gameField [ k, curColumn ] = ItemInfo.gameField [ k - delta, curColumn ];	
				}
			}
			return delta;
		}

		return 0;
	}


	private int[] GettingShiftInColumn ( int curColumn )
	{
		int[] indexes = new int[ 2 ];
		int startIndex = 0;

		for ( int i = 0; i < ItemInfo.line; i++ )
		{		
			if ( ItemInfo.gameField [ i, curColumn ].isNeedToRemove )
			{
				if ( startIndex == 0 ) 
				{
					indexes [ 0 ] = i;
				}

				startIndex = startIndex + 1;

				if ( i == ItemInfo.line - 1 )
				{
					indexes [ 1 ] = i;
					return indexes;
				}
			}
			else if ( startIndex != 0 )
			{
				startIndex = 0;
				indexes [ 1 ] = i - 1;
				return indexes;
			}
		}

		return null;
	}


	private List<Item> NewItemsGeneration ( int needingCount )
	{
		List<Item> newItems = new List<Item>();

		for ( int i = 0; i < ItemInfo.globalCount; i++ )
		{
			if ( !ItemInfo.all_Items[ i ].isUsedInField )
			{
				bool isRemoved = false;
				for ( int j = 0; j < winItem.Count; j++ )
				{
					if ( ItemInfo.all_Items[ i ].ID == winItem[ j ].ID )
					{
						isRemoved = true;
						break;
					}
				}

				if ( !isRemoved && newItems.Count <= needingCount )
				{
					newItems.Add ( ItemInfo.all_Items[ i ] );

					if ( newItems.Count == needingCount ) 
					{
						return newItems;
					}
				}
			}
		}

		return newItems;
	}


	private void NewItemsHandle ( List<Item> newItems, int curColumn )
	{
		for ( int i = 0; i < newItems.Count; i++ ) 
		{
			int resourceID = UnityEngine.Random.Range ( 0, ItemInfo.typeCount );
			newItems [ i ].type = resourceID;
			newItems [ i ].isUsedInField = true;

			Sprite curSprite = Resources.Load<Sprite>( "Sprites/" + ItemInfo.spriteResource [ resourceID ] );
			newItems[ i ].objectInGame.GetComponent<SpriteRenderer>().sprite = curSprite;

			ItemInfo.gameField [ i, curColumn ] = newItems [ i ];

			float oldItemX = ItemInfo.START_X + ItemInfo.DELTA_X * curColumn;
			float oldItemY = ItemInfo.START_Y - ItemInfo.DELTA_Y * ( newItems.Count - i );	

			float newItemX = ItemInfo.START_X + ItemInfo.DELTA_X * curColumn;
			float newItemY = ItemInfo.START_Y + ItemInfo.DELTA_Y * i;	

			newItems [ i ].objectInGame.transform.position = new Vector2 ( oldItemX, oldItemY );
			AddInfoInAnimList_2 ( newItems [ i ].objectInGame, new Vector3 ( oldItemX, oldItemY, 0f ), new Vector3 ( newItemX, newItemY, 0f ) );
		}				
	}


	private void ItemsRemove ()
	{
		for ( int i = 0; i < winItem.Count; i++ )
		{
			winItem [ i ].isNeedToRemove = false;
			winItem [ i ].isUsedInField = false;
			winItem [ i ].objectInGame.transform.position = new Vector2( ItemInfo.DEFAULT_X, ItemInfo.DEFAULT_Y );
			winItem [ i ].objectInGame.GetComponent<SpriteRenderer>().color = new Color( 255f, 255f, 255f, 1f );
		}	
	}


	private void RemovingItemAnim ( Item curItem )
	{
		string vfxName = "Vfx_" + curItem.ID;
		GameObject itemVFX = GameObject.Find ( vfxName );

		itemVFX.transform.position = curItem.objectInGame.transform.position;
		itemVFX.GetComponent<ParticleSystem>().Play();

		curItem.objectInGame.GetComponent<SpriteRenderer>().color = new Color( 255f, 255f, 255f, 0f );
	}


	private void PrivateUpdate ()
	{
		for ( int i = 0; i < ItemInfo.line; i++ ) 
		{
			for ( int j = 0; j < ItemInfo.column; j++ ) 
			{	
				float curItemX = ItemInfo.START_X + ItemInfo.DELTA_X * j;
				float curItemY = ItemInfo.START_Y + ItemInfo.DELTA_Y * i;	

				GameObject curObj = ItemInfo.gameField[ i, j ].objectInGame;
				curObj.transform.position = new Vector2( curItemX, curItemY );
			}
		}

	}


	private void ItemMoving ( GameObject curItem, Vector3 oldPosition, Vector3 newPosition )
	{
		Vector3 center = ( oldPosition + newPosition ) * 0.5f;
		Vector3 riseRelCenter = oldPosition - center;
		Vector3 setRelCenter = newPosition - center;
		float fracComplete = ( Time.time - startTime ) / swapRate;

		curItem.transform.position = Vector3.Slerp( riseRelCenter, setRelCenter, fracComplete) ;
		curItem.transform.position += center;
	}


	private void AddInfoInAnimList ( int curOldLine, int curOldColumn, int curNewLine, int curNewColumn )
	{		
		GameObject curItem = ItemInfo.gameField [ curOldLine, curOldColumn ].objectInGame;

		float oldItemX = ItemInfo.START_X + ItemInfo.DELTA_X * curOldColumn;
		float oldItemY = ItemInfo.START_Y + ItemInfo.DELTA_Y * curOldLine;	

		float newItemX = ItemInfo.START_X + ItemInfo.DELTA_X * curNewColumn;
		float newItemY = ItemInfo.START_Y + ItemInfo.DELTA_Y * curNewLine;	

		Vector3 oldCoord = new Vector3( oldItemX, oldItemY, 0f );
		Vector3 newCoord = new Vector3( newItemX, newItemY, 0f );

		movingItems.Add ( curItem );
		oldPosition.Add ( oldCoord );
		newPosition.Add ( newCoord );
	}


	private void AddInfoInAnimList_2 ( GameObject curItem, Vector3 oldCoord, Vector3 newCoord )
	{		
		movingItems.Add ( curItem );
		oldPosition.Add ( oldCoord );
		newPosition.Add ( newCoord );
	}


	private bool IsGameFinish ()
	{
		if ( ItemInfo.score <= 0 || ItemInfo.FindPotentialWinItem () == null )
		{
			return true;
		}
		return false;
	}


	private bool IsRecord ()
	{
		if ( SaveLoad.globalRecordScore.Count > 0 )
		{
			int count = 0;
			for ( int i = 0; i < SaveLoad.globalRecordScore.Count; i ++ )
			{
				if ( ItemInfo.score > SaveLoad.globalRecordScore[ i ] )
				{
					count++;
				}
			}

			if ( count < SaveLoad.globalRecordScore.Count - 1 )
			{
				return false;
			}
		}
		return true;
	}


	private void HandleRecord ()
	{
		SaveLoad.globalRecordScore.Insert ( 0, ItemInfo.score );
		DateTime date = DateTime.Now;
		SaveLoad.globalRecordDate.Insert ( 0, Convert.ToString( date ) );
		SaveLoad.Save ();
	}


	void OnGUI ()
	{
		if ( isSadness ) 
		{
			GUI.Box ( new Rect ( 0, 0, 500, 400 ), "" );

			if ( GUI.Button( new Rect ( 180, 130, 100, 50 ), "Ok" ) ) 
			{
				isSadness = false;	
				SceneManager.LoadScene( "Menu" );
			}
		}
	}


	void Update ()
	{
		if ( isSwapping )
		{	
			if ( isSwappingFirst ) 
			{	
				isSwappingFirst = false;	
				movingItems = new List<GameObject> ();
				oldPosition = new List<Vector3> ();
				newPosition = new List<Vector3> ();

				AddInfoInAnimList ( oldLine, oldColumn, newLine, newColumn );
				AddInfoInAnimList ( newLine, newColumn, oldLine, oldColumn );

				Item temp = ItemInfo.gameField [ oldLine, oldColumn ];
				ItemInfo.gameField [ oldLine, oldColumn ] = ItemInfo.gameField [ newLine, newColumn ];
				ItemInfo.gameField [ newLine, newColumn ] = temp; 
			}

			for ( int i = 0; i < movingItems.Count; i++ ) 
			{
				ItemMoving ( movingItems [ i ], oldPosition [ i ], newPosition [ i ] );
			}

			if ( Vector3.Distance ( movingItems [ movingItems.Count - 1 ].transform.position, newPosition [ movingItems.Count - 1 ] ) < 0.1 )
			{
				isSwapping = false;
				isSwappingFirst = true;

				winItem = ItemInfo.FindWinItem ( 0, ItemInfo.line, 0, ItemInfo.column );
				startTime = Time.time;

				if ( winItem.Count > 0 )
				{
					for ( int i = 0; i < winItem.Count; i++ ) 
					{
						winItem [ i ].isNeedToRemove = true;
						RemovingItemAnim ( winItem [ i ] );
					}
					isTopItemsMoving = true;
				} 
				else 
				{					
					isReverseSwapping = true;
				}
			}
		}
		/////////////////////////////////////////////////////
		else if ( isReverseSwapping ) 
		{	
			if ( isReverseSwappingFirst )
			{			
				isReverseSwappingFirst = false;	
				movingItems = new List<GameObject> ();
				oldPosition = new List<Vector3> ();
				newPosition = new List<Vector3> ();

				AddInfoInAnimList ( newLine, newColumn, oldLine, oldColumn );
				AddInfoInAnimList ( oldLine, oldColumn, newLine, newColumn );

				Item temp = ItemInfo.gameField [ oldLine, oldColumn ];
				ItemInfo.gameField [ oldLine, oldColumn ] = ItemInfo.gameField [ newLine, newColumn ];
				ItemInfo.gameField [ newLine, newColumn ] = temp; 
			}

			for ( int i = 0; i < movingItems.Count; i++ )
			{
				ItemMoving ( movingItems [ i ], oldPosition [ i ], newPosition [ i ] );
			}

			if ( Vector3.Distance ( movingItems [ movingItems.Count - 1 ].transform.position, newPosition [ movingItems.Count - 1 ] ) < 0.1 ) 
			{
				ItemInfo.isBlocking = false;
				isReverseSwapping = false;
				isReverseSwappingFirst = true;
							
				if ( IsGameFinish () )
				{
					if ( IsRecord () ) 
					{
						HandleRecord ();
						SceneManager.LoadScene( "Records" );
					} 
					else 
					{
						isSadness = true;
					}
				}
			}
		}
		/////////////////////////////////////////////////////
		else if ( isTopItemsMoving ) 
		{
			if ( isTopItemsMovingFirst ) 
			{
				isTopItemsMovingFirst = false;
				movingItems = new List<GameObject> ();
				oldPosition = new List<Vector3> ();
				newPosition = new List<Vector3> ();

				for ( int j = 0; j < ItemInfo.column; j++ ) 
				{				
					int removedItemsCount = TopItemsDisplacement ( j ); 

					if ( removedItemsCount != 0 )
					{
						List<Item> newItems = NewItemsGeneration ( removedItemsCount );
						NewItemsHandle ( newItems, j );
					}
				}
			}

			for ( int i = 0; i < movingItems.Count; i++ ) 
			{
				ItemMoving ( movingItems [ i ], oldPosition [ i ], newPosition [ i ] );
			}

			if ( Vector3.Distance ( movingItems [ movingItems.Count - 1 ].transform.position, newPosition [ movingItems.Count - 1 ] ) < 0.1 ) 
			{
				isTopItemsMoving = false;
				isTopItemsMovingFirst = true;

				startTime = Time.time;
				isOperationEnd = true;
			}
		}
		/////////////////////////////////////////////////////
		else if ( isOperationEnd ) 
		{
			if ( !ItemInfo.isDemo ) 
			{				
				ItemInfo.score = ItemInfo.score + winItem.Count - 1;
			}
			isOperationEnd = false;
			ItemsRemove ();
			winItem = ItemInfo.FindWinItem ( 0, ItemInfo.line, 0, ItemInfo.column );

			if ( winItem.Count > 0 ) 
			{
				for ( int i = 0; i < winItem.Count; i++ ) 
				{
					winItem [ i ].isNeedToRemove = true;
					RemovingItemAnim ( winItem [ i ] );
				}
				startTime = Time.time;
				isTopItemsMoving = true;
			} 
			else 					
			{
				ItemInfo.isBlocking = false;

				if ( ItemInfo.isDemo ) 
				{
					DemoMode ();
				}
				else if ( IsGameFinish () )
				{
					if ( IsRecord () ) 
					{
						HandleRecord ();
						SceneManager.LoadScene( "Records" );
					} 
					else 
					{
						isSadness = true;
					}
				}
			}
		}
	}





}
