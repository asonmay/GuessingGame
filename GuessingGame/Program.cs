using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace GuessingGame
{
    internal class Program
    {
        public enum GameMode
        {
            GuessingGame = 1,
            ReverseGuessingGame = 2,
            Exit = 3
        }
        static void Main(string[] args)
        {
            Random random = new Random();
            const int MAX_ATTEMPTS = 10;
            const int MIN_RANGE = 10;

            Console.Write("What's your name? ");
            string playerName = Console.ReadLine();
            Console.WriteLine($"Welcome to Guessing Game, {playerName}");

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("What would you like to play?");
                Console.WriteLine($"  {(int)GameMode.GuessingGame}. Guessing Game (I pick, you guess)");
                Console.WriteLine($"  {(int)GameMode.ReverseGuessingGame}. Reverse Guessing Game (you pick, I guess)");
                Console.WriteLine($"  {(int)GameMode.Exit}. Exit");
                int choice = ReadNumber("Enter 1, 2 or 3: ");

                GameMode mode = (GameMode)choice;

                if (mode == GameMode.Exit)
                {
                    Console.WriteLine($"Thanks for playing, {playerName}. Goodbye!");
                    running = false;
                }
                else if(mode == GameMode.GuessingGame)
                {
                    RunGuessingGame(random, MAX_ATTEMPTS, MIN_RANGE);
                }
                else
                {
                    RunReverseGuessingGame(random, MAX_ATTEMPTS, MIN_RANGE);
                }
            }
        }

        static void RunReverseGuessingGame(Random random, int MAX_ATTEMPTS, int MIN_ALLOWED_RANGE)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Reverse Guessing Game");
            Console.WriteLine("you will choose a random number and I will attempt to guess that number in less then 10 attempts");

            int min;
            int max;
            SetRange(out min, out max, MIN_ALLOWED_RANGE);

            Console.WriteLine("Ok, here we go!");
            Console.WriteLine("Choose your number then type something when you are ready.");
            Console.ReadLine();

            int attempts = 0;
            int[] guesses = new int[MAX_ATTEMPTS];
            while (attempts < MAX_ATTEMPTS)
            {
                guesses[attempts] = min + (max - min) / 2;
                Console.WriteLine($"I have {attempts} attempts left.");
                Console.WriteLine($"Is your number {guesses[attempts]}? (h or l or c)");
                string response = Console.ReadLine();
                if (response == "h")
                {
                    min = guesses[attempts] + 1;
                }
                else if (response == "l")
                {
                    max = guesses[attempts] - 1;
                }
                else if(response == "c")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("What?");
                    attempts--;
                }
                attempts++;
            }
            
            if (attempts >= MAX_ATTEMPTS)
            {
                Console.WriteLine("Game Over!");
                Console.WriteLine("I ran out of attempts...");
            }
            else
            {
                Console.WriteLine("Hurray! I win");
            }

            DisplayGuesses(guesses, attempts);
            PlayAgain(RunReverseGuessingGame, random, MAX_ATTEMPTS, MIN_ALLOWED_RANGE);
        }

        static void RunGuessingGame(Random random, int MAX_ATTEMPTS, int MIN_ALLOWED_RANGE)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Guessing Game");
            Console.WriteLine("I will choose a number in your desired range and you will attempt to guess that number in less then 10 attempts");

            int min;
            int max;
            SetRange(out min, out max, MIN_ALLOWED_RANGE);

            Console.Clear();
            Console.WriteLine("Ok, here we go!");
            Console.WriteLine("I have chosen my number, start guessing away!");
            int chosenNumber = random.Next(min, max);

            int attempts = 0;
            int[] guesses = new int[MAX_ATTEMPTS];
            while (attempts < MAX_ATTEMPTS)
            {
                Console.WriteLine($"You have {MAX_ATTEMPTS - attempts} attempts left");
                guesses[attempts] = ReadNumber("Enter your guess here: ");
                
                if(guesses[attempts] < chosenNumber)
                {
                    Console.WriteLine("Too Low! Try again.");
                }
                else if(guesses[attempts] > chosenNumber)
                {
                    Console.WriteLine("Too High! Try again.");
                }
                else
                {
                    break;
                }
                Console.WriteLine();
                attempts++;
            }

            if(attempts >= MAX_ATTEMPTS)
            {
                Console.WriteLine("Game Over!");
                Console.WriteLine("You ran out of attempts...");
                Console.WriteLine($"My number was {chosenNumber}");
            }
            else
            {
                Console.WriteLine("Correct!!!");
                Console.WriteLine($"You guessed my number in {attempts + 1} attempts, it was {chosenNumber}");
            }

            DisplayGuesses(guesses, attempts);
            PlayAgain(RunGuessingGame, random, MAX_ATTEMPTS, MIN_ALLOWED_RANGE);
        }

        static void PlayAgain(Action<Random, int, int> game, Random random, int MAX_ATTEMPTS, int MIN_ALLOWED_RANGE)
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to play again (y/n)");
            string response = Console.ReadLine();

            if (response.Equals("y"))
            {
                game(random, MAX_ATTEMPTS, MIN_ALLOWED_RANGE);
            }
            else if (response.Equals("n"))
            {
                Console.WriteLine("Ok goodbye!");
            }
            else
            {
                Console.WriteLine("I'll take that as a no...");
            }
        }

        static int ReadNumber(string input)
        {
            Console.WriteLine(input);
            string response = Console.ReadLine();
            if (int.TryParse(response, out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Please enter a number...");
                Console.WriteLine("Try again");
                return ReadNumber(input); ;
            }
        }

        static void SetRange(out int min, out int max, int MIN_ALLOWED_RANGE)
        {
            Console.WriteLine();
            min = ReadNumber("Please enter your minimum value: ");
            Console.WriteLine();
            max = ReadNumber("Please enter your maximum value: ");

            if (max < min)
            {
                Console.WriteLine("What");
                Console.WriteLine("Try again");
                Thread.Sleep(1000);
                SetRange(out min, out max, MIN_ALLOWED_RANGE);
                return;
            }
            if (max - min < 10)
            {
                Console.WriteLine($"Please keep range bigger than {MIN_ALLOWED_RANGE}");
                Console.WriteLine("Try again");
                Thread.Sleep(1000);
                SetRange(out min, out max, MIN_ALLOWED_RANGE);
                return;
            }
        }

        static void DisplayGuesses(int[] guesses, int attempts)
        {
            Console.Write("Here is the list of guesses: ");
            for(int i = 0; i < attempts; i++)
            {
                Console.Write($"{guesses[i]}, ");
            }
        }
    }
}
