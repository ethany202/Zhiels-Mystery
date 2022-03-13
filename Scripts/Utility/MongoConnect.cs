using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DnsClient;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using Steamworks;
using TMPro;

public class MongoConnect : MonoBehaviour
{

    private const string MONGO_URI = "mongodb://player:i44OVrrVaWHVcM2H@userdata-shard-00-00.fz2wm.mongodb.net:27017,userdata-shard-00-01.fz2wm.mongodb.net:27017,userdata-shard-00-02.fz2wm.mongodb.net:27017/myFirstDatabase?ssl=true&replicaSet=atlas-98aj6c-shard-0&authSource=admin&retryWrites=true&w=majority";
    private const string DATABASE_NAME = "UserDataDB";
    private const string COLLECTION_NAME = "UserDataCollection";
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> collection;


    private string id;
    private string latestName;
    private int currency;

    private List<string> ownedSkins;
    private BsonDocument pipeline;

    public TMP_Text currencyText;

    void Start()
    {
        InitializeDatabase();
        id = SteamUser.GetSteamID().ToString();
        latestName = SteamFriends.GetPersonaName();
        VerifyUser();
    }

    void OnGUI()
    {
        int retrievedCurrency = (int)pipeline.GetValue("currency");
        
        if (retrievedCurrency != currency)
        {
            currencyText.text = "Balance:  " + currency + " Q";
        }
    }


    public void VerifyUser()
    {
        if (!RetrieveUserData(id))
        {
            AddValue(id);
            SetValue(id, "currency", 1000);
            
        }
        else
        {
            currency = (int)pipeline.GetValue("currency");
            currencyText.text = "Balance:  " + currency + " Q";
        }
        SetValue(id, "latest name", latestName);
    }

    public void InitializeDatabase()
    {
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        collection = db.GetCollection<BsonDocument>(COLLECTION_NAME);
    }

    public bool RetrieveUserData(string id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("id", id);
        var doc = collection.Find(filter).FirstOrDefault();

        if (doc == null)
        {
            return false;
        }
        else
        {
            pipeline = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(doc);
            return true;
        }
    }

    public void SetValue(string id, string category, object val)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("id", id);
        var change = Builders<BsonDocument>.Update.Set(category, val);

        collection.UpdateOne(filter, change);
    }

    public void AddValue(string id)
    {
        var document = new BsonDocument { { "id", id } };
        collection.InsertOne(document);
    }

}
