/* cs112 - Section A
 * Seong Chan Cho (sc77)
 * Program 2: Babbling - This program was designed to recieve the textfile as an input and build a random combination of words using hashtable
 * October 9th,2021*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BabbleSample
{
    /// Babble framework
    /// Starter code for CS212 Babble assignment
    public partial class MainWindow : Window
    {
        private string input;                              // input file
        private string[] words;                            // input file broken into array of words
        private int wordCount = 200;                       // number of words to babble
        private Random random_index = new Random();        // Random type to determine random number
        private string current_word = " ";                 // string of current word
        int word_num = 0;                                  // int of number of words to keep count of the word sequence
        int unique_word_num = 0;                           // int of number of unique words to keep count of the unique words sequence



        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Sample"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                textBlock1.Text = "Loading file " + ofd.FileName + "\n";
                input = System.IO.File.ReadAllText(ofd.FileName);  // read file
                words = Regex.Split(input, @"\s+");       // split into array of words
            }
        }

        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + order);
            }
        }

        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            // if the selected is order of 0
            if (orderComboBox.SelectedIndex == 0)
            {
                //clear the box prior to babble
                textBlock1.Text = " ";
                //For loop that prints between the first 200 words and the whole text file which has its length shorter than wordcount 200.
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                {
                    textBlock1.Text += " " + words[i];          //print out words
                }
            }
            // if selected is order of 1
            if (orderComboBox.SelectedIndex == 1)
            {
                Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();
                unique_word_num = 0;
                word_num = 0;
                
                // loop it to the amount of lesser value from either the set word count, which is 200, or the word length of the text file.
                for (int i = 0; i < Math.Min(wordCount, words.Length - 1); i++)
                {
                    word_num++;
                    string firstword = words[i];
                    if (!hashTable.ContainsKey(firstword))
                    {
                        hashTable.Add(firstword, new ArrayList());
                        unique_word_num++;
                    }
                    hashTable[firstword].Add(words[i + 1]);

                }

                // print the number of unique words and words sequences.
                wordNumCount.Text = " \r\n";
                wordNumCount.Text += Convert.ToString(unique_word_num) + " sequences of unique words. \r\n";
                wordNumCount.Text += Convert.ToString(word_num) + " sequences of words. \r\n";

                // start from the first one word of the text file
                current_word = words[0];
                textBlock1.Text = " " + words[0];
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                {
                    //get a random value from a specific key of the hashtable that comes after the corresponding key.
                    string random_word = Babble(hashTable);
                    textBlock1.Text += " " + random_word;
                    current_word = random_word;
                }
            }

            if (orderComboBox.SelectedIndex == 2)
            {
                Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();
                unique_word_num = 0;
                word_num = 0;

                for (int i = 0; i < Math.Min(wordCount, words.Length -2); i++)
                {
                    word_num++;
                    string first_two_words = words[0] + " " + words[1];
                    if (!hashTable.ContainsKey(first_two_words))
                    {
                        hashTable.Add(first_two_words, new ArrayList());
                        unique_word_num++;
                    }
                    hashTable[first_two_words].Add(words[i + 2]);
                }

                // print the number of unique words and words sequences.
                wordNumCount.Text = " \r\n";
                wordNumCount.Text += Convert.ToString(unique_word_num) + " sequences of unique words. \r\n";
                wordNumCount.Text += Convert.ToString(word_num) + " sequences of words. \r\n";

                // start from the first two words of the text file
                current_word = words[0] + " " + words[1];
                textBlock1.Text = " " + current_word;

                // get a random word that comes after two consecutive words from the text
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                {
                    string random_word = Babble(hashTable);
                    textBlock1.Text += " " + random_word;
                    current_word = random_word;
                }
            }

            if (orderComboBox.SelectedIndex == 3)
            {
                Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();
                unique_word_num = 0;
                word_num = 0;

                for (int i = 0; i < Math.Min(wordCount, words.Length - 3); i++)
                {
                    word_num++;
                    string first_three_words = words[0] + " " + words[1] + " " + words[2];
                    if (!hashTable.ContainsKey(first_three_words))
                    {
                        unique_word_num++;
                        hashTable.Add(first_three_words, new ArrayList());
                    }
                    hashTable[first_three_words].Add(words[i + 3]);
                }

                // print the number of unique words and words sequences.
                wordNumCount.Text = " \r\n";
                wordNumCount.Text += Convert.ToString(unique_word_num) + " sequences of unique words. \r\n";
                wordNumCount.Text += Convert.ToString(word_num) + " sequences of words. \r\n";

                // start from the first two words of the text file
                current_word = words[0] + " " + words[1] + " " + words[2];
                textBlock1.Text = " " + current_word;

                // get a random word that comes after two consecutive words from the text
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                {
                    string random_word = Babble(hashTable);
                    textBlock1.Text += " " + random_word;
                    current_word = random_word;
                }
            }
            if (orderComboBox.SelectedIndex == 4)
            {
                Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();
                unique_word_num = 0;
                word_num = 0;

                for (int i = 0; i < Math.Min(wordCount, words.Length - 4); i++)
                {
                    word_num++;
                    string first_four_words = words[0] + " " + words[1] + " " + words[2] + " " + words[3];
                    if (!hashTable.ContainsKey(first_four_words))
                    {
                        unique_word_num++;
                        hashTable.Add(first_four_words, new ArrayList());
                    }
                    hashTable[first_four_words].Add(words[i + 3]);
                }

                // print the number of unique words and words sequences.
                wordNumCount.Text = " \r\n";
                wordNumCount.Text += Convert.ToString(unique_word_num) + " sequences of unique words. \r\n";
                wordNumCount.Text += Convert.ToString(word_num) + " sequences of words. \r\n";

                // start from the first two words of the text file
                current_word = words[0] + " " + words[1] + " " + words[2] + " " + words[3];
                textBlock1.Text = " " + current_word;

                // get a random word that comes after two consecutive words from the text
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                {
                    string random_word = Babble(hashTable);
                    textBlock1.Text += " " + random_word;
                    current_word = random_word;
                }
            }
            if (orderComboBox.SelectedIndex == 5)
            {
                Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();
                unique_word_num = 0;
                word_num = 0;

                for (int i = 0; i < Math.Min(wordCount, words.Length - 5); i++)
                {
                    word_num++;
                    string first_five_words = words[0] + " " + words[1] + " " + words[2] + " " + words[3] + " " + words[4];
                    if (!hashTable.ContainsKey(first_five_words))
                    {
                        unique_word_num++;
                        hashTable.Add(first_five_words, new ArrayList());
                    }
                    hashTable[first_five_words].Add(words[i + 3]);
                }
                // print the number of unique words and words sequences.
                wordNumCount.Text = " \r\n";
                wordNumCount.Text += Convert.ToString(unique_word_num) + " sequences of unique words. \r\n";
                wordNumCount.Text += Convert.ToString(word_num) + " sequences of words. \r\n";

                // start from the first two words of the text file
                current_word = words[0] + " " + words[1] + " " + words[2] + " " + words[3] + " " + words[4];
                textBlock1.Text = " " + current_word;

                // get a random word that comes after two consecutive words from the text
                for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                {
                    string random_word = Babble(hashTable);
                    textBlock1.Text += " " + random_word;
                    current_word = random_word;
                }
            }
        }
        

        // random word generating function, gets a random value from a specific key recieved by parameter
        private string Babble(Dictionary<string, ArrayList> hashTable)
        {
            //following if statements checks if the hashtable contains the current checking word(S) before creating a random word
            //and if it does not, it will start over at the start of the text file. 
            if (orderComboBox.SelectedIndex == 1)
            {
                if (!hashTable.ContainsKey(current_word))
                { current_word = words[0]; }
            }

            if (orderComboBox.SelectedIndex == 2)
            {
                if (!hashTable.ContainsKey(current_word))
                { current_word = words[0] + " " + words[1]; }
            }

            if (orderComboBox.SelectedIndex == 3)
            {
                if (!hashTable.ContainsKey(current_word))
                { current_word = words[0] + " " + words[1] + " " + words[2]; }
            }

            if (orderComboBox.SelectedIndex == 4)
            {
                if (!hashTable.ContainsKey(current_word))
                { current_word = words[0] + " " + words[1] + " " + words[2] + " " + words[3]; }
            }

            if (orderComboBox.SelectedIndex == 5)
            {
                if (!hashTable.ContainsKey(current_word))
                { current_word = words[0] + " " + words[1] + " " + words[2] + " " + words[3] + " " + words[4]; }
            }



            //Generate a random number according the number of elements in the ArrayList
            int num_choice = random_index.Next(hashTable[current_word].Count);

            //Create a new ArrayList for the specified key from the hashTable.
            ArrayList list = hashTable[current_word];

            //Convert and create a string which is the output of the funtion
            string new_word = Convert.ToString(list[num_choice]);

            //Return the random word.
            return new_word;
        }

        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);
        }
    }
}
