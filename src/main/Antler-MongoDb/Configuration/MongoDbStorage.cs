﻿using MongoDB.Driver;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.MongoDb.Configuration
{
    public class MongoDbStorage : IStorage
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private string _idPropertyName;
        private bool _recreateDatabase;

        private MongoDbStorage(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
            _idPropertyName = "Id";
            _recreateDatabase = false;
        }

        public static MongoDbStorage Use(string connectionString, string databaseName)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            
            return new MongoDbStorage(connectionString, databaseName);
        }
        
        public MongoDbStorage WithIdPropertyName(string idPropertyName)
        {
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");

            _idPropertyName = idPropertyName;
            return this;
        }

        public MongoDbStorage WithRecreatedDatabase()
        {
            _recreateDatabase = true;
            return this;
        }

        public void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");
            
            if (_recreateDatabase)
            {
                DropDatabase();
            }

            var sessionScopeFactory = new MongoDbSessionScopeFactory(_connectionString, _databaseName, _idPropertyName);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);
        }

        private void DropDatabase()
        {            
            var server = new MongoClient(_connectionString).GetServer();
            server.DropDatabase(_databaseName);            
        }
    }
}