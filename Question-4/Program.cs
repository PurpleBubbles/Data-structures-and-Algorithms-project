using System.Text.Json;
using System.Text.Json.Serialization;

namespace Question_4
{
    internal class Program
    {
        //create a hotel object that has attributes that match those of the json file data
        public class Hotel
        {
            public int id { get; set; }
            public string name { get; set; }
            public double nightly_rate { get; set; }
            public int stars { get; set; }
            public int distance_from_airport { get; set; }
        }

        static void Main(string[] args)
        {
            string fileName = "q4_data.json"; //file name stored in program
            string jsonString = File.ReadAllText(fileName); //read all text located in file

            //deserialise the data of json file to a list of Hotel objects
            List<Hotel>? hotels = JsonSerializer.Deserialize<List<Hotel>>(jsonString);

            if (hotels == null)
            {
                Console.WriteLine($"Jason file is empty");
            }
            else
            {
                foreach (var hotel in hotels)
                {
                    Console.WriteLine($"Hotel: {hotel.name}");
                }

                Console.WriteLine($"{JsonSerializer.Serialize(hotels)}");
            }
        }
    }
}
