using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UI_Forms
{
    public static class SlotResponseCache
    {
        private static readonly object _lock = new object();
        private static readonly string _cachePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "UI_Forms",
            "slot_responses.json"
        );

        private static Dictionary<string, string> _responses;

        public static string GetResponse(string userId, string slotId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(slotId)) return null;

            lock (_lock)
            {
                EnsureLoaded();
                string response;
                return _responses.TryGetValue(BuildKey(userId, slotId), out response) ? response : null;
            }
        }

        public static void SetResponse(string userId, string slotId, string response)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(slotId)) return;

            lock (_lock)
            {
                EnsureLoaded();
                _responses[BuildKey(userId, slotId)] = string.IsNullOrWhiteSpace(response) ? "maybe" : response;
                Save();
            }
        }

        private static string BuildKey(string userId, string slotId)
        {
            return $"{userId.Trim().ToLowerInvariant()}|{slotId.Trim().ToLowerInvariant()}";
        }

        private static void EnsureLoaded()
        {
            if (_responses != null) return;

            try
            {
                if (File.Exists(_cachePath))
                {
                    string json = File.ReadAllText(_cachePath);
                    _responses = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
            }
            catch
            {
                _responses = null;
            }

            if (_responses == null)
            {
                _responses = new Dictionary<string, string>();
            }
        }

        private static void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_cachePath));
            string json = JsonSerializer.Serialize(_responses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_cachePath, json);
        }
    }
}
