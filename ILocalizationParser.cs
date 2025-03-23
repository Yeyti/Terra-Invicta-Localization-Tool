namespace LocalizationTool
{
    public interface ILocalizationParser
    {
        Dictionary<string, string> ParseFile(string filePath);
        string GetKeyFromLine(string line);
    }
}
