using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;

namespace Database_9
{
    class MainClass
    {
        class Subject
        {
            public ObjectId Id { get; set; }
            public string name { get; set; }
            public MongoDBRef study_program { get; set; }
            public string control { get; set; }
        }

        private static async Task SaveDocs(MongoClient client)
        {
            try
            {
                var database = client.GetDatabase("lab6");
                var collection = database.GetCollection<BsonDocument>("subjects");

                Subject subject1 = new Subject { Id = ObjectId.GenerateNewId(), name = "Предмет21", control = "Екзамен" };
                Subject subject2 = new Subject { Id = ObjectId.GenerateNewId(), name = "Предмет22", control = "Екзамен" };
                Subject subject3 = new Subject { Id = ObjectId.GenerateNewId(), name = "Предмет23", control = "Екзамен" };
                Subject subject4 = new Subject { Id = ObjectId.GenerateNewId(), name = "Предмет24", control = "Екзамен" };
                subject1.study_program = new MongoDBRef("study_programs", new ObjectId("6411a53890c3d7573a72bc00"));
                subject2.study_program = new MongoDBRef("study_programs", new ObjectId("6411a53890c3d7573a72bc00"));
                subject3.study_program = new MongoDBRef("study_programs", new ObjectId("6411a53890c3d7573a72bc00"));
                subject4.study_program = new MongoDBRef("study_programs", new ObjectId("6411a53890c3d7573a72bc00"));

                BsonDocument insert1 = subject1.ToBsonDocument();
                BsonDocument insert2 = subject2.ToBsonDocument();
                BsonDocument insert3 = subject3.ToBsonDocument();

                await collection.InsertOneAsync(insert1);
                await collection.InsertManyAsync(new[] { insert2, insert3 });
                var collection2 = database.GetCollection<Subject>("subjects");
                await collection2.InsertOneAsync(subject4);
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }

        public static void Main(string[] args)
        {
            string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            MongoClient client = new MongoClient(con);

            SaveDocs(client).GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}