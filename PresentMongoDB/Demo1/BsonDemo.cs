namespace PresentMongoDB.Demo1
{
	using System;
	using System.Linq.Expressions;

	using MongoDB.Bson;
	using MongoDB.Bson.IO;
	using MongoDB.Driver;

	public class BsonDemo
	{
		public static void InsertOne(IMongoDatabase database)
		{
			var collection = database.GetCollection<BsonDocument>("doc1");
			var document1 = new BsonDocument
				                {
					                { "name", "Deborah Mongo" }, 
									{ "country", "New Zealand" }, 
									{ "age", 33 },
					                { "info", new BsonDocument { { "x", 203 }, { "y", 332 } } }
				                };
			var task = collection.InsertOneAsync(document1);
			task.Wait();
		}

		public static void Query(IMongoDatabase database)
		{
			Expression<Func<BsonDocument, bool>> query = 
				doc => doc["age"] == 33;

			var collection = database.GetCollection<BsonDocument>("doc1");
			var result = collection
				.Find(query)
				.Limit(10)
				.ToListAsync()
				.Result;

			result.ForEach(Console.WriteLine);
		}
	}
}