namespace LocalizationTool
{
    public class LocalizationParser : ILocalizationParser
    {
        public Dictionary<string, string> ParseFile(string filePath)
        {
            var keys = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                string key = GetKeyFromLine(line);
                if (!string.IsNullOrEmpty(key))
                {
                    keys[key] = line.Substring(key.Length + 1).Trim();
                }
            }
            return keys;
        }

        public string GetKeyFromLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains("=")) return null;
            return line.Substring(0, line.IndexOf('=')).Trim();
        }
    }
}
