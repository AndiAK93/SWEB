using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;

public enum CardOrientation { Vertical, Horizontal };

public class Card : MonoBehaviour
{
	public Sprite[] ak_faces_;
	public Sprite[] lv_faces_;
	public Sprite[] wk_faces_;

	Sprite card_face_;
	public Sprite card_back_;

	int card_id_;
	int card_face_id_;

	CardOrientation card_orientation_;

	string name;
	string card_type_;
	string description;

	// Use this for initialization
	void Start()
	{
	
	}

	void Awake()
	{

	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public void applyProperties(int card_id)
	{
		string conn = "URI=file:" + Application.dataPath + "/Database/GameCards.s3db"; //Path to database.
		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "SELECT * FROM GameCards WHERE Card_ID=" + card_id;
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();
		while(reader.Read())
		{
			card_id_ = reader.GetInt32(0);
			card_type_ = reader.GetString(1);
			card_face_id_ = reader.GetInt32(2);
			
			Debug.Log( "card_id = "+card_id+" card_type = "+card_type_);
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;

		switch(card_type_)
		{
		case "AK":
			card_face_ = ak_faces_[card_face_id_];
			card_orientation_ = CardOrientation.Vertical;
			break;
		case "LV": 
			card_face_ = lv_faces_[card_face_id_]; 
			card_orientation_ = CardOrientation.Horizontal;
			break;
		case "WK": 
			card_face_ = wk_faces_[card_face_id_]; 
			card_orientation_ = CardOrientation.Vertical;
			break;
		default: 
			Debug.Log("Card.cs: unknown type?");
			break;
		}
	}

	public Sprite getCardFace()
	{
		return card_face_;
	}
}
