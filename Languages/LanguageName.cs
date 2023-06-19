namespace GalacticLib.Languages;

/// <summary>
/// <list type="bullet">
/// <item> System • Current system language (<see cref="English"/> if system language not supported) </item>
/// <item> (Google Translate API supported languages) </item>
/// <item> Other • Any language not listed </item>
/// </list>
/// </summary>
public enum LanguageName {
    Other = -1, //? Will default to English
    System = 0,

    Afrikaans, Albanian, Amharic, Arabic, Armenian, Azerbaijani,

    Basque, Belarusian, Bengali, Bosnian, Bulgarian,

    Catalan, Cebuano, Chichewa, ChineseSimplified, ChineseTraditional,
    Corsican, Croatian, Czech,

    Danish, Dutch,

    English, Esperanto, Estonian,

    Filipino, Finnish, French, Frisian,

    Gujarati, Galician, Georgian, German, Greek,

    HaitianCreole, Hausa, Hawaiian, Hindi, Hmong, Hungarian,

    Icelandic, Igbo, Indonesian, Irish, Italian,

    Japanese, Javanese,

    Kannada, Kazakh, Khmer, Korean, KurdishKurmanji, Kyrgyz,

    Latin, Luxembourgish, Lao, Lithuanian, Latvian,

    Macedonian, Malagasy, Malay, Malayalam, Maltese, Maori,
    Marathi, Mongolian, MyanmarBurmese,

    Nepali, Norwegian,

    Pashto, Persian, Polish, Portuguese, Punjabi,

    Romanian, Russian,

    Samoan, ScotsGaelic, Serbian, Sesotho, Shona, Sindhi, Sinhala, Slovak,
    Slovenian, Somali, Spanish, Sundanese, Swahili, Swedish,

    Tajik, Tamil, Telugu, Thai, Turkish,

    Ukrainian, Urdu, Uzbek,

    Vietnamese,

    Welsh,

    Xhosa,

    Yiddish, Yoruba,

    Zulu,

}
