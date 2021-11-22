using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
//TODO Ändra height och width så att det blir tydligare
//TODO Skapa high score lista
//TODO Kolla över utskrift av spelplan
//TODO gör en större Game Over Text
//TODO gör ett välkomstmeddelande
//TODO lägg till större frukt med mer poäng kanske under viss tid
//TODO lägg till "onda" frukter
//TODO Skapa svårighetsgrad/hastighet
namespace Snake
{
    class Program 
    {
        private static int height = 20;
        private static int width = 40;
        static Dictionary<string, Point> posDict = new Dictionary<string, Point>();
        private static List<Point> tailPos = new List<Point>();
        private static Direction direction = Direction.Stop;
        static bool newXBool = false;
        static bool gameOver = false;
        static bool isRunning = true;
        static string input;
        
        private enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Stop
        }
        static void Main(string[] args)
        {
            HighScore hs = new HighScore("highscore.txt");
            
            while(isRunning)
            {
                Setup();
                hs.Print(45, 7);
                while (!gameOver)
                {
                    Draw();
                    GetKey();
                    UpdatePos();
                    Thread.Sleep(100);
                }
                Console.Clear();
                Console.WriteLine($"\n\t\tGAME OVER!\n\n\t\t You got: {tailPos.Count()} points!");
                if (hs.CheckScore(tailPos.Count()))
                {
                    Console.Write("\n\t\tGrattis! Du kom in i highscoren!\n\t\tSkriv in ditt namn: ");
                    hs.AddToHighScore(new Player(Console.ReadLine(), tailPos.Count()));
                }
                Console.WriteLine();
                hs.Print();     
                do
                {
                    Console.Write("\n\t\tDo you want to play again? [yes/no]: ");
                    input = Console.ReadLine().ToLower();
                } while (input != "yes" && input != "no");
                isRunning = input == "no" ? false : true;         
            }
            hs.Save();
        }
        static void Setup()
        {
            Console.Clear();
            Console.CursorVisible = false;
            gameOver = false;
            newXBool = false;
            tailPos.Clear();
            tailPos.Add(new Point(20, 10));
            posDict.Clear();
            posDict.Add("head", new Point(20, 9));
            posDict.Add("fruit", RandomPos());
            posDict.Add("X", new Point(35, 35));
            direction = Direction.Stop;
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c  < width; c++)
                {
                    if ((r == 0 && c == 0) || (r == height - 1 && c == width -1) || (r == 0 && c == width - 1) || (r == height - 1 && c == 0))
                    {
                        Console.Write("+");
                    }
                    else if (r == 0 || r == height - 1)
                    {
                        Console.Write("-");
                    }
                    else if (c == 0 || c == width - 1)
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            for (int r = 0; r < height - 2; r++)
            {
                Console.SetCursorPosition(1, r + 1);
                for (int c = 0; c < width - 1; c++)
                {
                    if (c == 0)
                    {

                    }
                    else
                    {
                        Console.Write("X", Console.ForegroundColor = ConsoleColor.Blue);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }
        static void Draw()
        {
            if (newXBool)
            {
                Write(posDict["X"], "X", ConsoleColor.Blue);
            }
            Write(posDict["fruit"], "*", ConsoleColor.Cyan);
            Write(posDict["head"], "0", ConsoleColor.Yellow);          
            foreach (var item in tailPos)
            {
                Write(item, "o", ConsoleColor.DarkYellow);
            }
            Write(new Point(45, 5), $"Score: {tailPos.Count()}", ConsoleColor.White);
        }
        static void Write(Point pos, string str, ConsoleColor color)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(str, Console.ForegroundColor = color);
            Console.ResetColor();
        }
        static void UpdatePos()
        {
            if (direction == Direction.Stop) { return; }
            if (tailPos.Contains(posDict["head"])) { gameOver = true; return; }
            tailPos.Add(posDict["head"]);
            if(posDict["head"] != posDict["fruit"])
            {
                newXBool = true;
                posDict["X"] = tailPos[0];
                tailPos.RemoveAt(0);
            }
            else
            {
                newXBool = false;
                posDict["fruit"] = RandomPos();
            }

            if (direction == Direction.Up)
            {
                if(posDict["head"].Y == 1) { posDict["head"] = new Point(posDict["head"].X, 18); }
                else { posDict["head"] = new Point(posDict["head"].X, posDict["head"].Y - 1); }     
            }
            else if(direction == Direction.Down)
            {
                if(posDict["head"].Y == 18) { posDict["head"] = new Point(posDict["head"].X, 1); }
                else { posDict["head"] = new Point(posDict["head"].X, posDict["head"].Y + 1); }
            }
            else if(direction == Direction.Left)
            {
                if(posDict["head"].X == 1) { posDict["head"] = new Point(38, posDict["head"].Y); }
                else { posDict["head"] = new Point(posDict["head"].X - 1, posDict["head"].Y); }
            }
            else if (direction == Direction.Right)
            {
                if(posDict["head"].X == 38) { posDict["head"] = new Point(1, posDict["head"].Y); }
                else { posDict["head"] = new Point(posDict["head"].X + 1, posDict["head"].Y); }
            }
        }
        static void GetKey()
        {
            if (!Console.KeyAvailable) { return; }

            var key = Console.ReadKey(false).Key;
            
            if(key == ConsoleKey.UpArrow)
            {
                direction = Direction.Up;
            }
            else if(key == ConsoleKey.DownArrow)
            {
                direction = Direction.Down;
            }
            else if(key == ConsoleKey.LeftArrow)
            {
                direction = Direction.Left;
            }
            else if(key == ConsoleKey.RightArrow)
            {
                direction = Direction.Right;
            }
            else if(key == ConsoleKey.Enter)
            {
                direction = Direction.Stop;
            }
        }
        static Point RandomPos()
        {
            Random rnd = new Random();
            Point pos = new Point();
            pos.X = rnd.Next(2, 39);
            pos.Y = rnd.Next(2, 19);
            if (tailPos.Contains(pos) || posDict["head"] == pos) { RandomPos(); }
            return pos;
        }
    }
}
