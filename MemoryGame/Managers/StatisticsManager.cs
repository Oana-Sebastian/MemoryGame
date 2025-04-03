using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MemoryGame.Model;

namespace MemoryGame.Managers
{
    public static class StatisticsManager
    {
        private static readonly string statsPath = "statistics.json";

        public static ObservableCollection<User> LoadStatistics()
        {
            if (File.Exists(statsPath))
            {
                var stats = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText(statsPath));
                return stats ?? new ObservableCollection<User>();
            }
            return new ObservableCollection<User>();
        }

        public static void SaveStatistics(ObservableCollection<User> statistics)
        {
            File.WriteAllText(statsPath, JsonSerializer.Serialize(statistics, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void RemoveUserStatistics(string username)
        {
            var stats = LoadStatistics();
            var statToRemove = stats.FirstOrDefault(s => s.Username == username);
            if (statToRemove != null)
            {
                stats.Remove(statToRemove);
                SaveStatistics(stats);
            }
        }
    }
}
