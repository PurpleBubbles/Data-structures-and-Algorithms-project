using System.Reflection;
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

            //to dynamically be able to sort hotels list based on marker input and not have to hard code if statements to find what I want to sort by

            // changes "names" to whatever you want to sort according to (name, nightly_rate, stars or distance_from_airport)
            /*string marker = "names"; //default sort hotels by names

            PropertyInfo propertyInfo = hotels[0].GetType().GetProperty(marker);

            Console.WriteLine(propertyInfo.GetValue(hotels[0], null));*/

            if (hotels.Count != 0 || hotels != null) //check if elements are present in array
            {
                quickSort(hotels, 0, hotels.Count - 1);
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
        private static void quickSort(List<Hotel> hotels, int startIndex, int endIndex)
        {
            var pivot = hotels[endIndex].name; // pivot value is equal to the value of element in last index of list

            if (endIndex == startIndex) //termination point
            {
                return;
            }

            int indexJ = startIndex;
            int indexI = startIndex - 1;


            while (indexJ != endIndex)
            {
                if (hotels[indexJ].name.CompareTo(pivot) == -1) // j is less than pivot
                {
                    indexI++;

                    //swap i and j 
                    var tempStorage = hotels[indexI];

                    hotels[indexI] = hotels[indexJ];
                    hotels[indexJ] = tempStorage;

                }
                indexJ++;
            }

            // put pivot at index position it needs to be for all values left to be smaller and all values right to be greater or equal
            indexI++;

            //swap i and j
            var temp = hotels[indexI];
            hotels[indexI] = hotels[indexJ];
            hotels[indexJ] = temp;

            //end recursion when partition would be smaller or equal to one.
            if (endIndex - startIndex <= 2)
            {
                return;
            }

            // new indices for left and right hand of pivot
            int leftStartIndex = startIndex;
            int leftEndIndex = indexI - 1;

            int rightStartIndex = indexI + 1;
            int rightEndIndex = endIndex;

            quickSort(hotels, leftStartIndex, leftEndIndex);
            quickSort(hotels, rightStartIndex, rightEndIndex);
        }
    }
}
