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

namespace Database_9
{
    class MainClass
    {
        class Student
        {
            public ObjectId Id { get; set; }
            public Name name { get; set; }
            public int age { get; set; }
            public MongoDBRef cathedra { get; set; }
            public string group { get; set; }
            public string[] languages { get; set; }
            public int year_of_entry { get; set; }
            public MongoDBRef study_program { get; set; }
            public Grade[] grades { get; set; }
        }

        class Name
        {
            public string first { get; set; }
            public string last { get; set; }
        }

        class Grade
        {
            public MongoDBRef Id { get; set; }
            public int semester_grade { get; set; }
            public int exam_grade { get; set; }
            public int grade { get; set; }
        }

        public static void Task3()
        {
            string[] lang = { "українська", "англійська", "німецька" };
            Student student = new Student { age = 17, languages = lang, year_of_entry = 2022, group = "Група9" };
            student.name = new Name { first = "Микола", last = "Шкляренко" };
            student.cathedra = new MongoDBRef("cathedras", new ObjectId("6411a53890c3d7573a72bbf8"));
            student.study_program = new MongoDBRef("subjects", new ObjectId("6411a53890c3d7573a72bbfd"));
            Console.WriteLine(student.ToJson());
        }

        public static void Task4()
        {
            BsonDocument name = new BsonDocument
            {
                { "first", "Микола" },
                { "last", "Шкляренко" }
            };
            string[] lang = { "українська", "англійська", "німецька" };
            BsonDocument doc = new BsonDocument
            {
                { "name", name },
                { "age", 17 },
                { "cathedra", new BsonDocument { ["$id"] = new ObjectId("63fe6e7416a66da7029c6f4d"), ["$ref"] = "cathedras" } },
                { "group", "Група9" },
                { "languages", new BsonArray { "українська", "англійська", "німецька" } },
                { "year_of_entry", 2022 },
                { "study_program", new BsonDocument { ["$id"] = new ObjectId("6411a53890c3d7573a72bbfd"), ["$ref"] = "study_programs" } }
            };
            Student student = BsonSerializer.Deserialize<Student>(doc);
            Console.WriteLine(student.ToJson());
        }

        public static void Task5()
        {
            string[] lang = { "українська", "англійська", "німецька" };
            Student student = new Student { age = 17, languages = lang, year_of_entry = 2022, group = "Група9" };
            student.name = new Name { first = "Микола", last = "Шкляренко" };
            student.cathedra = new MongoDBRef("cathedras", new ObjectId("63fe6e7416a66da7029c6f4d"));
            student.study_program = new MongoDBRef("study_programs", new ObjectId("6411a53890c3d7573a72bbfd"));
            BsonDocument doc = student.ToBsonDocument();
            Console.WriteLine(doc);
        }

        public static void Main(string[] args)
        {
            string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            MongoClient client = new MongoClient(con);
            GetDatabaseNames(client).GetAwaiter();
            Console.ReadLine();
            GetCollectionsNames(client).GetAwaiter();
            Console.ReadLine();
            Task3();
            Console.ReadLine();
            Task4();
            Console.ReadLine();
            Task5();
            Console.ReadLine();
        }

        private static async Task GetDatabaseNames(MongoClient client)
        {
            using (var cursor = await client.ListDatabasesAsync())
            {
                var databaseDocuments = await cursor.ToListAsync();
                foreach (var databaseDocument in databaseDocuments)
                {
                    Console.WriteLine(databaseDocument["name"]);
                }
            }
        }

        private static async Task GetCollectionsNames(MongoClient client)
        {
            using (var cursor = await client.ListDatabasesAsync())
            {
                var dbs = await cursor.ToListAsync();
                foreach (var db in dbs)
                {
                    Console.WriteLine("У базi даних {0} є наступнi колекцiї:", db["name"]);
                    IMongoDatabase database = client.GetDatabase(db["name"].ToString());
                    using (var collCursor = await database.ListCollectionsAsync())
                    {
                        var colls = await collCursor.ToListAsync();
                        foreach (var col in colls)
                        {
                            Console.WriteLine(col["name"]);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}