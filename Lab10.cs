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
        class Student
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            [BsonElement("Student Name")]
            public Name name { get; set; }
            [BsonIgnoreIfDefault]
            public int age { get; set; }
            [BsonIgnoreIfNull]
            public MongoDBRef cathedra { get; set; }
            public string group { get; set; }
            public string[] Languages { get; set; }
            [BsonRepresentation(BsonType.String)]
            public int year_of_entry { get; set; }
            [BsonIgnore]
            public MongoDBRef study_program { get; set; }
            [BsonIgnore]
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

        class Subject
        {
            [BsonId]
            public string Name { get; set; }
            public string Control { get; set; }
        }

        public static void Main(string[] args)
        {
            string con = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            MongoClient client = new MongoClient(con);

            var conventionPack = new ConventionPack();
            conventionPack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            Subject subject = new Subject { Name = "SubjectTest", Control = "Екзамен" };
            Console.WriteLine(subject.ToJson());
            Console.ReadLine();

            BsonClassMap.RegisterClassMap<Student>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(p => p.year_of_entry).SetElementName("year of entry");
            });

            string[] lang = { "українська", "англійська", "німецька" };
            Student student = new Student { Id = ObjectId.GenerateNewId().ToString(), Languages = lang, year_of_entry = 2022, group = "Група9" };
            student.name = new Name { first = "Микола", last = "Шкляренко" };
            student.study_program = new MongoDBRef("study_programs", new ObjectId("6411a53890c3d7573a72bbfd"));

            BsonDocument doc = student.ToBsonDocument();
            Console.WriteLine(doc);
            Console.ReadLine();
        }
    }
}