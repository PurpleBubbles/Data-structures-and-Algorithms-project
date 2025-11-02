using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Question_4
{
    public class Marker
    {
        //Please change "name" to any of the predefined ssort by criteria (name, nightly_rate, stars, distance_from_airport)
        public string marker = "name"; //default sort hotels by names
    }

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

            //instantiate a string called marker that is equal to a new Marker object with an attribute marker
            string marker = new Marker().marker;

            if (hotels.Count != 0 || hotels != null) //check if elements are present in array
            {
                Console.WriteLine($"\n-----------Hotels sorted by {marker}-----------");

                quickSort(marker, hotels, 0, hotels.Count - 1);
            }
            else
            {
                Console.WriteLine($"No elements present in array");
            }

            foreach (var hotel in hotels)
            {
                Console.WriteLine($"Hotel: {hotel.name}");
            }
        }
        private static void quickSort(string marker, List<Hotel> hotels, int startIndex, int endIndex)
        {

            if (endIndex <= startIndex) //termination point
            {
                return;
            }

            int indexJ = startIndex;
            int indexI = startIndex - 1;


            while (indexJ < endIndex)
            {
                int compareResult = 0;

                if (marker == "name")
                {
                    compareResult = hotels[indexJ].name.CompareTo(hotels[endIndex].name);
                }
                else if (marker == "nightly_rate")
                {
                    compareResult = hotels[indexJ].nightly_rate.CompareTo(hotels[endIndex].nightly_rate);
                }
                else if (marker == "stars")
                {
                    compareResult = hotels[indexJ].stars.CompareTo(hotels[endIndex].stars);
                }
                else if (marker == "distance_from_airport")
                {
                    compareResult = hotels[indexJ].distance_from_airport.CompareTo(hotels[endIndex].distance_from_airport);
                }
                else
                {
                    Console.WriteLine($"\n-----------Error occured, incorrect marker given, it does not match exsiting sort by criteria-----------");
                    return;
                }

                if (compareResult == -1) // j is less than endIndex value
                {
                    indexI++;

                    //swap i and j 
                    var tempStorage = hotels[indexI];

                    hotels[indexI] = hotels[indexJ];
                    hotels[indexJ] = tempStorage;

                }
                indexJ++; //j not less than endIndex value
            }

            // put pivot at index position it needs to be for all values left to be smaller and all values right to be greater or equal
            indexI++;

            //swap i and j
            var temp = hotels[indexI];
            hotels[indexI] = hotels[indexJ];
            hotels[indexJ] = temp;

            // new indices for left and right hand of pivot
            int leftStartIndex = startIndex;
            int leftEndIndex = indexI - 1;

            int rightStartIndex = indexI + 1;
            int rightEndIndex = endIndex;

            quickSort(marker, hotels, leftStartIndex, leftEndIndex);
            quickSort(marker, hotels, rightStartIndex, rightEndIndex);
        }
    }
}
