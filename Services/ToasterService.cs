using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using ToastApiReact.Models;

namespace ToastApiReact.Services
{
    public class ToasterService
    {
        private readonly IMongoCollection<Toaster> _toasters;

        // IMongoCollection interface represents a typed collection: toasters

        public ToasterService(IToastersDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);

            // the client provides the server instance to the local DB so that it can
            // perform actions on it

            var database = client.GetDatabase(settings.DatabaseName);

            // the database variable represents the client's connection to db

            _toasters = database.GetCollection<Toaster>(settings.ToastersCollectionName);

            // this connects to the actual collection, toasters
        }

        public List<Toaster> Get() =>
            _toasters.Find(toaster => true).ToList();

        public Toaster Get(string id) =>
            _toasters.Find<Toaster>(toaster => toaster.Id == id).FirstOrDefault();

        public Toaster Create(Toaster toaster)
        {
            _toasters.InsertOne(toaster);
            return toaster;
        }

        public void Update(string id, Toaster toasterIn)
        {
            FilterDefinition<Toaster> filter = Builders<Toaster>.Filter.Eq(toaster => toaster.Id, id);
            // using built in MongoDriver helpers to match id on toaster to be updated
            UpdateDefinition<Toaster> update = Builders<Toaster>.Update.Set("Time", toasterIn.Time).Set("On", toasterIn.On).Set("Heat", toasterIn.Heat);
            _toasters.FindOneAndUpdate(filter, update);      //(filter, update);

            //toaster => toaster.Id == id
        }
        public void Remove(Toaster toasterIn) =>
            _toasters.DeleteOne(toaster => toaster.Id == toasterIn.Id);

        public void Remove(string id) =>
            _toasters.DeleteOne(toaster => toaster.Id == id);
    }
}


// So the same DI (dependency injection) container that holds the
// IToasterDatabaseSettings is retrieved in the service here through
// constructor injection. This allows access to the appsettings.json values