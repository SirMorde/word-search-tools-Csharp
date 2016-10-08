using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PA03_Ma {
    class Program {

        // Main method that will execute when the application is ran
        static void Main(string[] args) {

            // Relative file path to our dictionary of words
            string fullPath = @"..\..\WordList.txt";

            // Make sure that the user has WordList.txt in their project folder
            if (File.Exists(fullPath)) {

                /// <summary>
                /// Allocate memory to an array to hold all of our words. We over allocate just to 
                /// ensure that the array is big enough
                /// </summary>
                string[] wordList = new string[22301];

                // Store each word of WordList.txt into its own element in the array wordList
                wordList = System.IO.File.ReadAllLines(fullPath);

                // Normalize all the data to be lower case and remove all non alphabet characters
                for (int i = 0; i < wordList.Length; i++) {
                    wordList[i] = wordList[i].ToLower();

                    /// <summary>
                    /// Replace all characters that aren't A-Z or a-z with a blank. 
                    /// This is to account for the case where the dictionary has a word with a symbol embedded in it
                    /// Ex: hel;lo.
                    /// We already made all of our data lower case, but I still include capital A-Z as a precaution
                    /// </summary>
                    wordList[i] = Regex.Replace(wordList[i], @"[^A-Za-z]+", "");
                }

                // To be used as a menu selection. It will eventually be populated with the user's menu choice
                int userInput = 0;

                do {

                    // Display the menu to the user and store their menu selection in userInput
                    userInput = DisplayMenu();

                    // Executes the case statement based on which menu option the user chooses
                    switch (userInput) {
                        case 1:
                            Console.WriteLine();
                            Console.WriteLine("1. All words selected!");
                            Console.WriteLine();

                            // Loop through the array and display each word
                            for (int i = 0; i < wordList.Length; i++) {
                                Console.WriteLine("Word #{0}: {1}", i, wordList[i]);
                            }
                            Console.WriteLine();
                            break;
                        case 2:
                            Console.WriteLine();
                            RhymingWords(wordList);
                            break;
                        case 3:
                            Console.WriteLine();
                            ScrabbleWords(wordList);
                            break;
                        case 4:
                            Console.WriteLine();
                            MorphWords(wordList);
                            Console.WriteLine();
                            break;
                        case 5:
                            Console.WriteLine();
                            MorphChain(wordList);
                            Console.WriteLine();
                            break;
                        case 6:
                            Console.WriteLine();
                            Console.WriteLine("6. Quit selected");
                            Console.WriteLine();
                            Console.WriteLine("Thank you for using this application! :)");
                            Console.WriteLine();
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;

                        /// <summary>
                        /// Default case used as a precaution in case if the user somehow enters an unrecognizable 
                        /// menu option
                        /// </summary>
                        default:
                            Console.WriteLine("Hmm, something went wrong. Please choose another option");
                            break;
                    } // End of switch case statement

                    // Keep on looping until the user inputs 5
                } while (userInput != 6);
            } // End of the if statement that ensures WordList.txt could be located

            /// <summary>
            /// If WordList.txt could not be found, tell the user to add it to the proper folder before running 
            /// the application
            /// <summary>
            else {
                Console.WriteLine("**ERROR**");
                Console.WriteLine("WordList.txt could not be located!");
                Console.WriteLine("Please make sure that WordList.txt is in the same folder as Program.cs");
                Console.WriteLine();
                Console.WriteLine("Step 1: Locate C:\\...\\PA02_Ma\\Program.cs");
                Console.WriteLine("Step 2: Copy WordList.txt into the same folder");
                Console.WriteLine("  Ex: C:\\...\\PA02_Ma\\WordList.txt");
                Console.WriteLine("Step 3: Re-run the console application again");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            } // End of else statement

        } // End of Main method


        static public void MorphChain(string[] wordList) {
            bool invalidInput = false;
            string morphWordStart;
            string morphWordEnd;
            int morphChainMax = 0;

            Console.WriteLine("5. Morph Chain selected!");


            // |=====**READ IN USER INPUTS**======|

            // Error checking to ensure that the user enters a string between a-z
            do {
                Console.Write("Please enter start word: ");
                morphWordStart = Console.ReadLine();

                // Ensure that only letters from a-z were provided
                if (IsAllLetters(morphWordStart) == true) {

                    // Exit the do while loop
                    invalidInput = false;
                }
                else {
                    Console.WriteLine();
                    Console.WriteLine("This is not a valid input! Only letters from a-z are allowed");
                    invalidInput = true;
                }

              // If the input given is not a string with characters a-z, loop again
            } while (invalidInput == true);

            // Error checking to ensure that the user enters a string between a-z
            do {
                Console.Write("Please enter an end word: ");
                morphWordEnd = Console.ReadLine();

                // Ensure that only letters from a-z were provided and that the start & end words are the same length
                if (IsAllLetters(morphWordEnd) == true && morphWordEnd.Length == morphWordStart.Length ) {

                    // Exit the do while loop
                    invalidInput = false;
                }
                else {
                    Console.WriteLine();
                    Console.WriteLine("This is not a valid input! Only letters from a-z are allowed");
                    Console.WriteLine("Your end word must be the same length as your start word.");
                    Console.WriteLine("Your start word is {0} characters long.", morphWordStart.Length);
                    invalidInput = true;
                }

              // If the input given is not a string with characters a-z, loop again
            } while (invalidInput == true);

            // Error checking to ensure that the user enters an int
            do {
                Console.Write("Please enter a max chain length: ");

                if (int.TryParse(Console.ReadLine(), out morphChainMax) == true) {
   
                        // This will cause us to exit the do while loop
                        invalidInput = false;
                }
                else {
                    Console.WriteLine();
                    Console.WriteLine("This is not a valid input! Only whole numbers are allowed");
                    invalidInput = true;
                }

              // If the input given is not an int, loop again
            } while (invalidInput == true);

            // Print space for formatting
            Console.WriteLine();

            //  |=====**BEGIN FORMING THE MORPH CHAIN**======|


            // Normalize all letters to lower case in order to match the data set in WordList
            morphWordStart = morphWordStart.ToLower();

            // Normalize all letters to lower case in order to match the data set in WordList
            morphWordEnd = morphWordEnd.ToLower();

            // List containing WordList.txt
            List<string> wordListDictionary = new List<string>(wordList);

            /// <summary>
            /// List of lists containing every morph word and each of its morph words
            /// Whenever a new group of morph words are found, it adds another list containing 
            /// each of the previous elements' morph words
            /// </summary>
            List<List<string>> finalMorphPath = new List<List<string>>();

            // This will hold every morph word and each of its morph words in a single list
            List<string> morphChainList = new List<string>();

            // Add the beginning morph word to our list
            morphChainList.Add(morphWordStart);

            // Remove our starting morph word so that we don't run through an iteration of it
            wordListDictionary.Remove(morphWordStart);

            //Will keep track of the length of the morph chain we have created
            int length = 1;

            //This will essentially loop forever as long as morphChainList is provided a start word
            while (morphChainList.Count > 0) {

                // Length of morphChainList
                int count = morphChainList.Count;

                // Increment into each word in your original morphChainList
                for (int i = 0; i < count; i++) {

                    // Remove first element in morphChainList and store in current
                    string current = RemoveFirstElement(morphChainList);

                    /// <summary>
                    /// Loop through each position of the current word in our morphChainList and form
                    /// the different morph words position by position.
                    /// EX:
                    /// given starting word is told
                    /// first iteration will produce a list of things like told, bold, cold, etc.
                    /// </summary>
                    for (int j = 0; j < current.Length; j++) {
                        for (char c = 'a'; c <= 'z'; c++) {
                            if (c == current[j]) {

                                // Move on to the next iteration of the for (char c = 'a'; c <= 'z'; c++) loop
                                continue;
                            }

                            // Make temp equal to a morph word of the current word in the dictionary
                            string temp = Replace(current, j, c);

                            /// <summary>
                            /// If temp equals our final morph word, return how many morph chains it took to
                            /// Get to our final morph word
                            /// </summary>
                            if (temp.CompareTo(morphWordEnd) == 0) {

                                //Ensure that the traversal path was below the specified max chain length
                                if(length+1 <= morphChainMax) {
                                    List<string> endingMorphPath = new List<string>(); ;

                                    // Insert our end morph word to the front of our endingMorphPath list
                                    endingMorphPath.Insert(0, temp);

                                    //Initialize your reference word to the desired starting word
                                    string currentReference = temp;

                                    /// <summary>
                                    /// Start from the end of the final morph path and work your way up the chain to the start word
                                    /// Within the list of lists (finalMorphPath), start at the back of the list
                                    /// </summary>
                                    for (int currentFinalElement = finalMorphPath.Count-1; currentFinalElement >= 0; currentFinalElement--) {
                                       foreach(string currentWord in finalMorphPath[currentFinalElement]) {

                                            /// <summary>
                                            /// Go through each word of the current list and if you find 1 character difference,
                                            /// insert it to our endingMorphPath to be printed and make this your new reference word
                                            /// </summary>
                                            if(OneCharDifferent(currentWord, currentReference) == true) {
                                                currentReference = currentWord;
                                                endingMorphPath.Insert(0, currentWord);
                                                break;
                                            } 
                                        }

                                    }

                                    //Insert our starting word to the beginning of the solution chain
                                    endingMorphPath.Insert(0, morphWordStart);
                                    Console.WriteLine("Solution Chain: ");

                                    //Print out our final morph chain from start to finish
                                    foreach(string morphChain in endingMorphPath) {
                                        Console.WriteLine(morphChain);
                                    }

                                    //Break out of this method
                                    return;
                                } // End of if(length+1 <= morphChainMax) 
                                else {

                                    // A morph path was found, but the morph chain is longer than the max morph chain length
                                    Console.WriteLine("No solution");

                                    //Break out of this method
                                    return;
                                }
                            }

                            //If temp is a morph word in our dictionary, but not our end word
                            //Add the word to our morphChainList and remove it from our dictionary
                            if (wordListDictionary.Contains(temp)) {
                                morphChainList.Add(temp);
                                wordListDictionary.Remove(temp);
                            }
                        } // End of (char c = 'a'; c <= 'z'; c++)
                    } // End of (int j = 0; j < current.Length; j++)
                } // End of for loop

                /// <summary>
                /// At this point our morphChainList contains every morph word of our starting word 
                /// and each of its morph words and those morph words have been removed from our dictionary
                /// Create a deep copy of the current iteration of morphChainList and add 
                /// it as an element to the list finalMorphPath
                /// </summary>>
                finalMorphPath.Add(new List<string>(morphChainList));
                length++;

            } // End of while loop

            // This line will only be reached if no morph path is found at all
            Console.WriteLine("No solution");

            //Break out of this method
            return;
        } // End of MorphChain method

        //Used to determine one character difference between two strings
        private static bool OneCharDifferent(string str1, string str2) {

            // Return true where the difference between each character of both strings evaluates to one
            return str1.Where((test, counter) => !test.Equals(str2[counter])).Count() == 1;
        }

        // Used to return the first element in a list and remove it from the list
        private static string RemoveFirstElement(List<string> list) {
            string firstElement = list[0];
            list.RemoveAt(0);
            return firstElement;
        }

        //This will replace the entered word's index with a user provided character
        private static string Replace(string str, int index, char c) {
            char[] chars = str.ToCharArray();

            // Replace current index of the passed in word from WordList.txt with a new letter
            chars[index] = c;
            return new string(chars);

            // Ex: pass in (told, 0, a) will return aold
        }

        // Method that will display the menu options to the user
        static public int DisplayMenu() {

            /// <summary>
            /// invalidInput will be used to ensure that the user enters an integer between 1-5.
            /// Remember that do while statements will iterate at least once, even if the condition 
            /// is met, so it is ok to initialize invalidInput to false
            /// </summary>
            int result;
            bool invalidInput = false;

            Console.WriteLine("Welcome user!");
            Console.WriteLine("In this application we will play with words!");
            Console.WriteLine("Here are your options: ");
            Console.WriteLine();
            Console.WriteLine("1. All Words");
            Console.WriteLine("2. Rhyming Words");
            Console.WriteLine("3. Scrabble Words");
            Console.WriteLine("4. Morph Words");
            Console.WriteLine("5. Morph Chain");
            Console.WriteLine("6. Quit");

            // Error checking to ensure that the user enters an integer between 1-5
            do {

                /// <summary>
                /// This if statement is here because on the first iteration, invalidInput will be false.
                /// This error message will only display after the user has inputted an invalid input
                /// </summary>
                if (invalidInput == true) {
                    Console.WriteLine("This is not a valid input! Only numbers 1-6 are allowed");
                }

                Console.WriteLine();
                Console.Write("Please enter your menu choice: ");

                // Ensure that the user enters a number between 1-6
                if (int.TryParse(Console.ReadLine(), out result) == true) {
                    if (result > 0 && result < 7) {

                        // This will cause us to exit the do while loop
                        invalidInput = false;
                    }
                    else {
                        invalidInput = true;
                    }
                }
                else {
                    invalidInput = true;
                }

                // If the input given is not an int from 1-6, loop through this do statement again
            } while (invalidInput == true);

            // Return the user's menu selection to our main function
            return result;

        } // End of DisplayMenu method


        // Function to validate that the string only contains alphabet letters
        static public bool IsAllLetters(string s) {

            // For each character in the string, look at each character and if it is not a letter, return false
            foreach (char c in s) {
                if (!Char.IsLetter(c)) {
                    return false;
                }
            }

            //If every character was a letter, return true
            return true;
        } // End of IsAllLetters method


        static public void RhymingWords(string[] wordList) {
            bool invalidInput = false;
            bool noRhyme = true;
            string rhymEnding;
            string rhymeEndingOrig;
            Console.WriteLine("2. Rhyming Words selected!");

            // Error checking to ensure that the user enters a string between a-z
            do {
                Console.Write("Please enter desired ending string: ");
                rhymEnding = Console.ReadLine();

                if (IsAllLetters(rhymEnding) == true) {
                    invalidInput = false;
                }
                else {
                    Console.WriteLine();
                    Console.WriteLine("This is not a valid input! Only letteres a-z are allowed");
                    invalidInput = true;
                }

                // If the input given is not a string from a-z, loop through this do statement again
            } while (invalidInput == true);

            // Original input with preserved case to be displayed to the user at the end
            rhymeEndingOrig = rhymEnding;

            // Normalize all letters to lower case in order to match the data set in WordList
            rhymEnding = rhymEnding.ToLower();

            Console.WriteLine();
            Console.WriteLine("Here are the words that rhyme with {0}: ", rhymeEndingOrig);

            // Match rhymEnding with each word in WordList.txt and print the rhyming words
            foreach (string word in wordList) {
                if (word.EndsWith(rhymEnding) == true) {
                    noRhyme = false;
                    Console.WriteLine(word);
                }
            }

            if (noRhyme == true) {
                Console.WriteLine("No words found that rhyme with {0}", rhymeEndingOrig);
            }
            Console.WriteLine();
        } // End of RhymingWords method


        static public void ScrabbleWords(string[] wordList) {
            bool invalidInput = false;
            bool noWords = true;
            string scrabbleLetters;
            string scrabbleLettersOrig;
            char[] scrabbleLettersArray;
            char[] wordListArray;
            int match = 0;

            Console.WriteLine("3. Scrabble Words selected!");

            // Error checking to ensure that the user enters a string between a-z
            do {
                Console.Write("Please enter scrabble letters: ");
                scrabbleLetters = Console.ReadLine();

                // Ensure that three to seven letters from a-z were provided
                if (IsAllLetters(scrabbleLetters) == true && scrabbleLetters.Length > 2 && scrabbleLetters.Length < 8) {
                    invalidInput = false;
                }
                else {
                    Console.WriteLine();
                    Console.WriteLine("This is not a valid input! Three to seven letters from a-z must be provided");
                    invalidInput = true;
                }

                // If the input given is not a 3-7 character long string from a-z, loop through this do statement again
            } while (invalidInput == true);

            // Original input with preserved case to be displayed to the user at the end
            scrabbleLettersOrig = scrabbleLetters;

            // Normalize all letters to lower case in order to match the data set in WordList
            scrabbleLetters = scrabbleLetters.ToLower();

            // Create an array storing each character of the scrabble letters into its own index
            scrabbleLettersArray = scrabbleLetters.ToCharArray();

            Console.WriteLine();
            Console.WriteLine("Here are the words made using {0}: ", scrabbleLettersOrig);

            // loop through each word in the data set stored in WordList.txt
            foreach (string word in wordList) {
                if (word.Length <= scrabbleLetters.Length && word.Length > 2 && word.Length < 8) {

                    // Create an array storing each character of the current word its respective an index
                    wordListArray = word.ToCharArray();

                    // Initialize match to 0 for every word we will cycle through from WordList.txt
                    match = 0;

                    // Loop through every character of the scrabble letters 
                    for (int counter = 0; counter < scrabbleLetters.Length; counter++) {

                        // Loop through every character of the word from WordList.txt
                        for (int count2 = 0; count2 < word.Length; count2++) {

                            /// <summary>
                            /// If there is a match between the current letter in the current word and the current
                            /// letter of the scrabble letters, then  remove the current letter of the current word
                            /// so it won't be matched again and continue to search for more matches
                            /// </summary>
                            if (wordListArray[count2] == scrabbleLettersArray[counter]) {

                                /// <summary>
                                /// Every time a match is made, increment match. By the end, if the # of matches
                                /// equals the # of characters in the current word in WordList.txt we will print it
                                /// </summary>
                                match++;

                                /// <summary>
                                /// Replace the matched character with a space so it won't be matched again when
                                /// we move onto the next scrabble letter
                                /// </summary>
                                wordListArray[count2] = ' ';

                                /// <summary>
                                /// As soon as a match is made between the current character of the scrabble letter
                                /// and the current letter of the word from WordList.txt, exit this loop and move on
                                /// to the next scrabble letter and continue to compare with the current word
                                /// </summary>
                                break;
                            } // End of if statement
                        } // End of for loop
                    } // End of for loop

                    // If # of matches were same as # of letters in the current word from WordList.txt, print it
                    if (match == word.Length) {

                        //noWords is only true if no words were able to be formed from the provided letters
                        noWords = false;
                        Console.WriteLine(word);
                    }
                } // End of if statement checking if current word from WordList.txt meets conditions
            } // End of foreach loop

            if (noWords == true) {
                Console.WriteLine("No words could be made using {0}", scrabbleLettersOrig);
            }
            Console.WriteLine();
        } // End of ScrabbleWords method


        static public void MorphWords(string[] wordList) {
            bool invalidInput = false;
            bool noWords = true;
            string morphWord;
            string morphWordOrig;
            char[] morphWordArray;
            char[] wordListArray;
            int match = 0;

            Console.WriteLine("4. Morph Words selected!");

            // Error checking to ensure that the user enters a string between a-z
            do {
                Console.Write("Please enter start word: ");
                morphWord = Console.ReadLine();

                // Ensure that only letters from a-z were provided
                if (IsAllLetters(morphWord) == true) {

                    // Exit the do while loop
                    invalidInput = false;
                }
                else {
                    Console.WriteLine();
                    Console.WriteLine("This is not a valid input! Only letters from a-z are allowed");
                    invalidInput = true;
                }

                // If the input given is not a sting with characters a-z, loop again
            } while (invalidInput == true);

            // Original  input with preserved case to be displayed to the user at the end
            morphWordOrig = morphWord;

            // Normalize all letters to lower case in order to match the data set in WordList
            morphWord = morphWord.ToLower();

            // This will be used later on to compare the morph word to words in wordList
            morphWordArray = morphWord.ToCharArray();

            Console.WriteLine();
            Console.WriteLine("Here are the morph words of {0}: ", morphWordOrig);

            // For each word in WordList.txt compare it to our morph word
            foreach (string word in wordList) {

                // Ensure that the morph word and current word in wordList are the same length
                if (word.Length == morphWord.Length) {

                    // Turn the current word into an array of chars with each letter occupying its respective index
                    wordListArray = word.ToCharArray();

                    // Initialize match to 0 for every word we will cycle through from WordList.txt
                    match = 0;

                    for (int counter = 0; counter < morphWord.Length; counter++) {

                        /// <summary>
                        /// Everytime a match is found between the current index of the word in wordList
                        /// and the current index of our morph word, increment match
                        /// </summary>
                        if (wordListArray[counter] == morphWordArray[counter]) {
                            match++;
                        }
                    } // End of for loop

                    // If the two words differ by one character, print it and set noWords to false
                    if (match == morphWord.Length - 1) {
                        noWords = false;
                        Console.WriteLine(word);
                    }

                } // End of if statement checking for matching lengths of our morph word and current word in WordList.txt
            } // End of foreach loop

            // If no morph words were found, tell the user
            if (noWords == true) {
                Console.WriteLine("There are no morph words of {0}", morphWordOrig);
            }

            // Skip a line to seperate next line of text
            Console.WriteLine();
        } // End of MorphWords method

    } // End of class Program
} // End of namespace PA03_Ma