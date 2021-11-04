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

    private const string MONGO_URI; // Include URI
    private const string DATABASE_NAME; // Include DB name
    private const string COLLECTION_NAME; // Include collection name
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
            currencyText.text = "Current Amount:\n\n" + currency + " Quetzal";
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
            currencyText.text = "Current Amount:\n\n" + currency + " Quetzal";
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
