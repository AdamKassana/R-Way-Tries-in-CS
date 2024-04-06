/* Adam Kassana, Carmen Tullio, Alexander Wolf
  
Assignment 3 - RTrie.cs
  
This code will house all code relating to both the RTrie and it's containing RTrieNodes.
 - The majority of this code was directly imported from my (Carmen) lab 3 submission. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;

namespace COIS3020Assignment3
{
    //This class will construct our RTrieNode.
    public class RTrieNode
    {
        //Define the variables of our RTrieNode
        //Note that the default values for each are -1 and 0 respectively.
        private int nValue = -1; //This value is -1 to represent a default value.
        private int childQuantity = 0; //This value represents the number of children of the node.
        //Getters and setters since these properties will be public, despite not really being needed.
        public int Value
        {
            get { return nValue; }
            set { nValue = value; }
        }
        public int ChildQuantity
        {
            get { return childQuantity; }
            set { childQuantity = value; }
        }
        //Create our children dictionary so we can add nodes at individual chars.
        public Dictionary<char, RTrieNode> children = new Dictionary<char, RTrieNode>();


        public RTrieNode()
        {
            Debug.WriteLine("Created a node.");
            //Note that the length of the array is related to the values used for the children.
        }
    }
    //This class will house the actual RTrie structure consistent of RTrieNodes.
    public class RTrie
    {
        private RTrieNode root = null; //It is a good idea to initialize our root even if it is null.

        public RTrie(string[] words, int value = -1)
        {
            //Create our root node, as this is the constructor.

            this.root = new RTrieNode();

            //Create a random to randomly insert values for fun
            Random rnd = new Random();

            foreach (string word in words)
            {
                //If the user has specified a value, (cannot be negative one as this is default)
                //We can insert a random value for fun. (rnd.Next is always non-negative).
                if (value == -1)
                    this.Insert(word, rnd.Next());
                else
                    this.Insert(word, value);
            }
        }

        public RTrie(string path, int value = -1)
        {
            //This constructor will take in a file path to generate an RTrie.

            //Check that the string is not null
            if (path.Equals(""))
                //We should use exceptions here, as the constructor is like a void method.
                throw new Exception("No path provided.");
            else if (!File.Exists(path))
                //Once again, throw an exception.
                throw new Exception("Failed to find file.");
            //Import all words into an array of words.
            string[] words = File.ReadAllLines(path);

            //Finally, import all words into the RTrie.
            //Create our root node, as this is the constructor.

            this.root = new RTrieNode();

            //Create a random to randomly insert values for fun
            Random rnd = new Random();

            foreach (string word in words)
            {
                //If the user has specified a value, (cannot be negative one as this is default)
                //We can insert a random value for fun. (rnd.Next is always non-negative).
                if (value == -1)
                    this.Insert(word, rnd.Next());
                else
                    this.Insert(word, value);
            }
        }

        public RTrie()
        {
            root = new RTrieNode(); //Create our new Node for the root in our tree.
        }

        //This is the PrefixSearch method, it does the same thing as print, but passes print
        //a prefix, and the print method checks each string starts with a prefix that may have been
        //specified and prints only if it starts with it if it was specified.
        public void PrefixMatch(string prefix, RTrieNode current = null)
        {
            Print("", current, prefix);
        }


        //This method allows the user to skip needing to type a null string.
        public void Print(RTrieNode current = null)
        {
            //The end user should not need to pass a key.
            Print("", current);
        }

        //This method will perform a traversal on the treen and print all nodes. heck yeah bonus stuff
        //Do not ask me for the time complexity, because iterating through the alphabet (26)
        //letters is really inefficient, but i'm going by the sample code from week 10.

        //This is not my fault, i'm just doing what i'm told, so don't get mad at me ok?
        private void Print(string key, RTrieNode current = null, string prefix = "")
        {
            //Check and see if the user passed us something
            if (current == null)
            {
                current = root;
            }

            //Check to ensure that the value was properly set and not default.
            //For prefix search, we should ensure the word either starts with the prefix, or the user
            //has not specified one.
            if (current.Value != -1 && (prefix.Equals("") || key.StartsWith(prefix.ToLower())))
            {
                Console.WriteLine("{2} -> {0} : {1}", current.Value, current.ChildQuantity, key);
            }

            //We might be able to use the childquanity but I am using count because it feels safer based
            //on the contents of the loop.
            for (int i = 0; i < current.children.Count; i++)
            {
                try
                {
                    //Get the current char as efficiently as possible.
                    char c = current.children.Keys.ElementAt(i);
                    //Efficiently call upon that character based on the bonus submission but more efficiently.
                    Print(key + c, current.children[c], prefix);
                }
                catch (Exception ignored)
                {
                    //This is mostly just leftover from my bonus submission, but it cannot hurt to keep it here.

                    //Since we used a dictionary improperly there's a good chance running this garbage is gonna fail,
                    //So just ignore ignore the errors it's fine
                    //Even though there's likely none to begin with :)
                }
            }

        }


        //Begin implementing the standard methods.

        //This method will be used for inserting a value for a key.
        //It will return true if the key was sucessfully inserted.
        //The time complexity is O(L) where L is the length of our string.
        public bool Insert(string key, int value)
        {
            //For this bonus, we can only allow lowercase characters, check that the input is a lowercase character.

            //Ensure the user has input a string.
            if (key.Equals(""))
            {
                Program.PrintInColour("You must specify a non-null string as a key to insert.");
                return false;
            }

            //Don't allow the user to set the value to default, force them to remove things properly.
            if (value == -1)
            {
                Program.PrintInColour(string.Format("The key \"{0}\" was not inserted as it has a default value of (-1).))", key));
                return false;
            }
            //Convert our string to a char array and DO NOT ignore case.
            RTrieNode current = this.root; //Set our current node to be the non-static root.
            char[] chars = key.ToCharArray();

            foreach (char c in chars)
            {
                if (!current.children.ContainsKey(c))
                {
                    //We need to break new ground!
                    // *this is because the key does not yet exist*

                    current.children.Add(c, new RTrieNode()); //Add the node with the key being the char.

                }
                //The node already exists, or has just been created, continue onto the next node.
                current = current.children[c];
            }
            //Once we have iterated through all chars, we should set our value.
            //Note that the final node is stored as current.
            //Remember that we can only update the value if the current value is default. (-1)
            if (current.Value == -1)
                current.Value = value;
            else
                return false; //We don't want any further values changing considering the changes.

            //Now we must go through and increase all affected children quanities by one.
            current = root; //We should reset our root.
            foreach (char c in chars)
            {
                current.ChildQuantity++; //Increase the child quantity by one.
                current = current.children[c];
            }
            //Increase the childquantity for the final node.
            current.ChildQuantity++;

            return true;
        }

        //This method is used for searching the RTrie.
        //It will return the value or negative one if it is not found.
        //Time complexity is O(min(L, M))
        public int Search(string key)
        {
            //Check that the key was specified.
            if (key.Equals(""))
                return -1;
            char[] chars = key.ToCharArray();
            //Take the current node from the RTrie's root.
            RTrieNode current = this.root;
            //We must follow our key to the root.

            foreach (char c in chars)
            {
                if (current.children.ContainsKey(c))
                {
                    //Continue searching for our key.
                    current = current.children[c];
                }
                else
                {
                    //If the code reaches this point, the key does not exist.
                    return -1;
                }
            }
            //If we are able to escape the foreach loop without returning -1
            //The the current RTrieNode has the value we are searching for.

            //We must also ensure that the value is not the default
            //fortunately, the default is negative one, so we can
            //simply just return current.Value either way.
            return current.Value; ;
        }

        //This method will be used to remove a key from the RTrie.
        //It will return true if the key is removed successfully.
        //Time complexity is O(min(L, M))
        public bool Remove(string key)
        {
            //Ensure the user has provided a string.
            if (key.Equals(""))
            {
                //No string provided, return false.
                return false;
            }
            //This should be fun, we must once again follow the key.
            char[] chars = key.ToCharArray();
            //Define our current node as the root.
            RTrieNode current = this.root;

            foreach (char c in chars)
            {
                if (current.children.ContainsKey(c))
                {
                    //Iterate through every char to ensure the key exists.
                    current = current.children[c];
                }
                else
                {
                    //If the code reaches here, the key does not exist. We must return false.
                    return false;
                }
            }
            //Now that we have found our desired current node, we must ensure it has a value
            //which is not the default, indicating a compeleted insert.

            if (current.Value == -1)
            {
                //If the value we found was negative one, we cannot remove.
                return false;
            }

            //At this point, the current node will be the one that contains the value we want to remove.
            //Let's remove it. Remember -1 is our default.
            current.Value = -1;

            //At this point, we must trace back and remove any redundant children.
            //We do now however that the key DOES exist at this point.
            //We must do another foreach loop unfortunately.

            current = this.root;
            //Reset our root.

            //Foreach character in our key, we should check all respective nodes to see
            //if any remain revant to our tree.
            foreach (char c in chars)
            {
                //Store the parent so we can remove it's children.
                RTrieNode parent = current;
                //Iterate the loop, immediately, as we should NOT be adjusting the root.
                current = current.children[c]; //We should not ever need to try get in this instance.

                //Subtract one from the quantity of children of each related node.
                current.ChildQuantity--;
                //If the current node being inspected has no children, it can be removed.
                //I'm pretty sure the this.root check is redundant, but it's best to make sure.
                if (current.ChildQuantity <= 0 && !parent.Equals(this.root))
                //Note that this if statement realistically never reach below zero
                //But lets account for the case anyways, as the children can never be negative.
                //We must also ensure we NEVER remove the root.
                {
                    //Since we are recursively checking if the children is zero there cannot
                    //Be any dependant children remaining, we can simply remove this child and break the loop.
                    parent.children.Remove(c);
                    //It is this break which results in the O(min(L, M)) time complexity.
                    break;
                }

            }
            //Finally, we must ensure we subtract one from the root's child quantity.
            root.ChildQuantity--;
            //At this point, if the code reaches here, the key and value were sucessfully removed.
            return true;
        }

    }
}
