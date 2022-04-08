using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunimoKart
{
    static class HighScores
    {
        private static List<KeyValuePair<string, int>> scores;

        public static void Save()
        {
            Properties.Settings.Default.HighScores = Serialize();
            Properties.Settings.Default.Save();
        }

        private static string Serialize()
        {
            return string.Join("\n", scores.Select(p => p.Key + ":" + p.Value).ToList());
        }

        private static void Deserialize(string value)
        {
            scores = new List<KeyValuePair<string, int>>();
            var lines = value.Split('\n');
            foreach (string line in lines)
            {
                var parts = line.Split(':');
                scores.Add(new KeyValuePair<string, int>(parts[0], int.Parse(parts[1])));
            }
        }

        private static void Load()
        {
            string scoresString = Properties.Settings.Default.HighScores;
            if (scoresString == null || scoresString.Length == 0)
            {
                LoadDefault();
                return;
            }
            Deserialize(scoresString);

        }

        private static void LoadDefault()
        {
            scores = new List<KeyValuePair<string, int>>();
            scores.Add(new KeyValuePair<string, int>("Lewis", 50000));
            scores.Add(new KeyValuePair<string, int>("Shane", 25000));
            scores.Add(new KeyValuePair<string, int>("Sam", 10000));
            scores.Add(new KeyValuePair<string, int>("Abigail", 5000));
            scores.Add(new KeyValuePair<string, int>("Vincent", 250));
        }

        public static List<KeyValuePair<string, int>> Get()
        {
            if (scores == null) Load();
            return scores;
        }

        public static void AddScore(string v, int score)
        {
            Get();
            scores.Add(new KeyValuePair<string, int>(v, score));
            scores.Sort((a, b) => b.Value - a.Value);
            while (scores.Count > 5) scores.RemoveAt(5);
            Save();
        }
    }
}
