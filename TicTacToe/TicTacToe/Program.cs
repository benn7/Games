using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate Game
            TicTacToe game = new TicTacToe();
            game.Start();

            Console.ReadLine();                   
        }
    }

    // Player Class
    class Player
    {
        public string name { get; set; }
        public char marker { get; set; }

        public Player(int player, char marker)
        {
            this.marker = marker;
            this.name = getName(player);
        }

        // get player's name
        private string getName(int player)
        {
            Console.WriteLine("Enter Player's {0} name:", player);
            string name = Console.ReadLine();

            return (verifyName(name)) ? name : getName(player);
        }
    
        // ensure we don't have an empty name
        private bool verifyName(string name)
        {
            return name != "";
        }
    }
    
    // Gameboard Class
    class Gameboard
    {
        public char[] grid { get; set; }

        // constructor
        public Gameboard()
        {
            this.grid = new char[9];
            init_grid(this.grid.Length);
        }

        // draws board
        public void draw_board()
        {
            for (int i = 0; i < 9; i++)
            {
                if ((i+1) % 3 == 0)
                {
                    Console.WriteLine(grid[i]);
                }

                else if (i != 6 || i != 7 || i != 8)
                {
                    Console.Write(grid[i]);
                    Console.Write('|');
                }
            }
        }

        // initialize grid
        private void init_grid(int d)
        {
            for (int i = 0; i < d; i++)
            {
                this.grid[i] = ' ';
            }
        }
    }


    // Abstract Game class
    abstract class Game
    {
        // Game display functions
        public void display(string message)
        {
            Console.WriteLine(message);
        }

        public void display(string message, string name)
        {
            Console.WriteLine(message, name);
        }
    }

    // TicTacToe Class
    class TicTacToe : Game
    {
        public Gameboard board { get; set; }
        public Player player1 { get; set; }
        public Player player2 { get; set; }

        // creates a new TicTacToe Game
        public TicTacToe()
        {
            // Welcome message
            display("Welcome To Tic Tac Toe");

            // instantiate board
            this.board = new Gameboard();

            // instantiate Players
            this.player1 = new Player(1, 'X');
            this.player2 = new Player(2, 'O');
        }

        public void Start()
        {
            // track game's progress
            bool game_over = false;

            // start game
            while (!game_over)
            {
                // player1's turn
                game_over = execute_turn(player1);

                // player2's turn if.. player1 didn't win
                if (!game_over)
                {
                    game_over = execute_turn(player2);
                }
            }
        }

        // move pieces
        public bool Move(int tile, char marker)
        {
            // check if tile is occupied
            if (isOccupied(tile))
            {
                return false;
            }

            // else make a move
            else
            {
                // place marker
                board.grid[tile] = marker;
                return true;
            }
        }

        // checks for a win
        public bool IsWon(char marker)
        {
            return isHorizontalVictory(marker) || isVerticalVictory(marker) || isDiagonalVictory(marker);
        }

        // checks for occupied tiles
        private bool isOccupied(int tile)
        {
            return board.grid[tile] != ' ';
        }

        private bool execute_turn(Player player)
        {
            // player 1 goes first
            display("{0}'s turn to move", player.name);

            // selected tile for movement
            int tile = 0;

            // verify a complete turn
            bool turn_complete = false;

            do
            {
                // verify's tile selection
                do
                {
                    display("Select a tile from (topLeft)1 through 9(bottomRight)");
                    tile = Int32.Parse(Console.ReadLine());

                } while (tile < 1 || tile > 9);

                // checks for valid move
                if (Move(tile - 1, player.marker))
                {
                    Console.Clear();
                    this.board.draw_board();
                    turn_complete = true;

                    // check for win
                    if (IsWon(player.marker))
                    {
                        display("{0} WINS!!", player.name);
                        return true;
                    }
                }

                else
                {
                    Console.WriteLine("tile {0} is occupied!", tile);
                }

            } while (!turn_complete);

            // check for tie game
            return isTieGame();
        }

        private bool isHorizontalVictory(char marker)
        {
            return (board.grid[0] == marker && board.grid[1] == marker && board.grid[2] == marker) ||
                   (board.grid[3] == marker && board.grid[4] == marker && board.grid[5] == marker) ||
                   (board.grid[6] == marker && board.grid[7] == marker && board.grid[8] == marker);
        }

        private bool isVerticalVictory(char marker)
        {
            return (board.grid[0] == marker && board.grid[3] == marker && board.grid[6] == marker) ||
                   (board.grid[1] == marker && board.grid[4] == marker && board.grid[7] == marker) ||
                   (board.grid[2] == marker && board.grid[5] == marker && board.grid[8] == marker);
        }

        private bool isDiagonalVictory(char marker)
        {
            return (board.grid[0] == marker && board.grid[4] == marker && board.grid[8] == marker) ||
                   (board.grid[2] == marker && board.grid[4] == marker && board.grid[6] == marker);
        }

        // checks if all spaces on grid contains markers
        private bool isTieGame()
        {
            foreach (char mark in board.grid)
            {
                if (mark == ' ')
                {
                    return false;
                }
            }
            display("TIE GAME!");
            return true;
        }

    }


}
