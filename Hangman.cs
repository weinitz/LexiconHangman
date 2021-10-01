using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Hangman
{
    internal class Hangman
    {
        public readonly int MaxNumberOfGuesses = 10;

        private readonly Player _player;

        private int NumberOfGuesses =>
            _correctGuesses.Count + _incorrectGuesses.Length + _incorrectWordGuesses.Count;

        private int _secretWordIndex;

        private string[] _secreteWords;

        private List<char> _correctGuesses;
        private StringBuilder _incorrectGuesses;
        private List<string> _incorrectWordGuesses;

        private int NumberOfIncorrectGuesses =>
            _incorrectGuesses.Length + _incorrectWordGuesses.Count;

        private StringBuilder _maskedSecretWord;

        public Hangman()
        {
            _player = new Player();
            NewGame();
        }

        public void NewGame()
        {

            ResetGuesses();
            SetRandomSecretWordIndex();
            MaskSecretWord();

        }

        private void MaskSecretWord()
        {
            _maskedSecretWord = new StringBuilder(WordToGuess.Length);
            _maskedSecretWord.Append("".PadRight(WordToGuess.Length, '_'));
            // Reveal any spaces
            RevealToPlayer(' ');
        }

        private void ResetGuesses()
        {
            _correctGuesses = new List<char>();
            _incorrectGuesses = new StringBuilder();
            _incorrectWordGuesses = new List<string>();
        }

        private void SetRandomSecretWordIndex()
        {
            var random = new Random();
            _secretWordIndex = random.Next(0, SecretWords.Length);
        }

        private void RevealToPlayer(char newChar)
        {
            for (var i = 0; i < WordToGuess.Length; i++)
            {
                if (WordToGuess[i] == newChar)
                {
                    _maskedSecretWord[i] = newChar;
                }
            }
        }

        public void Play()
        {
            do
            {
                Draw();

                // Player won
                if (_maskedSecretWord.ToString() == WordToGuess)
                {
                    PlayerWon();
                }

                // Player lost
                if (NumberOfGuesses == MaxNumberOfGuesses)
                {
                    PlayerLost();
                }

                if (_maskedSecretWord.ToString() != WordToGuess && NumberOfGuesses < MaxNumberOfGuesses)
                {
                    Guess();
                }

            } while (true);
        }

        private void PlayerWon()
        {
            _player.Wins++;
            DrawPlayerWon();
            NewGame();
        }

        private void PlayerLost()
        {
            _player.Loss--;
            DrawPlayerLost();
            NewGame();
        }

        private static void DrawPlayerState(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
            Console.ReadKey();
        }

        private static void DrawPlayerWon()
        {
            const string text = @"  
  ______                                                       __     
 /      \                                                     /  |    
/$$$$$$  |  ______    ______    ______    ______    _______  _$$ |_   
$$ |  $$/  /      \  /      \  /      \  /      \  /       |/ $$   |  
$$ |      /$$$$$$  |/$$$$$$  |/$$$$$$  |/$$$$$$  |/$$$$$$$/ $$$$$$/   
$$ |   __ $$ |  $$ |$$ |  $$/ $$ |  $$/ $$    $$ |$$ |        $$ | __ 
$$ \__/  |$$ \__$$ |$$ |      $$ |      $$$$$$$$/ $$ \_____   $$ |/  |
$$    $$/ $$    $$/ $$ |      $$ |      $$       |$$       |  $$  $$/ 
 $$$$$$/   $$$$$$/  $$/       $$/        $$$$$$$/  $$$$$$$/    $$$$/";
            DrawPlayerState(text);
        }

        private static void DrawPlayerLost()
        {
            const string text = @"  
  ______                                           __                        __     
 /      \                                         /  |                      /  |    
/$$$$$$  |  ______   _____  ____    ______        $$ |  ______    _______  _$$ |_   
$$ | _$$/  /      \ /     \/    \  /      \       $$ | /      \  /       |/ $$   |  
$$ |/    | $$$$$$  |$$$$$$ $$$$  |/$$$$$$  |      $$ |/$$$$$$  |/$$$$$$$/ $$$$$$/   
$$ |$$$$ | /    $$ |$$ | $$ | $$ |$$    $$ |      $$ |$$ |  $$ |$$      \   $$ | __ 
$$ \__$$ |/$$$$$$$ |$$ | $$ | $$ |$$$$$$$$/       $$ |$$ \__$$ | $$$$$$  |  $$ |/  |
$$    $$/ $$    $$ |$$ | $$ | $$ |$$       |      $$ |$$    $$/ /     $$/   $$  $$/ 
 $$$$$$/   $$$$$$$/ $$/  $$/  $$/  $$$$$$$/       $$/  $$$$$$/  $$$$$$$/     $$$$/";
            DrawPlayerState(text);
        }

        private void Guess()
        {
            if (NumberOfGuesses == MaxNumberOfGuesses)
            {
                return;
            }

            var guess = global::System.Console.ReadLine().ToLower();
            if (guess.Length < 1)
            {
                return;
            }

            try
            {
                if (guess.Length > 1)
                {
                    Guess(guess);
                }
                else
                {
                    Guess(guess[0]);
                }

            }
            catch (IsNotLetterException e)
            {
                Console.WriteLine(e.Message);
                Guess();
            }
            catch (LetterAlreadyInUseException e)
            {
                Console.WriteLine(e.Message);
                Guess();
            }
            catch (WordAlreadyInUseException e)
            {
                Console.WriteLine(e.Message);
                Guess();
            }
        }

        private void Guess(string word)
        {
            if (word == WordToGuess)
            {
                _maskedSecretWord.Clear();
                _maskedSecretWord.Append(word);
            }
            else
            {
                if (_incorrectWordGuesses.Contains(word))
                {
                    throw new WordAlreadyInUseException("Word is already in use");
                }
                
                _incorrectWordGuesses.Add(word);
            }
        }

        private void Guess(char letter)
        {
            if (!char.IsLetter(letter))
            {
                throw new IsNotLetterException("Input is not a letter");
            }

            if (_correctGuesses.Contains(letter) || _incorrectGuesses.ToString().Contains(letter))
            {
                throw new LetterAlreadyInUseException("Letter is already in use");
            }

            if (!WordToGuess.Contains(letter))
            {
                _incorrectGuesses.Append(letter);
                return;
            }

            _correctGuesses.Add(letter);

            RevealToPlayer(letter);
        }

        private void DrawHangMan()
        {
            var pics = new string[]
            {
                "      |\n      |\n      |\n=========",
                "      |\n      |\n      |\n      |\n=========",
                "      |\n      |\n      |\n      |\n      |\n=========",
                "  +---+\n  |   |\n      |\n      |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n      |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n  |   |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|   |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|\\  |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|\\  |\n /    |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|\\  |\n / \\  |\n      |\n========="
            };

            if (NumberOfIncorrectGuesses <= 0 && MaxNumberOfGuesses != NumberOfGuesses) return;
            var index = MaxNumberOfGuesses == NumberOfGuesses ? pics.Length : NumberOfIncorrectGuesses;
            Console.WriteLine("\n\n" + pics[index - 1]);

            if (MaxNumberOfGuesses == NumberOfGuesses)
            {
                Console.ReadKey();
            }
        }

        private void DrawIncorrectGuesses()
        {
            if (_incorrectWordGuesses.Count > 0)
            {
                Console.WriteLine("Incorrect word guesses: {0}",
                    string.Join(", ", _incorrectWordGuesses.ToArray()));
            }

            if (_incorrectGuesses.Length > 0)
            {
                Console.WriteLine("Incorrect letter guesses: {0}", _incorrectGuesses);
            }
        }

        private void Draw()
        {
            Console.Clear();
            Console.WriteLine("Wins: {0} Loss {1}", _player.Wins, _player.Loss);
            DrawIncorrectGuesses();
            DrawHangMan();
            Console.WriteLine("\n{0} letters: {1}", _maskedSecretWord.Length, _maskedSecretWord);
            Console.WriteLine("\nGuesses: {0} of {1}", NumberOfGuesses, MaxNumberOfGuesses);
            Console.Write("Enter full word or a letter: ");
        }

        private static string[] ReadWordsFile()
        {
            const string path = @"words.txt";
            var readText = File.ReadAllText(path);
            return readText.Split(',');
        }

        private string[] SecretWords
        {
            get
            {
                if (_secreteWords != null) return _secreteWords;
                try
                {
                    _secreteWords = ReadWordsFile();
                }
                catch (Exception)
                {
                    _secreteWords = new[]
                    {
                        "svenska",
                        "lexicon",
                        "programmering"
                    };
                }

                return _secreteWords;
            }
        }

        private string WordToGuess => SecretWords[_secretWordIndex].ToLower();
    }
}
