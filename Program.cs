/* Adam Kassana, Carmen Tullio, Alexander Wolf
  
Assignment 3 - Program.cs
  
 - This codes expects words.txt in the same path as the current executable.
This code will house our main method, global/optional functions, and our testing code. */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace COIS3020Assignment3
{
    //This class will house our main method and subsequent methods.
    public class Program
    {
        //The main method, our code beings execution here.
        public static void Main(string[] args)
        {

            //Ensure we catch any exceptions that may arrise from our code, this is especially
            //important since we will be working with external reasources / IO

            //Who knows what could go wrong beyond the realm of what we can control...
            try { Testing(args); } catch (Exception e) { PrintInColour(e.ToString()); }

            Console.ReadLine(); //Pause the CLI.
        }


        //This method houses all of our testing code.
        public static void Testing(string[] args)
        {
            //For quicker testing, allow the user to specify their own word file.
            //Create a path variable for the file, use the default directory words.txt by default.
            string path = Environment.CurrentDirectory + "/words.txt";

            //Check that the user has specified an argument, and see if it is a valid file.
            //We must check that the file exists first, otherwise we might get a null object exception.
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                    //Change the path from using the words.txt in the executable directory by default.
                    path = args[0];
                else
                {
                    PrintInColour(string.Format("\nFailed to find file \"{0}\"", args[0]));
                //Got the exectutable name from this source, part of System in C#.
                //https://stackoverflow.com/questions/616584/how-do-i-get-the-name-of-the-current-executable-in-c
                    Console.WriteLine("Program usage: \"{0}\" {{path}}\n\nIf path is not specified, the progam will try defaulting to words.txt in the current directory.", AppDomain.CurrentDomain.FriendlyName.ToString());
                }
            }
            


            //Create our radix tree with the values from words.txt (path is in path variable).
            //This will grab words.txt from the current source directory.
            //Source: https://stackoverflow.com/questions/15653921/get-current-folder-path

            RTrie radix = new RTrie(path);
            PrintInColour("Successfully created the RTrie", ConsoleColor.Green);

            //For fun.
            //throw new Exception("alexander is cringe exception"); nou

            // Then let the user take over control of the trie
            int option;
            do
            {
                Console.WriteLine("\n=============== RTrie User Interface ===============");
                Console.WriteLine("1. Insert a string");
                Console.WriteLine("2. Remove a string");
                Console.WriteLine("3. Search for a string");
                Console.WriteLine("4. Print the RTrie");
                Console.WriteLine("5. Print the RTrie with a prefix");
                Console.WriteLine("6. Clear the command screen");
                Console.WriteLine("7. Exit (-1)");

                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            // Insert a string
                            Console.Write("\nEnter the string to add: ");
                            string add = Console.ReadLine();
                            // Insert an integer
                            Console.Write("\nEnter the integer value to add: ");
                            int val;
                            while (!int.TryParse(Console.ReadLine(), out val))
                            {
                                Console.WriteLine("That's not an integer.");
                                Console.Write("\nEnter the integer value to add: ");
                            }
                            // Insert the node into the trie.
                            bool ins = radix.Insert(add, val);
                            if (ins == true)
                            {
                                PrintInColour("Successfully inserted the node.", ConsoleColor.Green);
                            }
                            break;

                        case 2:
                            // Remove a string
                            Console.Write("\nEnter the string to remove: ");
                            string rem = Console.ReadLine();
                            bool ove = radix.Remove(rem);
                            if (ove == true)
                            {
                                PrintInColour("Successfully removed the node.", ConsoleColor.Green);
                            }
                            else
                            {
                                PrintInColour("Sorry, but the node could not be removed for some reason.");
                            }
                            break;

                        case 3:
                            // Search for a string
                            Console.Write("\nEnter the string to search: ");
                            string sea = Console.ReadLine();
                            int rch = radix.Search(sea);
                            if (rch == -1)
                            {
                                PrintInColour("Sorry, but the node could not be found.");
                            }
                            else
                            {
                                PrintInColour("Value: " + rch, ConsoleColor.Green);
                            }
                            break;

                        case 4:
                            // Print the RTrie
                            radix.Print();
                            break;

                        case 5:
                            // Print the RTrie by a prefix
                            Console.Write("\nEnter the string to print from: ");
                            string pre = Console.ReadLine();
                            radix.PrefixMatch(pre);
                            break;

                        case 6:
                            // Clear the command screen
                            Console.Clear();
                            Console.WriteLine("Command screen cleared.");
                            break;

                        case 7:
                            // Exit
                            Console.WriteLine("Exiting RTrie User Interface...");
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer option.");
                    Console.ReadLine(); // Consume the invalid input
                }

            } while (option != 7);
            PrintInColour("Exited the User Interface, have a good day!", ConsoleColor.Green);
        }




        //This method is optional, but allows us to print text in a specified colour.
        public static void PrintInColour(string key, ConsoleColor clr = ConsoleColor.Red)
        {
            Console.ForegroundColor = clr;
            Console.WriteLine(key);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
