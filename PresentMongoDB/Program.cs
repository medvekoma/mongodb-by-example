namespace PresentMongoDB
{
	using MongoDB.Driver;

	class Program
	{
		static void Main(string[] args)
		{
			var client = new MongoClient("mongodb://localhost:27017");
			var database = client.GetDatabase("prezi1");
			
			//Demo1.BsonDemo.InsertOne(database);
			//Demo1.BsonDemo.Query(database);

			//Demo2.MappingDemo.InsertOne(database);
			Demo2.MappingDemo.UpdateOne(database);
			Demo2.MappingDemo.QueryAndAggregate(database);
		}
	}
}
