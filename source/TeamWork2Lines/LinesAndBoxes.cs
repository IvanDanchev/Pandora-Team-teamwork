using System;
using System.Collections.Generic;
using System.Media;
using System.IO;

namespace Lines
{
    class LinesGame
    {
        static void Main()
        {
            PrintWelcomeScreen();

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();   // paints the console gray and text black
            Console.SetCursorPosition(0, 0);
            Console.Write("Please enter the table's size (number of cells in a row (2-10) ):");
            int tableSize = InputExceptions();

            Console.Clear();   // clears the messages 

            int[,] tableMatrix = new int[tableSize * 2 + 2, tableSize * 2 + 2];  // the matrix containing all elements (dots, cell walls and cell interiors)
            int[,] cellsMatrix = new int[tableSize * 2 + 2, tableSize * 2 + 2];  // this matrix keeps the number of walls for each cell 

            /* by default every element has a 0 value
             the first row (tableMatrix[0, ...]) and the first column (tableMatrix[..., 0]) will not be used in the game
             the table starts from row 1 and col 1
             every element with [odd, odd] index  will be a dot (+) (example: [1, 1] or [1, 3])
             every element with [odd, even] or [even, odd] index will be a wall
             every element with [even, even] index will be a cell interior
            */
            PrintRules(tableSize);
            PrintTable(tableSize);

            int currentRow = 1;         //variables that keep the position on the table
            int currentCol = 1;
            int lastRow = currentRow;
            int lastCol = currentCol;
            int playerTurn = 1;         // holds values 1 or 2, corresponding to Player 1 and Player 2
            int player1CellsCount = 0;  // variables that keep the number of closed cells 
            int player2CellsCount = 0;

            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                Console.SetCursorPosition(0, tableSize * 2 + 4);
                Console.Write("Player {0}'s turn:", playerTurn);
                Console.SetCursorPosition(currentCol, currentRow);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentRow > 1)
                        {
                            currentRow--;
                            DisplayCursorPosition(tableMatrix, currentRow, currentCol, lastRow, lastCol, "up");
                            lastRow = currentRow;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentRow < tableSize * 2 + 1)
                        {
                            currentRow++;
                            DisplayCursorPosition(tableMatrix, currentRow, currentCol, lastRow, lastCol, "down");
                            lastRow = currentRow;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (currentCol < tableSize * 2 + 1)
                        {
                            currentCol++;
                            DisplayCursorPosition(tableMatrix, currentRow, currentCol, lastRow, lastCol, "right");
                            lastCol = currentCol;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (currentCol > 1)
                        {
                            currentCol--;
                            DisplayCursorPosition(tableMatrix, currentRow, currentCol, lastRow, lastCol, "left");
                            lastCol = currentCol;
                        }
                        break;

                    case ConsoleKey.X:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:    // this case is where the big part of the magic happens, and where most of the methods are used
                        if (((currentRow % 2) > 0 ^ (currentCol % 2) > 0) && (tableMatrix[currentRow, currentCol] == 0))
                        {
                            tableMatrix[currentRow, currentCol] = 1;
                            DrawWall(tableMatrix, cellsMatrix, playerTurn, currentRow, currentCol);
                            WriteWallDataToMatrices(cellsMatrix, tableMatrix, tableSize, currentRow, currentCol, playerTurn);
                            int countCells = 0;
                            countCells = CountAndPrintClosedCells(tableMatrix, tableSize, playerTurn, countCells);

                            if (playerTurn == 1)
                            {
                                if (player1CellsCount < countCells)
                                {
                                    player1CellsCount = countCells;

                                }
                                else if (player1CellsCount == countCells)
                                {
                                    playerTurn = 2;
                                }
                            }
                            else if (playerTurn == 2)
                            {
                                if (player2CellsCount < countCells)
                                {
                                    player2CellsCount = countCells;
                                }
                                else if (player2CellsCount == countCells)
                                {
                                    playerTurn = 1;
                                }
                            }
                        }
                        PrintCurrentResult(tableSize, player1CellsCount, player2CellsCount, currentRow, currentCol);
                        break;
                }
                PrintScore(player1CellsCount, player2CellsCount, tableSize);
                if (player1CellsCount + player2CellsCount == tableSize * tableSize)
                {
                    break;
                }
            }
            ScoreToText(player1CellsCount, player2CellsCount);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, tableSize * 2 + 9);
            Console.Write("Press F5 to play again, or Esc to exit.");
            if ((keyInfo = Console.ReadKey(true)).Key == ConsoleKey.F5)
            {
                Main();
            }
            if ((keyInfo = Console.ReadKey(true)).Key == ConsoleKey.Escape)
            {
                Console.Clear();
                Environment.Exit(0);
            }
            Console.SetCursorPosition(0, 22);
        }

        static void PrintWelcomeScreen()
        {
            Console.Clear();
            Console.Title = "Pandora Lines";
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(80, 30);
            Console.SetWindowSize(80, 30);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"
█████████   ██████    ████     ██ ████████   ███████ █████████    ██████
██     ██  ██    ██   ██  ██   ██ ██      ██ ██   ██ ██     ██   ██    ██
█████████  ██    ██   ██   ██  ██ ██      ██ ██   ██ █████████   ██    ██
██        ██████████  ██    ██ ██ ██      ██ ██   ██ ██ ████    ██████████ 
██       ██        ██ ██      ███ ████████   ███████ ██  █████ ██        ██

                  ███      ██████  █████    ██  ████  ███████
                  ███        ██    ███  █   ██  ██    ██ 
                  ███        ██    ███   █  ██  ████  ███████
                  ███████  ██████  ███    ████  ██         ██
      TELERIKACADEMY                            ████  ███████
C#2 TEAMWORK - TEAM PANDORA
███████████████████████████████████████████████████████████████████████████████
                                         
           ▒▒▒▒▓▒▒▓▒▒▒▒       ▒▒▒███▒▒██▒▒███▒▒▒      ░▓░▓░▓░▓░▓░▓ 
           ▒▒▒▒▓▒▒▓▒▒▒▒       ▒▒██▒▒▒▒██▒▒▒▒██▒▒      ▓░▓░▓░▓░▓░▓░
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒██▒▒▒▒▒██▒▒▒▒▒██▒      ░▓░▓░▓░▓░▓░▓
           ▒▓▒▒▒▒▒▒▒▒▓▒       ▒██▒▒▒▒████▒▒▒▒██▒      ▓░▓░▓░▓░▓░▓░
           ▒▒▓▓▓▓▓▓▓▓▒▒       ▒██▒▒▒██████▒▒▒██▒      ░▓░▓░▓░▓░▓░▓
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒▒██▒██▒██▒██▒██▒▒      ░▓░▓░▓░▓░▓░▓
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒▒▒███▒▒██▒▒███▒▒▒      ░▓░▓░▓░▓░▓░▓
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒▒▒▒▒████████▒▒▒▒▒      ░▓░▓░▓░▓░▓░▓
         Player vs Player     Player vs Computer     History of games
            (Press P)             (Press C)             (Press H)
");
            Console.ForegroundColor = ConsoleColor.Black;
            ConsoleKeyInfo key;
            Console.WriteLine();
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                switch (key.Key)
                {
                    case ConsoleKey.P: PlayPVP(); break;
                    case ConsoleKey.C: PlayPC(); break;
                    case ConsoleKey.H: History(); break;
                    default: continue;
                }
                break;
            }
            if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }

        static void PlayPC()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Currently Not implemented, Wait for Patch");
            Console.WriteLine("Press Any Key to return to menu");
            ConsoleKeyInfo keyInfo;
            if ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                Main();
            }
        }

        static void PlayPVP()
        {
            Console.WriteLine("This is play vs player");
        }

        static void History()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines("../../scores.txt");
                foreach (var line in lines)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\t" + line);
                    Console.WriteLine("Press Any Key to Return to Menu");
                    ConsoleKeyInfo keyInfo;
                    keyInfo = Console.ReadKey();
                    Main();
                }
            }
            catch (FileNotFoundException)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("There is no previous Highscore");
                Console.WriteLine("Press Any Key to Return to Menu");
                ConsoleKeyInfo keyInfo;
                keyInfo = Console.ReadKey();
                Main();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void PrintRules(int tableSize)
        {
            try
            {
                Console.SetCursorPosition(tableSize * 2 + 8, 1);
                Console.Write("Press the arrow keys to move.");
                Console.SetCursorPosition(tableSize * 2 + 8, 2);
                Console.Write("Press Enter to place a wall.");
                Console.SetCursorPosition(tableSize * 2 + 8, 3);
                Console.Write("Close a cell to get an additional move.");
                Console.SetCursorPosition(tableSize * 2 + 8, 4);
                Console.Write("Close more cells than the enemy to win.");
                Console.SetCursorPosition(0, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

        }

        static void PrintTable(int tableSize)
        {
            Console.WriteLine();
            for (int row = 0; row <= tableSize; row++)   // printing the dots (+) of the empty table
            {
                for (int col = 0; col <= tableSize; col++)
                {
                    Console.Write(" +");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void DisplayCursorPosition(int[,] tableMatrix, int currentRow, int currentCol, int lastRow, int lastCol, string moveDirection)
        { // this method draws the current selected dot or wall
            Console.CursorVisible = false;
            switch (moveDirection)
            {
                case "up":

                case "down":
                    if ((lastRow % 2) > 0 && (currentCol % 2) > 0)   // if the element is a dot (odd coordinates)
                    {
                        Console.SetCursorPosition(currentCol, lastRow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("+");

                        Console.SetCursorPosition(currentCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if ((lastRow % 2 == 0) && (currentCol % 2 > 0))   // if the element is a vertical wall
                    {
                        Console.SetCursorPosition(currentCol, lastRow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (tableMatrix[lastRow, currentCol] == 1)   // this checks if there is a wall already placed here
                        {
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(currentCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("+");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if ((lastRow % 2 > 0) && (currentCol % 2 == 0))   // if the element is a horizontal wall
                    {
                        Console.SetCursorPosition(currentCol, lastRow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (tableMatrix[lastRow, currentCol] == 1)
                        {
                            Console.Write("—");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(currentCol, currentRow);
                    }
                    else if ((lastRow % 2 == 0) && (currentCol % 2 == 0))   // the element is a cell interior
                    {
                        Console.SetCursorPosition(currentCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("—");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    break;

                case "left":

                case "right":
                    if ((currentRow % 2) > 0 && (lastCol % 2) > 0)
                    {
                        Console.SetCursorPosition(lastCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("+");

                        Console.SetCursorPosition(currentCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("—");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if ((currentRow % 2 == 0) && (lastCol % 2 > 0))
                    {
                        Console.SetCursorPosition(lastCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (tableMatrix[currentRow, lastCol] == 1)
                        {
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(currentCol, currentRow);
                    }
                    else if ((currentRow % 2 > 0) && (lastCol % 2 == 0))
                    {
                        Console.SetCursorPosition(lastCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (tableMatrix[currentRow, lastCol] == 1)
                        {
                            Console.Write("—");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(currentCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("+");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if ((currentRow % 2) == 0 && (lastCol % 2) == 0)
                    {
                        Console.SetCursorPosition(currentCol, currentRow);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    break;
            }
        }

        static void DrawWall(int[,] tableMatrix, int[,] cellsMatrix, int playerTurn, int currentRow, int currentCol)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            if ((currentRow % 2 > 0) && (currentCol % 2 == 0))
            {
                Console.SetCursorPosition(currentCol, currentRow);
                Console.Write("—");
            }
            else
            {
                Console.SetCursorPosition(currentCol, currentRow);
                Console.Write("|");
            }
        }

        static int[,] WriteWallDataToMatrices(int[,] cellsMatrix, int[,] tableMatrix, int tableSize, int currentRow, int currentCol, int playerTurn)
        {   // this method writes the number of walls each cell has (1, 2, 3 or 4) to cellsMatrix then checks if 
            // a cell is closed (has 4 walls) and if it is, it changes the default cell value (0) in tableMatrix to 1 or 2
            if ((currentRow % 2 > 0) && (currentRow > 1) && (currentRow < (2 * tableSize + 1)))
            {
                cellsMatrix[currentRow - 1, currentCol]++;
                cellsMatrix[currentRow + 1, currentCol]++;
                if (cellsMatrix[currentRow - 1, currentCol] == 4)
                {
                    tableMatrix[currentRow - 1, currentCol] = playerTurn;
                }
                if (cellsMatrix[currentRow + 1, currentCol] == 4)
                {
                    tableMatrix[currentRow + 1, currentCol] = playerTurn;
                }
            }
            else if (currentRow == 1)
            {
                cellsMatrix[currentRow + 1, currentCol]++;
                if (cellsMatrix[currentRow + 1, currentCol] == 4)
                {
                    tableMatrix[currentRow + 1, currentCol] = playerTurn;
                }
            }
            else if (currentRow == (2 * tableSize + 1))
            {
                cellsMatrix[currentRow - 1, currentCol]++;
                if (cellsMatrix[currentRow - 1, currentCol] == 4)
                {
                    tableMatrix[currentRow - 1, currentCol] = playerTurn;
                }
            }

            if ((currentCol % 2 > 0) && (currentCol > 1) && (currentCol < (2 * tableSize + 1)))
            {
                cellsMatrix[currentRow, currentCol + 1]++;
                cellsMatrix[currentRow, currentCol - 1]++;
                if (cellsMatrix[currentRow, currentCol + 1] == 4)
                {
                    tableMatrix[currentRow, currentCol + 1] = playerTurn;
                }
                if (cellsMatrix[currentRow, currentCol - 1] == 4)
                {
                    tableMatrix[currentRow, currentCol - 1] = playerTurn;
                }
            }
            else if (currentCol == 1)
            {
                cellsMatrix[currentRow, currentCol + 1]++;
                if (cellsMatrix[currentRow, currentCol + 1] == 4)
                {
                    tableMatrix[currentRow, currentCol + 1] = playerTurn;
                }
            }
            else if (currentCol == (2 * tableSize + 1))
            {
                cellsMatrix[currentRow, currentCol - 1]++;
                if (cellsMatrix[currentRow, currentCol - 1] == 4)
                {
                    tableMatrix[currentRow, currentCol - 1] = playerTurn;
                }
            }
            return cellsMatrix;
        }

        static int CountAndPrintClosedCells(int[,] tableMatrix, int tableSize, int playerTurn, int countCells)
        {
            switch (playerTurn)
            {
                case 1:
                    for (int row = 2; row < (tableSize * 2 + 2); row += 2)
                    {
                        for (int col = 2; col < (tableSize * 2 + 2); col += 2)
                        {
                            if (tableMatrix[row, col] == 1)
                            {
                                countCells++;
                                Console.SetCursorPosition(col, row);
                                Console.BackgroundColor = ConsoleColor.Blue; ;
                                Console.WriteLine(" ");
                                Console.BackgroundColor = ConsoleColor.Gray;
                            }
                        }
                    }
                    break;

                case 2:
                    for (int row = 2; row < (tableSize * 2 + 2); row += 2)
                    {
                        for (int col = 2; col < (tableSize * 2 + 2); col += 2)
                        {
                            if (tableMatrix[row, col] == 2)
                            {
                                countCells++;
                                Console.SetCursorPosition(col, row);
                                Console.BackgroundColor = ConsoleColor.Red; ;
                                Console.WriteLine(" ");
                                Console.BackgroundColor = ConsoleColor.Gray;
                            }
                        }
                    }
                    break;
            }
            return countCells;
        }

        static void PrintCurrentResult(int tableSize, int player1CellsCount, int player2CellsCount, int currentRow, int currentCol)
        {
            Console.SetCursorPosition(0, tableSize * 2 + 6);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Player 1: {0}", player1CellsCount);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.Write("Player 2: {0}", player2CellsCount);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(currentCol, currentRow);
        }
        static void PrintScore(int player1CellsCount, int player2CellsCount, int tableSize)
        {
            if (player1CellsCount + player2CellsCount == Math.Pow(tableSize, 2))
            {
                if (player1CellsCount > player2CellsCount)
                {
                    Console.SetCursorPosition(0, tableSize * 2 + 9);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Player 1 wins!");
                    (new SoundPlayer(@"..\..\WinSound.wav")).PlaySync();
                }
                else if (player1CellsCount < player2CellsCount)
                {
                    Console.SetCursorPosition(0, tableSize * 2 + 9);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Player 2 wins!");
                    (new SoundPlayer(@"..\..\WinSound.wav")).PlaySync();
                }
                else
                {
                    Console.SetCursorPosition(0, tableSize * 2 + 9);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("It's a draw!");
                }
            }
        }
        static int InputExceptions()
        {
            int tableSize = 0;
            try
            {
                tableSize = int.Parse(Console.ReadLine());   // tableSize is the number of cells in a row
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid number!");
                Console.WriteLine("Enter \"r\" to enter a new number.");
                if (Console.ReadLine() == "r")
                {
                    Console.Clear();
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number!");
                Console.WriteLine("Enter \"r\" to enter a new number.");
                if (Console.ReadLine() == "r")
                {
                    Console.Clear();
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Enter \"r\" to enter a new number.");
                if (Console.ReadLine() == "r")
                {
                    Console.Clear();
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Invalid number!");
                Console.WriteLine("Enter \"r\" to enter a new number.");
                if (Console.ReadLine() == "r")
                {
                    Console.Clear();
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }

            }
            if (tableSize > 10 || tableSize < 2)
            {
                Console.WriteLine("Invalid number!");
                Console.WriteLine("Enter \"r\" to enter a new number.");
                if (Console.ReadLine() == "r")
                {
                    Console.Clear();
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            return tableSize;
        }

        static void ScoreToText(int PlayerScore1, int PlayerScore2)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("../../scores.txt", true))
                {
                    writer.WriteLine("[{0}] Player 1 :{1}    Player 2 :{2}", DateTime.Now, PlayerScore1, PlayerScore2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}