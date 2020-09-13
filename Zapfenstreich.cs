using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Drunkenpolls.Bar;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Drunkenpolls.Zapfenstreich
{
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
            gameToRemove = gameToRemove.FindAll(x => x.Creation == null
                                                     || DateTime.Parse(x.Creation).AddHours(1) < DateTime.Now);

            foreach (var item in gameToRemove)
            {
                collection.DeleteOne(Builders<Game>.Filter.Eq(x => x.Id, item.Id));
            }

            Console.WriteLine(string.Format("{0} items have been removed.", gameToRemove.Count));

            return Task.CompletedTask;
        }
    }

}