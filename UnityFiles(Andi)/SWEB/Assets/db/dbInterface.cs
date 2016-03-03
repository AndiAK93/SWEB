/*
 * SQLITE3 database connection
 * 
 * usage example:
 * 
	dbInterface dbI = new dbInterface ();
	card_t c =  dbI.getRandomCard();

	switch(c.cardType_)
	{
	case 1:
		actionCard_t ac = (actionCard_t)c;
		ac.print ();
		break;
	case 2:
		LVCard_t lvc = (LVCard_t)c;
		lvc.print ();
		break;
	case 3: 			
		knowledgecard_t kc = (knowledgecard_t)c;
		kc.print ();
		break;
	}	
 *
 *
 */


using UnityEngine;
using System.Collections;

using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;

public class card_t {
	public int id_;
	public string name_;
	public string image_;
	public int cardType_;
}

public class actionCard_t : card_t {
	public string description_;
	public int cost_; // amount of ECTS to play this card (Actioncards)
	public string effectType_;
	public int effectValue_;
	public void print() {
		Debug.Log ("id_=" + id_ + " name_=" + name_ + " image_=" + image_ + " cardType_=" + cardType_ + "\n" +
		" description_=" + description_ + " cost_=" + cost_ + " effectType_=" + effectType_ + " effectValue_=" + effectValue_);
	}
}

public class knowledgecard_t :card_t {
	public string effectType_;
	public int effectValue_;
	public int attack_;
	public int defense_;
	public void print() {
		Debug.Log ("id_=" + id_ + " name_=" + name_ + " image_=" + image_ + " cardType_=" + cardType_ + "\n" +
			" attack_=" + attack_ + " defense_=" + defense_ + " effectType_=" + effectType_ + " effectValue_=" + effectValue_);
	}
}

public class LVCard_t : card_t {
	public int startRound_;
	public int duration_;
	public int ECTSReward_;
	public int[] CardRewardID_;
	public void print() {
		Debug.Log ("id_=" + id_ + " name_=" + name_ + " image_=" + image_ + " cardType_=" + cardType_ + "\n" +
			" startRound_=" + startRound_ + " duration_=" + duration_ + " ECTSReward_=" + ECTSReward_ + " CardRewardID_[0]=" + CardRewardID_[0]
			+ " CardRewardID_[1]=" + CardRewardID_[1]);
	}
}

public class dbInterface {

	string dbNameFileName_ = "SWEB.db";
	// string dbNameFileName_ = "test.db";

	// internal test only
	public void initializeDB() {
		string pathDDL = Application.dataPath + "/db/sweb.ddl";
		string pathDML = Application.dataPath + "/db/sweb.dml";

		if (!File.Exists(Application.dataPath + "/db/" + dbNameFileName_) ) {
			string createDB = "sqlite3 " + Application.dataPath + "/db/" + dbNameFileName_;
			Debug.Log (createDB);
			executeStatement (createDB);
			executeFile (pathDDL);
			executeFile (pathDML);
		}
	}

	// internal test only
	private void executeFile(string path) {
		if(!File.Exists(path))
		{
			Debug.Log("DDL does not exist");
			return;
		}

		string[] read = File.ReadAllLines (path);

		for(uint i = 0; i < read.Length; i++) {
			executeStatement (read [i]);
			Debug.Log (read [i]);
		}			
	}

	// internal test only
	private void executeStatement(string statement) {
		string conn = "URI=file:" + Application.dataPath + "/db/" + dbNameFileName_;

		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open();
		IDbCommand dbcmd = dbconn.CreateCommand();

		dbcmd.CommandText = statement;
		IDataReader reader = dbcmd.ExecuteReader();

		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;
	}

	/*
	 * returns a random none LV card 
	 */
	public card_t getRandomCard() {
		return getCard (0);
	}

	/*
	 * get a card by its ID
	 * @param id: id of the card you want to get
	 * @note a id of 0 will get you a random none LV card (e.g. drawing from deck)
	 */
	public card_t getCard(int id) {

		string conn = "URI=file:" + Application.dataPath + "/db/" + dbNameFileName_;
		string sqlQuery;

		if (id == 0) {
			sqlQuery = "SELECT * FROM card WHERE card.cardType != 3 ORDER BY RANDOM() LIMIT 1";
		} else {
			sqlQuery = "SELECT * FROM card WHERE card.id=" + id;
		}

		string card_name, card_image;
		int card_type = 0;
		int card_id = 0;

		do {

			IDbConnection dbconn;
			dbconn = (IDbConnection)new SqliteConnection (conn);
			dbconn.Open (); //Open connection to the database.
			IDbCommand dbcmd = dbconn.CreateCommand ();

			dbcmd.CommandText = sqlQuery;
			IDataReader reader = dbcmd.ExecuteReader ();
			while (reader.Read ()) {
				card_id = reader.GetInt32 (0);
				card_name = reader.GetString (1);
				card_image = reader.GetString (2);
				card_type = reader.GetInt32 (3);

				Debug.Log ("card_id=" + card_id + " card_name=" + card_name + " card_image=" + card_image + " card_type=" + card_type);
			}

			reader.Close ();
			reader = null;
			dbcmd.Dispose ();
			dbcmd = null;
			dbconn.Close ();
			dbconn = null;

		} while ((card_id == 43 || card_id == 44 || card_id == 46 || card_id == 47 || card_id == 51 || card_id == 50 || card_id == 7) && id == 0);


		card_t card = new card_t();

		switch(card_type)
		{
		case 1:
			card = getActionCard (card_id);
			break;
		case 2:
			card = getLVCard (card_id);
			break;
		case 3:
			card = getKnowledgeCard (card_id);
			break;
		default:
			break;
		}


		return card;
	}


