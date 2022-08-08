//HangMan !!
//A simple text based hangman game designed by Barry McNickle



namespace Hangman
{

    class Program
    {
        static void Main(string[] args)
        {
            //Ensures that the console display area is large enough to show game
            Console.WindowHeight = 40;

            //defines the variables needed throughout program
            var level = new List<string> { "banana", "tortoise", "hedgehog", "carpet", "spider" };
            var lettersTried = new List<char> { };
            string answer;
            char guess = '`';
            int lives = 5;
            int checkIndex;
            bool validEntry = true;
            bool win = false;
            bool correctGuess;
            int selection = 0;
            char checkGuess;
            string solution = "";


            //User selects level from list of possible words called 'level'
            //loop checks user entry to ensure valid selection is made
            //prints error message with invalid entry
            do
            {
                UpdateDisplay(-1);
                Console.WriteLine();
                Console.WriteLine($"You have {level.Count()} possible levels");

                //Wording is changed depending on the value of selection.
                //First time message is displayed? Or has there been an invalid entry?
                if (selection == 0)
                {
                    Console.Write($"Please choose a level from 1 to {level.Count()}: ");
                }
                else
                {
                    Console.WriteLine("You have made an invalid selection!");
                    Console.Write($"Please choose a level from 1 to {level.Count()}: ");
                }

                //Checks that user entry is an int then assigns the value to 'selection' if it is
                var input = Console.ReadLine();
                Int32.TryParse(input, out selection);

                //If user entry is outside the range required an error message is displayed
                //and 'selection' is assigned a value of 0 to enuse that the loop will repeat
                if (selection < 1 || selection > level.Count())
                {
                    selection = -1;
                    Console.WriteLine();
                }

            } while (selection !< 1);

            //The answer is assigned to string variable so we can later check if user wins game
            //all entered char will be converted to UpperCase to avoid invalid entries
            //Each letter of answer is entered into a list to allow help check for correct guesses
            answer = level[selection -1];
            answer = answer.ToUpper();
            var checkAns = new List<char> { };
            checkAns.AddRange(answer);

            //The variable 'solution' is used to display to the user the number of letters in
            //the answer, the correct letters guessed with their position and the letters still
            //to be guessed.  It will be updated later with each correct guess.  Each unguessed
            //letter will be represented with a '_' and a space will be kept between each letter
            //for easy reading.
            while (solution.Length != (answer.Length * 2))
            {
                solution = solution + " _";
            }

            //Main body of game.  Will continue until user runs out of lives or win is changed to True
            while (lives > 0  && win != true)
            {
                selection = 0;
                correctGuess = false;

                //Do While loop is setup to display the screen, take input from user, check for
                //validation and if the user has made the same entry previously.
                do
                {
                    //Display is updated depending on the number of lives remaining
                    UpdateDisplay(lives);
                    Console.WriteLine();
                    //Solution with current correct guesses is dispayed
                    Console.Write("SOLUTION:" + solution);
                    
                    Console.WriteLine();
                    
                    //Diaplays message if user hasn't made any guesses yet
                    //or displays the letters that have been guessed.
                    if (lettersTried.Count() == 0)
                    {
                        Console.Write("You have made no guesses yet.");
                    }
                    else
                    {
                        Console.Write($"So far you have tried: ");
                        foreach (var item in lettersTried)
                        {
                            Console.Write(item + "  ");
                        }
                    }
                    
                    //Displays the current number of remaining lives
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"You have {lives} lives remaining");

                    //Displays an appropriate message depending user hasn't made a guess yet, has
                    //made an invalid entry or has already guessed the previous letter.
                    switch (selection)
                    {
                        case 0:
                            Console.Write("Please guess a letter: ");
                            break;
                        case 1:
                            Console.WriteLine($"Your entry of {guess} is invalid");
                            Console.Write("Please guess a letter: ");
                            break;
                        case 2:
                            Console.WriteLine($"You have already guessed {guess}. Try again!");
                            Console.Write("Please guess another letter: ");
                            break;
                    }

                    //User entry is assumed correct until proven wrong to aid in validation
                    validEntry = true;

                    //User entry is assigned to a temporary string to allow some validation checks.
                    //Spaces are removed from each end of the user entry and then the IF statement
                    //ensures that the entry can not be null
                    string temp = Console.ReadLine();
                    temp = temp.Trim();
                    if (temp == "")
                    {
                        temp = " ";
                    }

                    //Only the first character of the user entry is accepted and it is converted to
                    //uppercase to avoid confusion when checking for correct guesses
                    guess = temp[0];
                    guess = Char.ToUpper(guess);

                    //IF the character held in guess is NOT a letter, variable 'validEntry' is set to false
                    //and then 'selection' is set to ensure correct message is displayed when loop repeats.
                    if (!char.IsLetter(guess))
                    {
                        validEntry = false;
                        selection = 1;
                    }
                    else
                    {
                        //If the user guess IS a letter then it is checked against each previously guessed
                        //letter so that they don't guess the same letter twice.  If a repeated is found,
                        //'validEntry' is set to false and 'selection' is set to ensure correct message is
                        //displayed when loop repeats.
                        checkIndex = 0;
                        while (checkIndex < lettersTried.Count())
                        {
                            checkGuess = lettersTried[checkIndex];

                            if (guess.CompareTo(checkGuess) == 0)
                            {
                                checkIndex = lettersTried.Count();
                                selection = 2;
                                validEntry = false;
                            }

                            checkIndex++;
                        }
                    }
                    //If repeated letter is NOT found, 'validEntry' remains true and the program can continue.
                } while (validEntry == false);


                //Checks users guess with each letter of the answer individually.  It does
                //this using the variable checkAns where each letter was split into items of
                //a list just after the level was selected.
                checkIndex = 0;
                while (checkIndex < answer.Length)
                {
                    checkGuess = checkAns[checkIndex];

                    if (guess.CompareTo(checkGuess) == 0)
                    {
                        //when the guess is correct the method UpdateSolution is called to update the
                        //variable 'solution' to include the correct guess.
                        //The boolean 'correctGuess' is updated to ensure user does not lose a life
                        solution = UpdateSolution(solution, checkIndex, guess);
                        correctGuess = true;
                    }

                    checkIndex++;
                }

                //If the guess was correct, CheckWin method is called see if the user has completed the game
                //If the guess was wrong, the list 'lettersTried' is updated with incorrect guess
                //Then the number of lives remaining is then decremented
                if (correctGuess == true)
                {
                    win = CheckWin(answer, solution);
                }
                else
                {
                    lettersTried.Add(guess);
                    lives--;
                }
            }

