﻿/* cs 212 : Bingo
 * Seong Chan Cho (sc77)
 * Nov 19,2021
 * Code based from Professor Harry Plantinga's starter code*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Bingo
{
    class Program
    {
        private static RelationshipGraph rg;

        // Read RelationshipGraph whose filename is passed in as a parameter.
        // Build a RelationshipGraph in RelationshipGraph rg
        private static void ReadRelationshipGraph(string filename)
        {
            rg = new RelationshipGraph();                           // create a new RelationshipGraph object

            string name = "";                                       // name of person currently being read
            int numPeople = 0;
            string[] values;
            Console.Write("Reading file " + filename + "\n");
            try
            {
                string input = System.IO.File.ReadAllText(filename);// read file
                input = input.Replace("\r", ";");                   // get rid of nasty carriage returns 
                input = input.Replace("\n", ";");                   // get rid of nasty new lines
                string[] inputItems = Regex.Split(input, @";\s*");  // parse out the relationships (separated by ;)
                foreach (string item in inputItems)
                {
                    if (item.Length > 2)                            // don't bother with empty relationships
                    {
                        values = Regex.Split(item, @"\s*:\s*");     // parse out relationship:name
                        if (values[0] == "name")                    // name:[personname] indicates start of new person
                        {
                            name = values[1];                       // remember name for future relationships
                            rg.AddNode(name);                       // create the node
                            numPeople++;
                        }
                        else
                        {
                            rg.AddEdge(name, values[1], values[0]); // add relationship (name1, name2, relationship)

                            // handle symmetric relationships -- add the other way
                            if (values[0] == "hasSpouse" || values[0] == "hasSibling")
                                rg.AddEdge(values[1], name, values[0]);

                            // for parent relationships add child as well
                            else if (values[0] == "hasParent")
                                rg.AddEdge(values[1], name, "hasChild");
                            else if (values[0] == "hasChild")
                                rg.AddEdge(values[1], name, "hasParent");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Unable to read file {0}: {1}\n", filename, e.ToString());
            }
            Console.WriteLine(numPeople + " people read");
        }

        // Show the relationships a person is involved in
        private static void ShowPerson(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
                Console.Write(n.ToString());
            else
                Console.WriteLine("{0} not found", name);
        }

        // Show a person's Siblings
        private static void ShowSiblings(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                Console.Write("{0}'s Siblings: ", name);
                List<GraphEdge> SiblingEdges = n.GetEdges("hasSibling");
                foreach (GraphEdge e in SiblingEdges)
                {
                    Console.Write("{0} ", e.To());
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("{0} not found", name);
        }

        // Show the orphans of a Relationship Graph
        private static void ShowOrphans()
        {
            int numOrphans = 0;     //Create a counter variable to count the number of orphans
            List<GraphNode> orphanList = new List<GraphNode>();     //Create a list to store the Orphans

            foreach (GraphNode node in rg.nodes)            //Loop to go through the nodes in the graph
            {      
                List<GraphEdge> parentEdges = node.GetEdges("hasParent");
                if (parentEdges.Count == 0)
                {  
                    orphanList.Add(node);           //Add the current node to the list
                    numOrphans++;                   //Increment the orphan counter to keep in track of number of orphans
                }
            }

            string numberOrphans = Convert.ToString(numOrphans);

            foreach (GraphNode node in orphanList)      //print out each name in the orphan list
            {
                Console.Write(node.Name + '\n');
            }

            Console.Write("There are " + numberOrphans + " orphans in the GraphRelationship.");     
        }

        //List the descendants of an indvidual
        private static void ListDescendants(string name)
        {
            GraphNode node = rg.GetNode(name);              //Get the node name of the individual from the graph
            int descendant_count = 0;                       //initiate descendant count

            //If invalid search,
            if (node == null)
            {
                Console.Write("{0} not found\n", name);
            }
            //If the node has no hasChild edge,
            else if (node.GetEdges("hasChild").Count == 0)      
            {
                Console.Write("{0} has no descendants\n", name);
            }

            //Else, list out the descendants
            else
            {
                List<GraphEdge> descendantsList = node.GetEdges("hasChild");       

                List<GraphNode> nextDescendants = new List<GraphNode>();            //Create two new lists of GraphNodes, one for the current generation of descendants, and one for the following generation of descendants
                List<GraphNode> currentDescendants = new List<GraphNode>();          

                foreach (GraphEdge edge in descendantsList)                        
                {                                                                      
                    currentDescendants.Add(rg.GetNode(edge.To()));
                }

                while (currentDescendants.Count > 0)                                
                {

                    //Determining the label of descendants to write to the console
                    if (descendant_count == 0)
                    {
                        Console.Write(node.Name + " Children:\n");
                    }

                    else if (descendant_count == 1)
                    {
                        Console.Write(node.Name + " Grandchildren:\n");
                    }

                    else
                    {
                        Console.Write(node.Name);
                        for (int i = 0; i < descendant_count; i++)
                        {
                            Console.Write(" Great");
                        }
                        Console.Write(" GrandChildren:\n");

                    }

                    //For each node in the current descendants list
                    foreach (GraphNode descendant in currentDescendants)
                    {
                        Console.Write("{0}\n", descendant.Name);              //Write out the name of the descendant to the console
                        descendantsList = descendant.GetEdges("hasChild");      //Get a list of the edges hasChild from that descendant

                        foreach (GraphEdge edge1 in descendantsList)           
                        {
                            nextDescendants.Add(rg.GetNode(edge1.To()));    //Add the new next node that the edge hasChild is directing
                        }
                    }

                    currentDescendants = nextDescendants;                       //Copy the nextDescendant List to currentDescandents List
                    nextDescendants = new List<GraphNode>();                    //Clear the list
                    descendant_count++;                                         //Increment the generation counter.
                }
            }
        }


  

        // Find the shortest path of relationships between two people
        private static void Bingo(string start_person, string end_person)
        {
            //If the start and end person are the same person, inform the user
            if (start_person == end_person)
            {
                Console.Write(start_person + " and " + end_person + " are the same person.\n");
            }

            else
            {
                int depth_counter = 0;      //Variable to keep track the minimum depth to get to end_person from start_person

                Dictionary<string, Boolean> dictionary = new Dictionary<string, Boolean>();     //new Dictionary to see if a node has been visited before
                foreach (GraphNode node_ in rg.nodes)
                    dictionary.Add(node_.Name, false);

                Queue<string> q = new Queue<string>();      //Create a new queue

                dictionary[start_person] = true;            //Mark start_person as visited

                q.Enqueue(start_person);                    //Add the start_person to the queue

                //As long as the queue is not empty
                while (q.Count > 0)
                {
                    string name_one = q.Dequeue();                          //Remove the next element in the queue
                    GraphNode node_one = rg.GetNode(name_one);
                    List<GraphEdge> AdjacentEdges = node_one.GetEdges();
                    depth_counter++;                                        //Increment the depth counter as we went one depth further

                    //Check each GraphEdge coming out of the current node
                    foreach (GraphEdge edge in AdjacentEdges)
                    {

                        if (name_one == end_person)                         //If the current node and the end_person are the same, break from the While Loop
                            break;

                        if (dictionary[edge.To()] == false)
                        {
                            dictionary[edge.To()] = true;                   //Update the visited dictionary
                            q.Enqueue(edge.To());                           //Add it to the queue
                        }
                    }

                }

                List<GraphNode> list_nodes = new List<GraphNode>();        //New lists for the nodes and the edges of the path to the end_person
                List<GraphEdge> list_edges = new List<GraphEdge>();

                Dictionary<string, Boolean> dictionary2 = new Dictionary<string, Boolean>();
                foreach (GraphNode node_ in rg.nodes)
                    dictionary2.Add(node_.Name, false);

                int stack_depth = 0;                                    //Stack depth counter to keep track how far to go in the DFS
                Stack<GraphNode> stack_nodes = new Stack<GraphNode>();  //Create a new stack
                stack_nodes.Push(rg.GetNode(start_person));             //Push the start_person onto the stack

                while (true)
                {
                    while (stack_nodes.Count > 0)                               //As long as the stack is not empty
                    {
                        GraphNode node_one = stack_nodes.Pop();                 //Pop off the next element
                        list_nodes.Add(node_one);                               //Add it to the list

                        List<GraphEdge> edges_list = node_one.GetEdges();
                        list_edges.Add(edges_list[0]);

                        foreach (GraphEdge edge_two in edges_list)
                        {
                            stack_nodes.Push(rg.GetNode(edge_two.To()));       //Add each node coming off the edges to the Stack
                        }

                        stack_depth++;
                        if (stack_depth == depth_counter)                       //If the two depth counters are equal, break
                            break;
                    }

                    //Check to see if the first node in the list is the same as the start_person
                    //  and if the last node in the list is the same as the end_person
                    if (list_nodes[0] == rg.GetNode(start_person) && list_nodes[depth_counter - 1] == rg.GetNode(end_person))
                    {
                        break;
                    }
                }

                //Printing out the relationships between the start_person and the end_person
                int i = 0;
                foreach (GraphNode NODE in list_nodes)
                {
                    GraphEdge edge_label = list_edges[i];
                    Console.Write(NODE.Name + edge_label.Label + list_nodes[i + 1] + "\n");
                    i++;
                }
            }
        }

        // accept, parse, and execute user commands
        private static void commandLoop()
        {
            string command = "";
            string[] commandWords;
            Console.Write("Welcome to Seong Chan's Dutch Bingo Parlor!\n");

            while (command != "exit")
            {
                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();
                commandWords = Regex.Split(command, @"\s+");        // split input into array of words
                command = commandWords[0];

                if (command == "exit")
                    ;                                               // do nothing

                // read a relationship graph from a file
                else if (command == "read" && commandWords.Length > 1)
                    ReadRelationshipGraph(commandWords[1]);

                // show information for one person
                else if (command == "show" && commandWords.Length > 1)
                    ShowPerson(commandWords[1]);

                // show all the friends of a person
                else if (command == "siblings" && commandWords.Length > 1)
                    ShowSiblings(commandWords[1]);

                //  show all the people who are orphans (have no parents)
                else if (command == "orphans")
                    ShowOrphans();

                // show all the descendents of an individual
                else if (command == "descendants" && commandWords.Length > 1)
                    ListDescendants(commandWords[1]);

                else if (command == "bingo" && commandWords.Length > 1)
                    Bingo(commandWords[1], commandWords[2]);


                else if (command == "dump")
                    rg.Dump();

                // illegal command
                else
                    Console.Write("\nLegal commands: read [filename], dump, show [personname],\n siblings [personname], orphans, descendants [personname],\n exit\n");
            }
        }

        static void Main(string[] args)
        {
            commandLoop();
        }
    }
}