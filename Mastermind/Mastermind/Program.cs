using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Mastermind Game
 * Player/AI has 12 turns to guess secret code
 * Player can be GUESSER or CREATOR
 * AI for guessing code
 * 
 * 
 * 
 * 
 * 
 * 
 */
namespace Mastermind
{
    class Program
    {      
        static void Main(string[] args)
        {
            // welcome message
            display("Welcome to Mastermind. Enter your name: ");

            User _user = new User(Console.ReadLine());
            AI computer = new AI();

            // instantiate new game
            Game game = new Game(_user, computer);

            // track game's progress
            bool game_over = false;

            // start game
            while (!game_over)
            {
                game_over = game.Start();
            }
            Console.ReadLine();
        }

        // display messages
        internal static void display(string message)
        {
            Console.WriteLine(message);
        }

        // display messages with name
        internal static void display(string message, string name)
        {
            Console.WriteLine(message, name);
        }
    }
    

    abstract class Player
    {
        // player can be a AI or User
        public string name { get; set; }
        public int turns = 12;
        public string hint_word { get; set; }
        public string hint_range { get; set; }

        public virtual int compute_guess()
        {
            return 0;
        }
    }
    
    sealed class User : Player
    {
        public User(string _name)
        {
            name = _name;
        }   
    }

    sealed class AI : Player
    {
        // track min and max of guess
        public int min { get; set; }
        public int max { get; set; }
        public int current_guess { get; set; }
        
        public AI()
        {
            name = "Computer";
        }

        public override int compute_guess()
        {
            Random rnd = new Random();

            // take first guess
            if (this.turns == 12)
            {
                // update min
                this.current_guess = rnd.Next(1, 100);
                this.min = this.current_guess;
               
                // return guess
                return this.current_guess;
            }
            
            else
            {
                set_range();
                this.current_guess = rnd.Next(this.min, this.max);
                return this.current_guess;
            }         
        }

        // computes hint word & range and returns the range for guess
        private void set_range()
        {
            // if hint is higher, we need to increase the min range
            if (this.hint_range == "Higher")
            {
                increase_range();
            }

            // if hint is lower, decrease the max range
            else
            {
                decrease_range();
            }
        }

        // increases range for next guess
        private void increase_range()
        {
            if (this.turns == 11)
            {
                this.max = 100;
            }

            else
            {
                this.min = this.current_guess + find_range();
                this.max += find_range();
            }
        }

        // decrease range for next guess
        private void decrease_range()
        {
            if (this.turns == 11)
            {
                this.max = this.min;
                this.min = 1;
            }

            else
            {
                this.max = this.current_guess - find_range();
                this.min -= find_range();
            }
        }

        // returns range for next random guess
        private int find_range()
        {
            switch (hint_word)
            {
                case "Cold":
                    return 30;
                case "Warm":
                    return 15;
                case "Warmer":
                    return 10;
                default:
                    return 5;
            }
        } 
   }

    class Game
    {
        public Player game_master { get; set; }
        public Player _player { get; set; }
        private int secret_code { get; set; }

        //constructor
        public Game(User _user, AI computer)
        {
            // determine game master
            Game_master(_user, computer);
        }

        public bool Start()
        {
            // track player's turns
            if (_player.turns == 0)
            {
                Program.display("Game Over. Try again!");
                return true;
            }

            else
            {               
                // notify tries
                Program.display("{0} Turn", ordinal_numbers(_player.turns));

                // ask for guess
                Program.display("Enter your guess: ");             
                int guess = _player.name == "Computer" ? _player.compute_guess() : Int32.Parse(Console.ReadLine());

                Console.WriteLine("{0}'s guess: {1}", _player.name, guess.ToString());

                // check for win
                if (isWon(guess))
                {
                    return true;
                }                
            }
            return false;
        }
        
        // display welcome message
        private void Welcome()
        {
            Program.display("{0} is the Game Master", this.game_master.name);
            Program.display("{0} has 12 turns to guess the secret code", this._player.name);
            Program.display("Let's PLAY!");
        }

        // checks for a win
        private bool isWon(int guess)
        {
            Console.Clear();
            if (guess == this.secret_code)
            {
                Program.display("{0} WINS!", this._player.name);
                return true;
            }

            else
            {
                // display guess results
                Program.display("Your {0} is incorrect.", guess.ToString());
                give_hint(guess);
                _player.turns--;               
                return false;
            }
        }

        // gives player a hint
        private void give_hint(int guess)
        {
            // find difference from guess and secret code
            int difference = this.secret_code - guess;

             //display hint corresponding to guess
            if (difference > 0)
            {
                _player.hint_word = hint_word(difference);
                _player.hint_range = "Higher";
                Program.display("Your guess is {0}. Go Higher!", _player.hint_word);           
            }

            else
            {
                _player.hint_word = hint_word(difference);
                _player.hint_range = "Lower";
                Program.display("Your guess is {0}. Go Lower!", _player.hint_word);
            }
        }

        // return Cold, Warm, Warmer, Hot
        private string hint_word(int num)
        {
            num = Math.Abs(num);

            // > 30 Cold
            if (num > 30)
            {
                return "Cold";
            }

            // between 15-30
            else if (num >= 15 && num < 30)
            {
                return "Warm";
            }

            // between 5-15
            else if (num > 5 && num < 15)
            {
                return "Warmer";
            }

            // less than 5
            else
            {
                return "HOT!";
            }            
        }

        // return corresponding string for num
        private string ordinal_numbers(int num)
        {
            switch(num)
            {
                case 12:
                    return "Frist";
                case 11:
                    return "Second";
                case 10:
                    return "Third";
                case 9:
                    return "Fourth";
                case 8:
                    return "Fifth";
                case 7:
                    return "Sixth";
                case 6:
                    return "Seventh";
                case 5:
                    return "Eigth";
                case 4:
                    return "Ninth";
                case 3:
                    return "Tenth";
                case 2:
                    return "Eleventh";
                default:
                    return "Twelth";
            }
        }

        // determine game master
        private void Game_master(User user, AI computer)
        {
            string selection = verify("Secret code generater? Player or AI?.\nUsage: Enter Player or AI",
                "Player", "AI");

            if (selection == "Player")
            {
                this.game_master = user;
                this._player = computer;
            }

            else
            {
                this.game_master = computer;
                this._player = user;
            }

            // ask game master for code generation
            generate_code(this.game_master.name);
            
            // welcome and display rules to player
            Welcome();
        }

        // generate secret code
        private void generate_code(string master)
        {
            Random rnd = new Random();

            // Player game master
            if (master != "Computer")
            {
                this.secret_code = verify("Code Generation Usage: \"Random\" or \"Selected\" ",
                    "Random", "Selected") == "Selected" ? select_code() : rnd.Next(1, 100);
            }
            
            // AI game master
            else
            {
                this.secret_code = rnd.Next(1, 100);
            }
        }

        // player selected code
        private int select_code()
        {
            Program.display("Enter your Secret Code:");
            return Int32.Parse(Console.ReadLine());
        }

        // accepts 2 options and asks user for preference, and return the user's preference
        private string verify(string message, string option1, string option2)
        {
            string value;

            do
            {
                Program.display(message);
                value = Console.ReadLine();
            } while (value != option1 && value != option2);

            return value;
        }
    }
}
