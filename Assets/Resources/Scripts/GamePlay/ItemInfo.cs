using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ItemInfo 
{
	public static int globalCount = 90;
	public static int activeCount = 64;

	public static int line = 8;
	public static int column = 8;

	public static int typeCount = 6;

	public static Item[] all_Items = new Item[ globalCount ];

	public static string[] spriteResource = new string[] { "cake", "cupcake", "gingerbread-man", "gummy-bear", "heart-lollipop", "ice-cream" };

	public static Item[,] gameField = new Item[ line, column ];

	public static int lastItemID = -1;

	public static float DEFAULT_X = -15f;
	public static float DEFAULT_Y = 8f;

	public static float START_X = -5f;
	public static float START_Y = 3.8f;
	public static float DELTA_X = 1.05f;
	public static float DELTA_Y = -1.05f;

	public static int score = 3;
	public static bool isBlocking = false;
	public static bool isDemo = true;




	public static Item[] FindPotentialWinItem ()
	{
		Item[] winPotentialItem = new Item[ 2 ];

		winPotentialItem = FindPotentialWinItemOnHorizontal ();

		if ( winPotentialItem == null )
		{
			winPotentialItem = FindPotentialWinItemOnVertical ();
		}

		return winPotentialItem;		
	}


	private static Item[] FindPotentialWinItemOnHorizontal ()
	{
		Item[] resultItem = new Item[ 2 ];

		for ( int i = 0; i < line; i++ )
		{
			for ( int j = 0; j < column - 3; j++ )
			{
				int curType = gameField [ i, j ].type;
				int nearType = gameField [ i, j + 1 ].type;

				if ( curType == nearType )
				{	
					int nextType = gameField [ i, j + 3 ].type;

					if ( curType == nextType ) 
					{
						resultItem[ 0 ] = gameField [ i, j + 2 ];
						resultItem[ 1 ] = gameField [ i, j + 3 ];
						return resultItem;
					}
				}
			}
			////////////////////
			for ( int j = column - 1; j > 2; j-- )
			{
				int curType = gameField [ i, j ].type;
				int nearType = gameField [ i, j - 1 ].type;

				if ( curType == nearType )
				{	
					int prevType = gameField [ i, j - 3 ].type;

					if ( curType == prevType ) 
					{
						resultItem[ 0 ] = gameField [ i, j - 2 ];
						resultItem[ 1 ] = gameField [ i, j - 3 ];
						return resultItem;
					}
				}
			}

		}
		return null; 
	}


	private static Item[] FindPotentialWinItemOnVertical ()
	{
		Item[] resultItem = new Item[ 2 ];

		for ( int i = 0; i < column; i++ )
		{
			for ( int j = 0; j < line - 3; j++ )
			{
				int curType = gameField [ j, i ].type;
				int nearType = gameField [ j + 1, i ].type;

				if ( curType == nearType )
				{			
					int nextType = gameField [ j + 3, i ].type;

					if ( curType == nextType ) 
					{
						resultItem[ 0 ] = gameField [ j + 2, i ];
						resultItem[ 1 ] = gameField [ j + 3, i ];
						return resultItem;
					}
				}
			}
			////////////////////
			for ( int j = line - 1; j > 2; j-- )
			{
				int curType = gameField [ j, i ].type;
				int nearType = gameField [ j - 1, i ].type;

				if ( curType == nearType )
				{			
					int nextType = gameField [ j - 3, i ].type;

					if ( curType == nextType ) 
					{
						resultItem[ 0 ] = gameField [ j - 2, i ];
						resultItem[ 1 ] = gameField [ j - 3, i ];
						return resultItem;
					}
				}
			}
		}
		return null; 
	}


	public static List<Item> FindWinItem ( int startLine, int endLine, int startColumn, int endColumn )
	{
		List<Item> winItemHorizontal = new List<Item>();
		List<Item> winItemVertical = new List<Item>();

		winItemHorizontal = FindWinItemOnHorizontal ( startLine, endLine );

		if ( winItemHorizontal.Count == 0 ) 
		{
			winItemVertical = FindWinItemOnVertical ( startColumn, endColumn );
			return winItemVertical;
		}

		return winItemHorizontal;
	}


	private static List<Item> FindWinItemOnHorizontal ( int startLine, int endLine )
	{
		List<Item> winItem = new List<Item>();

		for ( int i = startLine; i < endLine; i++ ) 
		{
			int j = 0;
			int jNew = 1;

			while ( j < column - 2 )
			{				
				int type_1 = gameField [ i, j ].type;
				int type_2 = gameField [ i, j + 1 ].type;
				int type_3 = gameField [ i, j + 2 ].type;

				if ( type_1 == type_2 && type_2 == type_3 )
				{
					winItem.Add ( gameField [ i, j ] );
					winItem.Add ( gameField [ i, j + 1 ] );
					winItem.Add ( gameField [ i, j + 2 ] );

					jNew = j + 3;
					int typeNext = -1;

					if ( jNew < column ) 
					{
						typeNext = gameField [ i, jNew ].type;
					}

					while ( type_1 == typeNext && jNew < column ) 
					{
						winItem.Add ( gameField [ i, jNew ] );
						jNew = jNew + 1;

						if ( jNew < column )
						{
							typeNext = gameField [ i, jNew ].type;
						}
					}
					return winItem; 
				}
				j = j + jNew;
			}

		}
		return winItem; 
	}


	private static List<Item> FindWinItemOnVertical ( int startColumn, int endColumn )
	{
		List<Item> winItem = new List<Item>();

		for ( int i = startColumn; i < endColumn; i++ ) 
		{
			int j = 0;
			int jNew = 1;

			while ( j < line - 2 )
			{				
				int type_1 = gameField [ j, i ].type;
				int type_2 = gameField [ j + 1, i ].type;
				int type_3 = gameField [ j + 2, i ].type;

				if ( type_1 == type_2 && type_2 == type_3 )
				{
					winItem.Add ( gameField [ j, i ] );
					winItem.Add ( gameField [ j + 1, i ] );
					winItem.Add ( gameField [ j + 2, i ] );

					jNew = j + 3;
					int typeNext = -1;

					if ( jNew < line )
					{
						typeNext = gameField [ jNew, i ].type;
					}

					while ( type_1 == typeNext && jNew < line ) 
					{
						winItem.Add ( gameField [ jNew, i ] );
						jNew = jNew + 1;

						if ( jNew < line )
						{
							typeNext = gameField [ jNew, i ].type;
						}
					}
					return winItem; 
				}
				j = j + jNew;
			}
		}
		return winItem; 
	}


	public static int[] GettingItemPositionByID ( int curItemID )
	{
		int[] position = new int[ 2 ]; // 0 - line, 1 - column 

		for ( int i = 0; i < ItemInfo.line; i++ ) 
		{
			for ( int j = 0; j < ItemInfo.column; j++ ) 
			{
				if ( gameField [ i, j ].ID == curItemID )
				{
					position [ 0 ] = i;
					position [ 1 ] = j;
					return position;
				}
			}
		}

		return null;
	}




}
