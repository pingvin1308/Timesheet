using Nest;
using System;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var node = new Uri("http://localhost:9210");
            var settings = new ConnectionSettings(node)
                .DefaultIndex("people");
            
            var client = new ElasticClient(settings);

            var person = new Person
            {
                Id = 1,
                FirstName = "Martijn",
                LastName = "Laarman"
            };

            var indexResponse = client.IndexDocument(person);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