	private actionCard_t getActionCard(int id) {
		string query = "SELECT " +
							"card.id, " +
							"card.name, " +
							"card.image, " +
							"actioncard.description, " +
							"actioncard.cost, " +
							"effecType.effect, " +
							"effect.effectValue " +
						"FROM card " +
							"INNER JOIN actioncard ON card.id = actioncard.id " +
							"INNER JOIN effect ON actioncard.effect = effect.id " +
							"INNER JOIN effecType ON effecType.id = effect.effectType " +
						"WHERE card.id =" + id;


		actionCard_t card = new actionCard_t ();
		string conn = "URI=file:" + Application.dataPath + "/db/" + dbNameFileName_;

		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();

		dbcmd.CommandText = query;
		IDataReader reader = dbcmd.ExecuteReader();
		while(reader.Read())
		{
			card.id_ = reader.GetInt32 (0);
			card.name_ = reader.GetString (1);
			card.image_ = reader.GetString (2);
			card.description_ = reader.GetString (3);
			card.cost_ = reader.GetInt32 (4);
			card.effectType_ = reader.GetString (5);
			card.effectValue_ = reader.GetInt32 (6);
			card.cardType_ = 1;
		}

		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;


		return card;
	}
		

	private LVCard_t getLVCard(int id) {
		string query = 	"SELECT " +
							"card.id, " +
							"card.name, " +
							"card.image, " +
							"LVCard.startRound, " +
							"LVCard.duration, " +
							"LVCard.ECTSReward, " +
							"cardReward.rewardCard " +
						"FROM card " +
							"INNER JOIN LVCard ON card.id = LVCard.id " +
							"INNER JOIN cardReward ON card.id = cardReward.sourceCard " +
						"WHERE card.id =" + id;


		LVCard_t card = new LVCard_t ();
		string conn = "URI=file:" + Application.dataPath + "/db/" + dbNameFileName_;

		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();

		dbcmd.CommandText = query;
		IDataReader reader = dbcmd.ExecuteReader();
		int i = 0;
		card.CardRewardID_ = new int[2]; // two possible cards

		while(reader.Read())
		{
			card.id_ = reader.GetInt32 (0);
			card.name_ = reader.GetString (1);
			card.image_ = reader.GetString (2);
			card.startRound_ = reader.GetInt32 (3);
			card.duration_ = reader.GetInt32 (4);
			card.ECTSReward_ = reader.GetInt32 (5);
			card.CardRewardID_[i] = reader.GetInt32 (6);
			card.cardType_ = 2;
			i++;
            if (i >= 2)
                break;
		}

		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;


		return card;
	}
		
	private knowledgecard_t getKnowledgeCard(int id) {
		string query = "SELECT " +
							"card.id, " +
							"card.name, " +
							"card.image, " +
							"knowledgecard.attack, " +
							"knowledgecard.defense, " +
							"effecType.effect, " +
							"effect.effectValue " +
						"FROM card " +
							"INNER JOIN knowledgecard ON card.id = knowledgecard.id " +
							"LEFT JOIN effect ON knowledgecard.effect = effect.id " +
							"LEFT JOIN effecType ON effecType.id = effect.effectType " +
						"WHERE card.id =" + id;


		knowledgecard_t card = new knowledgecard_t ();
		string conn = "URI=file:" + Application.dataPath + "/db/" + dbNameFileName_;

		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();

		dbcmd.CommandText = query;
		IDataReader reader = dbcmd.ExecuteReader();
		while(reader.Read())
		{
			card.id_ = reader.GetInt32 (0);
			card.name_ = reader.GetString (1);
			card.image_ = reader.GetString (2);
			card.attack_ = reader.GetInt32 (3);
			card.defense_ = reader.GetInt32 (4);
			card.effectType_ = reader.GetString (5);
			card.effectValue_ = reader.GetInt32 (6);
			card.cardType_ = 3;
		}

		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;


		return card;
	}

}
