using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item  
{
	public int ID;
	public int type;
	public GameObject objectInGame;
	public bool isUsedInField;
	public bool isNeedToRemove;


	public Item ( int numb, int resource, GameObject sceneObject, bool active, bool destroy )
	{
		ID = numb;
		type = resource;
		objectInGame = sceneObject;
		isUsedInField = active;
		isNeedToRemove = destroy;
	}
}
