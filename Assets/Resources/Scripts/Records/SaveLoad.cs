using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class SaveLoad {

	public static List<int> globalRecordScore = new List<int>();
	public static List<string> globalRecordDate = new List<string>();



	public static void Save () 
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create ( Application.persistentDataPath + "/Match3.txt" );

		bf.Serialize( file, globalRecordScore );
		bf.Serialize( file, globalRecordDate );

		file.Close();
	}   


	public static void Load () 
	{
		Debug.Log ( "Application.persistentDataPath = " + Application.persistentDataPath);

		if ( File.Exists( Application.persistentDataPath + "/Match3.txt" ) ) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open( Application.persistentDataPath + "/Match3.txt", FileMode.Open );

			globalRecordScore = ( List<int> )bf.Deserialize( file );
			globalRecordDate = ( List<string> )bf.Deserialize( file );

			file.Close();
		}
	}
}