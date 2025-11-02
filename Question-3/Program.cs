namespace Question_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //create empty priority queue for clinic
            PriorityQueue<string, int> clinicQueue = new PriorityQueue<string, int>();

            //********testing
            /*clinicQueue.Enqueue("Sune", 1);
            clinicQueue.Enqueue("Francois", 3);
            clinicQueue.Enqueue("Jurie", 0);
            clinicQueue.Enqueue("Monje", 1);
            clinicQueue.Enqueue("Daniel", 2);

            int count = clinicQueue.Count;
            Console.WriteLine(count);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{clinicQueue.Dequeue()}");
            }*/

            while (true)
            {
                //Nurse queue UI printed to terminal
                Console.WriteLine($"\n------------Clinc Patient Queue------------\n" +
                    $"1. Add a Patient to the Queue\n" +
                    $"2. Call Patient in front of queue\n" +
                    $"3. Stop Application");

                Console.Write($"Please select a option (1-3): "); //ask for user input as to what they want to do
                int userInput = 0;

                try
                {
                    userInput = Convert.ToInt32(Console.ReadLine());// tries to convert user input from string to integer
                }
                catch (Exception) //prints error message if user input not an integer value
                {
                    Console.WriteLine($"\n------------INVALID USER INPUT (enter an integer value from 1 to 3 only)------------");
                    continue;
                }

                if (userInput == 1) //adds patient to queue after recieving patient name and priority
                {
                    string patientName;
                    int patientUrgency;

                    while (true) //patient name loop
                    {
                        Console.Write($"\n Please enter Patient's name: ");
                        patientName = Console.ReadLine();

                        if (patientName == null || patientName.Length == 0)
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (patient name cannot be nothing)------------");
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (true) //patient ailment urgency loop
                    {
                        Console.Write($"\n Please enter Patient's ailment urgency (0 (most urgent)-4(least urgent)): ");

                        try
                        {
                            patientUrgency = Convert.ToInt32(Console.ReadLine());// tries to convert user input from string to integer
                        }
                        catch (Exception) //prints error message if user input not an integer value
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter an integer value from 0-4 only)------------");
                            continue;
                        }

                        if (patientUrgency < 0 || patientUrgency > 4)
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (patient urgency can only be a integer value from 0-4)------------");
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    clinicQueue.Enqueue(patientName, patientUrgency);
                    Console.WriteLine($"\n Patient ({patientName}) added to queue.");
                }
                else if (userInput == 2) //prints patient name (calling them) in front of queue and removes them from queue
                {
                    if (clinicQueue.Count == 0) //print message to user that queue is empty
                    {
                        Console.WriteLine($"\n------------Clinic queue is currently empty------------");
                    }
                    else
                    {
                        string patientName = clinicQueue.Peek(); //peek at first patient in queue

                        for (int i = 0; i < 3; i++) // call first in queue patient 3 times
                        {
                            Console.Write($"\n------------Calling Patient: {patientName}");
                        }

                        clinicQueue.Dequeue(); //remove patient from queue

                        Console.WriteLine($"\n Patient ({patientName}) has been removed from queue."); //print that patient has been removed from queue
                    }
                }
                else if (userInput == 3) //option to break out of loop and close program
                {
                    Console.WriteLine($"\n------------Closing Program------------");
                    break;
                }
                else //prints error message if user input not a valid integer value
                {
                    Console.WriteLine($"\n------------INVALID USER INPUT (enter an INTEGER value from 1 to 3 only)------------");
                }
            }
        }
    }
}
