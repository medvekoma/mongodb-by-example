namespace PresentMongoDB.Demo2
{
	using System;
	using System.Linq;

	using MongoDB.Bson;
	using MongoDB.Bson.Serialization;
	using MongoDB.Driver;

	public class MappingDemo
	{
		static MappingDemo()
		{
			BsonClassMap.RegisterClassMap<Invoice>(map =>
			{
				map.AutoMap();
				map.SetIdMember(map.GetMemberMap(invoice => invoice.Id));
				map.GetMemberMap(invoice => invoice.Lines).SetElementName("Items");
			});
			
		}

		private static Invoice GetInvoice()
		{
			return new Invoice()
			{
				Id = "INV-001",
				Buyer = "Joe",
				Date = DateTime.Today,
				PayableInDays = 30,
				Lines = new[]
						    { 
							    new InvoiceLine() { Name = "Learning MongoDB", Price = 12.99M }, 
							    new InvoiceLine() { Name = "MongoDB in Action", Price = 22.99M }
						    }
			};
		}

		public static void InsertOne(IMongoDatabase database)
		{
			var collection = database.GetCollection<Invoice>("doc3");
			var invoice = GetInvoice();

			collection.InsertOneAsync(invoice).Wait();
		}

		public static void Query(IMongoDatabase database)
		{
			var collection = database.GetCollection<Invoice>("doc3");

			var invoices = collection
				.Find(inv => inv.Buyer == "Joe")
				.Limit(10)
				.ToListAsync()
				.Result;

			invoices.ForEach(invoice => Console.WriteLine(invoice.ToJson()));
		}

		public static void UpdateOne(IMongoDatabase database)
		{
			var collection = database.GetCollection<Invoice>("doc3");
			var invoice = GetInvoice();

			var options = new FindOneAndReplaceOptions<Invoice>()
				              {
					              IsUpsert = true
				              };
			collection
				.FindOneAndReplaceAsync<Invoice>(item => item.Id == invoice.Id, invoice, options)
				.Wait();
		}

		public static void QueryAndAggregate(IMongoDatabase database)
		{
			var collection = database.GetCollection<Invoice>("doc3");

			var items = collection
				.Aggregate()
				.Match(inv => inv.Buyer == "Joe")
				.Group(inv => new{inv.Buyer}, g => g.Key)
				.ToListAsync()
				.Result;

			items.ForEach(item => Console.WriteLine(item.ToJson()));
		}
	}
}