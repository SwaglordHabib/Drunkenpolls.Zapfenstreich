using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coravel.Invocable;
using Drunkenpolls.MangoMaracuja;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Drunkenpolls.Zapfenstreich
{
    public class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string Role { get; set; }
        public string connection { get; set; }
    }
    public class Game
    {
        public ObjectId _id { get; set; }
        public string Pin { get; set; }
        // 2020-06-11T22:55:07.784Z
        public string Creation { get; set; }
        public List<Player> Player { get; set; }
    }


    public class Zapfenstreich : IInvocable
    {
        private readonly IDrunkenpollsDatabaseSettings _databaseSettings;

        public Zapfenstreich(IOptions<DrunkenpollsDatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }


        public Task Invoke()
        {
            Console.WriteLine("Zapfenstreich Invoked.");

            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            IMongoCollection<Game> collection = database.GetCollection<Game>(_databaseSettings.DrunkenpollsCollectionName);

            var gameToRemove = collection.Find(Builders<Game>.Filter.Empty).ToList();
            gameToRemove = gameToRemove.FindAll(x => x.Creation != null ? DateTime.Parse(x.Creation).AddHours(1) < DateTime.Now : true);

            foreach (var item in gameToRemove)
            {
                collection.DeleteOne(Builders<Game>.Filter.Eq(x => x._id, item._id));
            }

            Console.WriteLine(string.Format("{0} items have been removed.", gameToRemove.Count));

            return Task.CompletedTask;
        }
    }

}