            //if statement diaplays the win/lose messages
            if (lives == 0)
            {
                UpdateDisplay(lives);
            }
            else
            {
                UpdateDisplay(lives);
                Console.WriteLine("        WOO HOO!!!");
                Console.WriteLine("             YOU NAILED IT!!");
                Console.WriteLine();
                Console.WriteLine("    THE ANSWER WAS:");
                Console.WriteLine("                    " + solution + " !! !! !!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            //Finally, a short message to say that "I Made This" and then a wait until user ends program
            Console.WriteLine("Brought to you by Barry The Brilliant!");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Press 'Enter' or 'Return' to end game");
            Console.Read();

        }
        static string UpdateSolution(string input, int index, char newChar)
        {
            //If a correct character is guessed, this method will update the string 'solution' to reveal
            //that character.  The string being updated is 'solution' in the Main method, which has
            //a ' ' before each letter.  So the position of the correct letter being passed into
            //the int 'index' needs to be altered to compinsate for this.
            index = index * 2 + 1;

            //The string being updated is converted into an array of characters
            char[] chars = input.ToCharArray();

            //The character at the position of 'index' is altered to match the value held in 'newChar',
            //which is the users correct guess
            chars[index] = newChar;

            //The array of characters is converted back into a string and returned to the Main method
            return new string(chars);
        }

        static bool CheckWin(string ans, string resultToCheck)
        {
            //This method is called whenever a correct guess is made.  It removes all of the spaces
            //in the string 'solution' then compares it to the string 'answer' which are passed when
            //the method is called.  When the method is complete, it returns a boolean value to say
            //if all letters have been revealed.
            bool success = false;

            string[] tempResult = resultToCheck.Split(' ');
            resultToCheck = "";
            foreach (var letter in tempResult)
            {
                resultToCheck = resultToCheck + letter;
            }

            success = Equals(ans, resultToCheck);

            return success;
        }

        static void UpdateDisplay(int screen)
        {
            //Gets the information needed from the file Display.txt needed to update the display
            //and assigns each line in the text file to a specific entry in a string array called lines.
            string[] lines = System.IO.File.ReadAllLines(@".\Display.txt");

            int printLine = 1;
            int lastLineToPrint = 1;

            //Screen is cleared and the lines needed to be written on screen are selected depending
            //on the number of lives the user have remaining as passed when method is called.
            Console.Clear();
            switch (screen)
            {
                case -1:
                    printLine = 2;
                    lastLineToPrint = 30;
                    break;
                case 5:
                    printLine = 33;
                    lastLineToPrint = 51;
                    break;
                case 4:
                    printLine = 55;
                    lastLineToPrint = 73;
                    break;
                case 3:
                    printLine = 77;
                    lastLineToPrint = 95;
                    break;
                case 2:
                    printLine = 99;
                    lastLineToPrint = 117;
                    break;
                case 1:
                    printLine = 121;
                    lastLineToPrint = 139;
                    break;
                case 0:
                    printLine = 143;
                    lastLineToPrint = 172;
                    break;

            }

            //Prints the lines from the file Display.txt as assigned in above switch statement
            while (printLine <= lastLineToPrint)
            {
                Console.WriteLine(lines[printLine]);
                printLine++;
            }
        }

    }

}