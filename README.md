# Terra-Invicta-Localization-Tool
A tool for collecting untranslated data and integrating it into localization files.

## Usage
1. Download the latest release from the [releases page](https://github.com/Yeyti/Terra-Invicta-Localization-Tool/releases/tag/release) or compil the source code.

2. Run the program with the following command:
```LocalizationTool.exe -original "en" -dest "rus" -check "untranslated.txt"```  

    Where: `en` - path to original language folder, `rus` - path to target language folder, `untranslated.txt` - path to file with untranslated data.

3. translate keys from untranslated.txt manually or with the help of a language model (they can preserve formatting). You can use Gemeni/Grock/ChatGpt and the following request:
 `Translate localization file to <language name> preserving formatting: <data from untranslated.txt>`

4. Run the program with the following command:
```LocalizationTool.exe -original "en" -dest "rus" -sync "translated.txt"```

    Where: `en` - path to original language folder, `rus` - path to target language folder, `translated.txt` - path to file with translated data.

   this command will add the translation to the target translation folder with original key order

## Additional information
You Ñan  use the `check.cmd` and `sync.cmd` to run the program with the necessary parameters.

Please note that the folder name, for example `en`, is also the file extension (`*.en`). Other files will be ignored.
