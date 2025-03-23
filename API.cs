namespace LocalizationTool
{
    public class API : IAPI
    {
        private readonly ILocalizationParser _parser;
        private Dictionary<string, Dictionary<string, string>> _originalKeys;
        private Dictionary<string, string> _originalfiles;
        private Dictionary<string, Dictionary<string, string>> _destinationKeys;
        private Dictionary<string, string> _destinationfiles;

        public API(ILocalizationParser parser)
        {
            _parser = parser;
            _originalKeys = new Dictionary<string, Dictionary<string, string>>();
            _destinationKeys = new Dictionary<string, Dictionary<string, string>>();
            _originalfiles = new Dictionary<string, string>();
            _destinationfiles = new Dictionary<string, string>();
        }

        public void LoadOriginal(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new Exception($"The folder with original translations does not exist: {folderPath}");

            _originalKeys.Clear();
            foreach (var file in Directory.GetFiles(folderPath, $"*.{folderPath}"))
            {
                string key = Path.GetFileNameWithoutExtension(file);

                _originalfiles[key] = file;
                _originalKeys[key] = _parser.ParseFile(file);
            }
            Console.WriteLine($"...Original translations from {folderPath} loaded");
        }

        public void LoadDestination(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new Exception($"The folder with target translations does not exist: {folderPath}");

            _destinationKeys.Clear();
            foreach (var file in Directory.GetFiles(folderPath, $"*.{folderPath}"))
            {
                string key = Path.GetFileNameWithoutExtension(file);
                _destinationfiles[key] = file;
                _destinationKeys[key] = _parser.ParseFile(file);
            }
            Console.WriteLine($"...Loaded target translations from {folderPath}");
        }

        public void CheckUntranslated(string outputFile)
        {
            if (_originalKeys.Count == 0)
                throw new Exception("First download the original translations using the '-original' command");

            var untranslated = new List<string>();
            foreach (var originalFileKey in _originalKeys.Keys)
            {

                var originalFileKeys = _originalKeys[originalFileKey];
                var destFileKeys = _destinationKeys.ContainsKey(originalFileKey) ? _destinationKeys[originalFileKey] : new Dictionary<string, string>();
                foreach (var key in originalFileKeys.Keys)
                {
                    if (!destFileKeys.ContainsKey(key))
                    {
                        untranslated.Add($"{key}={originalFileKeys[key]}");
                    }
                }
            }

            if (untranslated.Any())
            {
                File.WriteAllLines(outputFile, untranslated);
                Console.WriteLine($"Untranslated keys found. Saved to {outputFile}");
            }
            else
            {
                Console.WriteLine("All keys have already been translated.");
            }
        }

        public void SyncWithTranslations(string translationsFile)
        {
            if (_originalKeys.Count == 0 || _destinationKeys.Count == 0)
                throw new Exception("First download the original and target translations using the '-original' and '-dest' commands");

            if (!File.Exists(translationsFile))
                throw new Exception($"Translation file does not exist: {translationsFile}");

            Dictionary<string, string> additionalTranslations = _parser.ParseFile(translationsFile);

            foreach (var originalFileKey in _originalKeys.Keys)
            {
                string russianFilePath = _destinationfiles[originalFileKey];
                string originalFilePath = _originalfiles[originalFileKey];
                var originalFileKeys = _originalKeys[originalFileKey];
                Dictionary<string, string> russiankeys = _destinationKeys.ContainsKey(originalFileKey) ? _destinationKeys[originalFileKey] : new Dictionary<string, string>();

                var translated = russiankeys.Where(kv =>
                {
                    return originalFileKeys.ContainsKey(kv.Key);
                }).ToDictionary();

                foreach (var key in originalFileKeys.Keys)
                {
                    if (additionalTranslations.ContainsKey(key) && !translated.ContainsKey(key))
                    {
                        translated[key] = additionalTranslations[key];
                    }
                }

                List<string> syncedLines = new List<string>();
                foreach (var line in File.ReadAllLines(originalFilePath))
                {
                    string key = _parser.GetKeyFromLine(line);
                    if (string.IsNullOrEmpty(key))
                    {
                        syncedLines.Add(line);
                        continue;
                    }
                    syncedLines.Add($"{key}={translated[key]}");
                }

                File.WriteAllLines(russianFilePath, syncedLines);
            }

            Console.WriteLine("...Synchronization complete.");
        }
    }
}
