using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Snake
{
    class HighScore
    {
        private List<Player> highScore = new List<Player>();
        private string fileName = string.Empty;

        public HighScore(string fileName)
        {
            this.fileName = fileName;
            Load();
        }
        private void Load()
        {
            if(File.Exists(fileName))
            {
                string[] loadedLines = File.ReadAllLines(fileName);
                foreach (string line in loadedLines)
                {
                    string[] splitLine = line.Split("###");
                    Player p = new Player(splitLine[0], Convert.ToInt32(splitLine[1]));
                    highScore.Add(p);                    
                }
            }
        }
        public void Print()
        {
            for (int i = 1; i <= highScore.Count; i++)
            {
                Console.WriteLine($"\t\t{i}. {highScore[i - 1].Info()}");
            }
        }
        public void Print(int x, int y)
        {
            for (int i = 1; i <= highScore.Count; i++)
            {
                Console.SetCursorPosition(x, y);
                Console.WriteLine($"{i}. {highScore[i - 1].Info()}");
                y++;
            }
        }
        public void Save()
        {
            if(File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            if (highScore.Count > 0)
            {
                File.WriteAllText(fileName, $"{highScore[0].name}###{highScore[0].score}");
                for (int i = 1; i < highScore.Count - 1; i++)
                {
                    File.AppendAllText(fileName, $"\n{highScore[i].name}###{highScore[i].score}");
                } 
            }
        }
        private void SortList()
        {
            highScore.Sort((p1, p2) => p1.score.CompareTo(p2.score));
            highScore.Reverse();
        }
        public bool CheckScore(int score)
        {
            SortList();
            if (highScore.Count < 10)
                return true;
            else if (score > highScore[highScore.Count - 1].score)
                return true;
            else
                return false;
        }
        public void AddToHighScore(Player p)
        {
            if (highScore.Count >= 10)
                highScore.RemoveAt(highScore.Count - 1);
            highScore.Add(p);
            SortList();
        }
    }
}
