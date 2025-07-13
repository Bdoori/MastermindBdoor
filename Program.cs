// Steer Elite Internship Program – Skills Assessment – Gameplay Programming Track
// Mastermind Console Game
// 
// Copyright Bdoor Alolayan 2025.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastermindBdoor
{
    /// <summary>
    /// Entry point for the Mastermind console game.
    /// Handles user configuration and launches the game loop.
    /// </summary>
    class Program
    {
        #region Main and Configuration

        static void Main(string[] args)
        {
            string secretCode = null;
            int defaultAttempts = 10;
            bool hasValidConfiguration = false;
            bool hasShownInstructions = false;

            while (!hasValidConfiguration)
            {
                if (!hasShownInstructions)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Use -c to choose your secret code (4 unique digits from 0 to 8).");
                    Console.WriteLine("Use -t to set number of attempts (from 1 to 20).");
                    Console.WriteLine("Press Enter to play with a random code and default attempts.");
                    Console.WriteLine("Use -h or --help for instructions and help.");
                    Console.WriteLine("Press Ctrl+D to exit the game.");
                    Console.WriteLine();
                    Console.ResetColor();
                    hasShownInstructions = true;
                }

                Console.Write("Enter your choice: ");
                string userInput = ReadInput();

                // If Ctrl+D → EOF → exit immediately
                if (userInput == null)
                {
                    return;
                }

                string trimmedInput = userInput.Trim();

                // blank input → random code
                if (trimmedInput.Length == 0)
                {
                    hasValidConfiguration = true;
                    continue;
                }

                // Full help flag
                if (trimmedInput.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                    trimmedInput.Equals("--help", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintFullHelp();
                    Console.ResetColor();
                    continue;
                }

                // Parse flags (-c, -t)
                string[] tokens = trimmedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var parsedOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < tokens.Length; i++)
                {
                    string token = tokens[i];
                    if (token.Equals("-c", StringComparison.OrdinalIgnoreCase) && i + 1 < tokens.Length)
                    {
                        parsedOptions["c"] = tokens[++i];
                    }
                    else if (token.Equals("-t", StringComparison.OrdinalIgnoreCase) && i + 1 < tokens.Length)
                    {
                        parsedOptions["t"] = tokens[++i];
                    }
                    else
                    {
                        parsedOptions["?"] = string.Empty;
                    }
                }

                // Invalid token check
                if (parsedOptions.ContainsKey("?"))
                {
                    ShowError("Wrong input!");
                    continue;
                }

                // Valid combinations: c || t || c+t || t+c
                bool isCodeOnly = parsedOptions.ContainsKey("c") && parsedOptions.Count == 1;
                bool isAttemptsOnly = parsedOptions.ContainsKey("t") && parsedOptions.Count == 1;
                bool isCodeAndAttempts = parsedOptions.ContainsKey("c") && parsedOptions.ContainsKey("t") && parsedOptions.Count == 2;

                if (isCodeOnly)
                {
                    string codeValue = parsedOptions["c"];
                    if (!IsValidCode(codeValue))
                    {
                        ShowError("Wrong input!");
                        continue;
                    }
                    secretCode = codeValue;
                    hasValidConfiguration = true;
                }
                else if (isAttemptsOnly)
                {
                    if (!int.TryParse(parsedOptions["t"], out int tValue) || tValue < 1 || tValue > 20)
                    {
                        ShowError("Wrong input!");
                        continue;
                    }
                    defaultAttempts = tValue;
                    hasValidConfiguration = true;
                }
                else if (isCodeAndAttempts)
                {
                    string codeValue = parsedOptions["c"];
                    bool validCode = IsValidCode(codeValue);
                    bool validAttempts = int.TryParse(parsedOptions["t"], out int tValue) && tValue >= 1 && tValue <= 20;

                    if (!validCode || !validAttempts)
                    {
                        ShowError("Wrong input!");
                        continue;
                    }

                    secretCode = codeValue;
                    defaultAttempts = tValue;
                    hasValidConfiguration = true;
                }
                else
                {
                    ShowError("Wrong input!");
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine("Will you find the secret code?");
            Console.WriteLine("Please enter a valid guess:");
            Console.ResetColor();

            var game = new MastermindGame(secretCode, defaultAttempts);
            game.Play();
        }

        #endregion

        #region Help and I/O Utilities

        /// <summary>
        /// Reads user input key-by-key; returns null on Ctrl+D (EOF).
        /// </summary>
        public static string ReadInput()
        {
            var buffer = new StringBuilder();

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                // Ctrl+D detection
                if (keyInfo.Key == ConsoleKey.D && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    Console.WriteLine();
                    return null;
                }

                // Enter → end of line
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                // Backspace handling
                if (keyInfo.Key == ConsoleKey.Backspace && buffer.Length > 0)
                {
                    buffer.Length--;
                    Console.Write("\b \b");
                    continue;
                }

                buffer.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Displays the short help menu during gameplay.
        /// </summary>
        public static void PrintShortHelp()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help      Show help message");
            Console.WriteLine("  Ctrl+D          Exit the game");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays the full help menu for command-line options.
        /// </summary>
        private static void PrintFullHelp()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("  -c <code>       Choose your secret code (4 unique digits 0–8)");
            Console.WriteLine("  -t <attempts>   Set number of attempts (1–20)");
            Console.WriteLine("  [ENTER]         Play with random code and default attempts");
            Console.WriteLine("  -h, --help      Show help message");
            Console.WriteLine("  Ctrl+D          Exit the game");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Displays an error message in red.
        /// </summary>
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine();
            Console.ResetColor();
        }

        #endregion

        #region Validation Helper

        /// <summary>
        /// Validate secret code: exactly 4 unique digits between 0 and 8 .
        /// </summary>
        private static bool IsValidCode(string code)
        {
            if (code.Length != 4)
            {
                return false;
            }

            foreach (char digit in code)
            {
                if (digit < '0' || digit > '8' || code.IndexOf(digit) != code.LastIndexOf(digit))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates Mastermind game logic: guessing rounds, evaluation, and feedback.
    /// </summary>
    class MastermindGame
    {
        #region Fields and Constructor

        private static readonly Random RandomGenerator = new Random();
        private readonly string secretCode;
        private readonly int defaultAttempts;

        public MastermindGame(string code, int attempts)
        {
            secretCode = code ?? GenerateRandomCode();
            defaultAttempts = attempts;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs the game loop, handling guesses and providing feedback.
        /// </summary>
        public void Play()
        {
            int currentAttempt = 0;

            while (currentAttempt < defaultAttempts)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine();
                Console.WriteLine($"Round {currentAttempt}:");
                Console.ResetColor();

                string guess = Program.ReadInput();

                // Ctrl+D during guess → exit 
                if (guess == null)
                {
                    return;
                }

                string trimmedGuess = guess.Trim();
                if (trimmedGuess.Equals("-h", StringComparison.OrdinalIgnoreCase) || trimmedGuess.Equals("--help", StringComparison.OrdinalIgnoreCase))
                {
                    // only short help during gameplay
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Program.PrintShortHelp();
                    Console.ResetColor();
                    continue;
                }

                if (!IsValidGuess(trimmedGuess))
                {
                    ShowGuessError();
                    continue;
                }

                if (trimmedGuess == secretCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Congratz! You did it!");
                    Console.ResetColor();
                    return;
                }

                currentAttempt++;

                if (currentAttempt >= defaultAttempts)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game over! You ran out of attempts.");
                    Console.ResetColor();
                    return;
                }

                int wellPlacedCount = GetWellPlacedCount(trimmedGuess);
                int misplacedCount = GetMisplacedCount(trimmedGuess);

                Console.WriteLine($"Well placed pieces: {wellPlacedCount}");
                Console.WriteLine($"Misplaced pieces: {misplacedCount}");
            }
        }

        #endregion

        #region Private Helpers

        private static void ShowGuessError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input!");
            Console.ResetColor();
        }

        // Validate guess: exactly 4 unique digits between 0 and 8
        private bool IsValidGuess(string guess)
        {
            if (guess.Length != 4)
            {
                return false;
            }

            foreach (char digit in guess)
            {
                if (digit < '0' || digit > '8' || guess.IndexOf(digit) != guess.LastIndexOf(digit))
                {
                    return false;
                }
            }

            return true;
        }

        // Generate a random secret code of 4 unique digits between 0 and 8
        private string GenerateRandomCode()
        {
            var pool = new List<char>("012345678");
            var sb = new StringBuilder();

            while (sb.Length < 4)
            {
                int index = RandomGenerator.Next(pool.Count);
                sb.Append(pool[index]);
                pool.RemoveAt(index);
            }

            return sb.ToString();
        }

        // Count digits in correct position
        private int GetWellPlacedCount(string guess)
        {
            int count = 0;

            for (int i = 0; i < secretCode.Length; i++)
            {
                if (guess[i] == secretCode[i])
                {
                    count++;
                }
            }

            return count;
        }

        // Count digits correct but in wrong position
        private int GetMisplacedCount(string guess)
        {
            int count = 0;

            for (int i = 0; i < secretCode.Length; i++)
            {
                if (guess[i] != secretCode[i] && secretCode.Contains(guess[i]))
                {
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}