using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogDataContext
    {
        public CatalogDataContext(IMongoClient mongoClient, ICatalogDatabaseSettings catalogDatabaseSettings)
        {
            var database = mongoClient.GetDatabase(catalogDatabaseSettings.DatabaseName);
            Products = database.GetCollection<Product>(catalogDatabaseSettings.CollectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
