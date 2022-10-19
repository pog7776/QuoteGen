using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program {
    public static void Main(string[] args) {
        while(true) {
            Console.WriteLine("Generating");
            Quote q = GetQuoteAsync().Result;
            Quote q2 = GetQuoteAsync().Result;
            
            if(args.Length > 0 && args[0] == "f") {
                Console.WriteLine("=======================");
                Console.WriteLine(q.content + " - " + q.author);
                Console.WriteLine(q2.content + " - " + q2.author);
            }
            
            Console.WriteLine("\n\n=======================\n");
            Console.Write(q.SplitAtRandom(true));
            Console.WriteLine(" " + q2.SplitAtRandom(false));
            int random = new Random().Next(0, 3);
            //Console.WriteLine(random);
            if(random == 0) {
                Console.WriteLine("- " + q.author);
            } else if(random == 1) {
                Console.WriteLine("- " + q2.author);
            } else {
                Console.WriteLine("- " + q.author + " and " + q2.author);
            }
            Console.WriteLine("\n=======================\n");
            Console.WriteLine("Press any key to generate another.");
            Console.ReadKey();
            Console.WriteLine();
        }
    }

    public static async Task<Quote> GetQuoteAsync() {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.quotable.io/random"),
            Headers =
            {
                { "Accept", "application/json" },
            },
        };

        using (var response = await client.SendAsync(request))
        {            
            response.EnsureSuccessStatusCode();
            //var body = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsStringAsync();
            var jsonData = (JObject)JsonConvert.DeserializeObject(result);
            var message = jsonData["content"].Value<string>();

            //Console.WriteLine(message);
            //return message;
            return JsonConvert.DeserializeObject<Quote>(result);
        }
    }

    public class Quote {
        public string content { get; set; }
        public string author { get; set; }

        public Quote(string content, string author) {
            this.content = content;
            this.author = author;
        }

        public int WordCount() {
            return content.Split(' ').Length;
        }

        public string SplitAtRandom(bool first) {
            string[] words = content.Split(' ');
            //int index = new Random().Next(1, (int)Math.Ceiling(words.Length/1.5));
            //int index = new Random().Next(1, words.Length/2);
            if(first) {
                // Get first half
                int index = new Random().Next(1, words.Length/2);
                return string.Join(" ", words.ToList<string>().Take(index));
            } else {
                int index = new Random().Next(words.Length/2, words.Length-1);
                return string.Join(" ", words.ToList<string>().Skip(index));
            }
        }
    }
}