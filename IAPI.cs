namespace LocalizationTool
{
    public interface IAPI
    {
        void LoadOriginal(string folderPath);
        void LoadDestination(string folderPath);
        void CheckUntranslated(string outputFile);
        void SyncWithTranslations(string translationsFile);
    }
}
