namespace LocalizationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new LocalizationParser();
            var manager = new API(parser);

            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLower();

                if (!arg.StartsWith("-") || i + 1 >= args.Length)
                {
                    Console.WriteLine($"Invalid command format: {arg}");
                    ShowUsage();
                    return;
                }
                try
                {
                    string value = args[++i];
                    switch (arg)
                    {
                        case "-original":
                            manager.LoadOriginal(value);
                            break;
                        case "-dest":
                            manager.LoadDestination(value);
                            break;
                        case "-check":
                            manager.CheckUntranslated(value);
                            break;
                        case "-sync":
                            manager.SyncWithTranslations(value);
                            break;
                        default:
                            Console.WriteLine($"Unknown command: {arg}");
                            ShowUsage();
                            return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("LocalizationTool.exe [-original <folder>] [-dest <folder>] [-check <file>] [-sync <file>]");
            Console.WriteLine("  -original <folder> - folder of the original localization (en folder & will search for (*.<dir>) files) ");
            Console.WriteLine("  -dest <folder> - folder оf target localization (rus folder, (*.<dir>) files)");
            Console.WriteLine("  -check <file> - get  <file> with untranslated keys (difference between target and original localization)");
            Console.WriteLine("  -sync <file> - apply <file> with translations to target localization");
            Console.WriteLine("Example: LocalizationTool.exe -original \"C:\\terraInvicta\\en\" -dest \"C:\\terraInvicta\\rus\" -check \"untranslated.txt\" -sync \"new_translations.txt\"");
        }
    }
}