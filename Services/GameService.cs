using System;
using System.Collections.Generic;
using Drunkenpolls.Bar;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Zapfenstreich.Services
{
    public interface IGameService
    {
        List<Game> GetAllInactive();
        void Remove(string id);

    }
    public class GameService : IGameService
    {
        private readonly IMongoCollection<Game> _Games;

        public GameService(IOptions<DrunkenpollsDatabaseSettings> databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = client.GetDatabase(databaseSettings.Value.DatabaseName);

            _Games = database.GetCollection<Game>(databaseSettings.Value.DrunkenpollsCollectionName);
        }

        public List<Game> GetAllInactive()
        {
            var gameToRemove = _Games.Find(Builders<Game>.Filter.Empty).ToList();
            gameToRemove = gameToRemove.FindAll(x => x.Creation == null
                                                     || DateTime.Parse(x.Creation).AddHours(1) < DateTime.Now);
            return gameToRemove;
        }

        public void Remove(string id)
        {
            _Games.DeleteOne(Builders<Game>.Filter.Eq(x => x.Id, id));
        }
    }

}