//#define LOCALIZATION_DEBUG

using System.Collections.Generic;

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

public static class Localization
{
    private const string NO_TEXT_FOUND_PLACEHOLDER =
#if UNITY_EDITOR
        "NO_TRANSLATION";
#else
        "";
#endif

    private static Dictionary<string, string> _translationsDe;
    private static Dictionary<string, string> _translationsEn;

    public static bool IsGerman { get; set; } = true;

    public static string GetText(string key)
    {
        if (_translationsDe == null || _translationsEn == null)
        {
            InitTranslations();
        }

        var translations = IsGerman ? _translationsDe : _translationsEn;

#if LOCALIZATION_DEBUG
        // ReSharper disable once PossibleNullReferenceException
        return translations.TryGetValue(key, out var result) ? "-" : NO_TEXT_FOUND_PLACEHOLDER;
#else

        // ReSharper disable once PossibleNullReferenceException
        return translations.TryGetValue(key, out var result) ? result : NO_TEXT_FOUND_PLACEHOLDER;
#endif
    }


    private static void InitTranslations()
    {
        _translationsDe = new Dictionary<string, string>();
        _translationsEn = new Dictionary<string, string>();

        // paste code here:
        _translationsDe.Add("unity_scanning_title", "Suche eine ebene Standfläche");
        _translationsEn.Add("unity_scanning_title", "Find a flat place to stand");

        _translationsDe.Add("unity_scanning_tip_0", "Tipp 1: Richte dein Mobiltelefon auf einen flachen Untergrund.");
        _translationsEn.Add("unity_scanning_tip_0", "Tip #1 Point your mobile phone at a flat surface");

        _translationsDe.Add("unity_scanning_tip_1", "Tipp 2: Bewege dein Mobiltelefon langsam hin und her.");
        _translationsEn.Add("unity_scanning_tip_1", "Tip #2 Move your phone slowly from side to side.");

        _translationsDe.Add("unity_scanning_tip_2", "Tipp 3: Schaue, dass es genug Licht hat.");
        _translationsEn.Add("unity_scanning_tip_2", "Tip #3 Make sure there is enough light.");

        _translationsDe.Add("unity_load_panorama_image", "Lade Panoramabild ...");
        _translationsEn.Add("unity_load_panorama_image", "Loading panoramic image ...");

        _translationsDe.Add("unity_error_load_panorama_image",
            "Oops, es gab einen Fehler beim Laden des Bildes. Bitte versuche es erneut.");
        _translationsEn.Add("unity_error_load_panorama_image",
            "Oops, there was an error loading the image. Please try again.");

        _translationsDe.Add("unity_loom_living", "Wohnen");
        _translationsEn.Add("unity_loom_living", "Living");

        _translationsDe.Add("unity_loom_clock", "Arbeitszeit");
        _translationsEn.Add("unity_loom_clock", "Working hours");

        _translationsDe.Add("unity_loom_rapport", "Frauen in der Fabrik");
        _translationsEn.Add("unity_loom_rapport", "Women in the factory");

        _translationsDe.Add("unity_loom_loom", "Webstuhl");
        _translationsEn.Add("unity_loom_loom", "Loom");

        _translationsDe.Add("unity_flood_bridge_wooden_bridge", "Holzbrücke");
        _translationsEn.Add("unity_flood_bridge_wooden_bridge", "Wooden bridge");

        _translationsDe.Add("unity_flood_railroad_bridge", "Eisenbahnbrücke");
        _translationsEn.Add("unity_flood_railroad_bridge", "Railway bridge");

        _translationsDe.Add("unity_flood_final_extent", "Finale Ausdehnung");
        _translationsEn.Add("unity_flood_final_extent", "Maximum extension");

        _translationsDe.Add("unity_ar_initialization_failed",
            "Oops! Es ist etwas schief gelaufen. Starte das Erlebnis erneut und überprüfe, ob dein Gerät Augmented Reality-tauglich ist.");
        _translationsEn.Add("unity_ar_initialization_failed",
            "Oops! Something went wrong. Restart the experience and check if your device supports Augmented Reality.");

        _translationsDe.Add("unity_recommend_title", "Du brauchst die LIstory App!");
        _translationsEn.Add("unity_recommend_title", "You need the LIstory app!");

        _translationsDe.Add("unity_recommend_text",
            "Hast du Deine Wanderschuhe schon an? Lade dir jetzt den einzigartigen Begleiter für den Liechtenstein Weg herunter.");
        _translationsEn.Add("unity_recommend_text",
            "Are you ready to go on a hike? Download the unique guide to the Liechtenstein Trail.");

        _translationsDe.Add("unity_webstuhl_begruessung_0", "Ich bin die Weberin Maria Banzer aus Triesen.");
        _translationsEn.Add("unity_webstuhl_begruessung_0", "I am Maria Banzer, a weaver from Triesen.");

        _translationsDe.Add("unity_webstuhl_begruessung_1",
            "Vor mir steht die Webmaschine, mein wichtigstes Arbeitsgerät.");
        _translationsEn.Add("unity_webstuhl_begruessung_1",
            "In front of me is a weaving loom, my most important tool.");

        _translationsDe.Add("unity_webstuhl_begruessung_2",
            "Ich erzähle dir gerne ein bisschen von meinem Arbeitsalltag ...");
        _translationsEn.Add("unity_webstuhl_begruessung_2",
            "I would like to tell you a little bit about my daily work ...");

        _translationsDe.Add("unity_webstuhl_begruessung_3", "hier in der Baumwollspinnerei.");
        _translationsEn.Add("unity_webstuhl_begruessung_3", "here in the cotton-weaving mill.");

        _translationsDe.Add("unity_webstuhl_begruessung_4",
            "Klicke auf einen Gegenstand, um mehr darüber zu erfahren.");
        _translationsEn.Add("unity_webstuhl_begruessung_4", "Click on an item to learn more about it.");

        _translationsDe.Add("unity_webstuhl_frauen_0",
            "Mit mir arbeiten vor allem junge, ledige Frauen in der Fabrik.");
        _translationsEn.Add("unity_webstuhl_frauen_0",
            "Most of the people who work with me are young, unmarried women.");

        _translationsDe.Add("unity_webstuhl_frauen_1", "Unsere Arbeit geniesst leider nur geringes Ansehen,");
        _translationsEn.Add("unity_webstuhl_frauen_1", "Sadly our work is not considered prestigious,");

        _translationsDe.Add("unity_webstuhl_frauen_2",
            "aber unser bescheidene Verdienst ist wichtig für unsere Familien.");
        _translationsEn.Add("unity_webstuhl_frauen_2", "but the modest money we earn is important for our families.");

        _translationsDe.Add("unity_webstuhl_frauen_3", "Hier in Triesen sind wir Frauen als Weberinnen,");
        _translationsEn.Add("unity_webstuhl_frauen_3", "Here in Triesen we women work as weavers,");

        _translationsDe.Add("unity_webstuhl_frauen_4", "Spulerinnen, Tuchschauerinnen und Hilfskräfte beschäftigt.");
        _translationsEn.Add("unity_webstuhl_frauen_4", "bobbin winders, cloth inspectors and general helpers.");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_0",
            "Wir arbeiten hier in der Fabrik in drei Schichten à 9 Stunden pro Tag.");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_0",
            "In the factory we work in shifts. There are three shifts per day, each lasting nine hours.");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_1", "Gesetzlich gilt die 48-Stunden-Woche.");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_1",
            "By law we are not allowed to work more than 48 hours a week.");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_2", "Früher hatten die Arbeiterinnen und Arbeiter ...");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_2", "In the past, people worked ...");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_3", "noch wesentlich längere Arbeitszeiten als wir.");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_3", "much longer hours than we do now.");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_4", "So betrug in den Anfängen der Baumwollweberei ...");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_4", "In the early days of cotton weaving,");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_5", "um 1863 die tägliche Arbeitszeit noch 13 Stunden.");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_5", "around 1863, people still worked 13 hours a day.");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_6", "Ab 1908 galt dann die 60-Stunden-Woche");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_6", "From 1908 the working week was limited to 60 hours");

        _translationsDe.Add("unity_webstuhl_arbeitszeit_7", "10 Stunden pro Tag von Montag bis Samstag.");
        _translationsEn.Add("unity_webstuhl_arbeitszeit_7", "10 hours per day from Monday to Saturday.");

        _translationsDe.Add("unity_webstuhl_clock", "Uhr");
        _translationsEn.Add("unity_webstuhl_clock", "Clock");

        _translationsDe.Add("unity_webstuhl_loom", "Webmaschine");
        _translationsEn.Add("unity_webstuhl_loom", "Weaving loom");

        _translationsDe.Add("unity_webstuhl_paper", "Rapport");
        _translationsEn.Add("unity_webstuhl_paper", "Report");

        _translationsDe.Add("unity_webstuhl_keys", "Schlüssel");
        _translationsEn.Add("unity_webstuhl_keys", "Keys");

        _translationsDe.Add("unity_webstuhl_webstuhl_0", "Mein Arbeitsgerät ist die Webmaschine.");
        _translationsEn.Add("unity_webstuhl_webstuhl_0", "My working tool is the weaving loom.");

        _translationsDe.Add("unity_webstuhl_webstuhl_1", "Diese Webmaschine hier wurde im Jahr 1957 hergestellt.");
        _translationsEn.Add("unity_webstuhl_webstuhl_1", "This weaving loom was made in 1957.");

        _translationsDe.Add("unity_webstuhl_webstuhl_2", "Der Anschaffungspreis betrug 24‘000 Schweizer Franken.");
        _translationsEn.Add("unity_webstuhl_webstuhl_2", "It cost 24,000 Swiss francs.");

        _translationsDe.Add("unity_webstuhl_webstuhl_3", "In Triesen betreiben wir 240 solcher Webstühle.");
        _translationsEn.Add("unity_webstuhl_webstuhl_3", "In Triesen we operate 240 such looms.");

        _translationsDe.Add("unity_webstuhl_webstuhl_4",
            "Bei diesem Modell hier handelt es sich mit Ausnahme der Elektromotoren,");
        _translationsEn.Add("unity_webstuhl_webstuhl_4", "With the exception of the electric motors,");

        _translationsDe.Add("unity_webstuhl_webstuhl_5", "um einen rein mechanischen Webstuhl.");
        _translationsEn.Add("unity_webstuhl_webstuhl_5", "this model is a purely mechanical loom.");

        _translationsDe.Add("unity_webstuhl_webstuhl_6", "Das Schiffchen wird 160 Mal pro Minute ...");
        _translationsEn.Add("unity_webstuhl_webstuhl_6", "The shuttle is shot 160 times per minute ...");

        _translationsDe.Add("unity_webstuhl_webstuhl_7",
            "von einer Seite auf die andere Seite des Webstuhls geschossen.");
        _translationsEn.Add("unity_webstuhl_webstuhl_7", "from one side of the loom to the other.");

        _translationsDe.Add("unity_webstuhl_webstuhl_8", "Dies ergibt je nach Webdichte ...");
        _translationsEn.Add("unity_webstuhl_webstuhl_8", "This produces around four metres of fabric per hour,");

        _translationsDe.Add("unity_webstuhl_webstuhl_9", "eine Stofflänge von 4 Metern pro Stunde.");
        _translationsEn.Add("unity_webstuhl_webstuhl_9", "depending on how tight the weave is.");

        _translationsDe.Add("unity_webstuhl_webstuhl_10", "Die Maschine war sehr laut, überzeuge dich selbst.");
        _translationsEn.Add("unity_webstuhl_webstuhl_10", "The machine was very loud – listen for yourself.");

        _translationsDe.Add("unity_webstuhl_wohnen_0", "Ich wohne in meinem Elternhaus hier in Triesen.");
        _translationsEn.Add("unity_webstuhl_wohnen_0", "I live in the house where I grew up here in Triesen.");

        _translationsDe.Add("unity_webstuhl_wohnen_1", "Für die Familien unserer auswärtigen Facharbeiter ...");
        _translationsEn.Add("unity_webstuhl_wohnen_1", "The families of foreign workers live in accommodation");

        _translationsDe.Add("unity_webstuhl_wohnen_2", "wurden bereits 1873 Arbeiterwohnungen ...");
        _translationsEn.Add("unity_webstuhl_wohnen_2", "provided for them by the factory.");

        _translationsDe.Add("unity_webstuhl_wohnen_3", "wie das Kosthaus oder 1942 das Doppelhaus ...");
        _translationsEn.Add("unity_webstuhl_wohnen_3",
            "An early example is the Kosthaus, built in 1873, or the semi-detached house ...");

        _translationsDe.Add("unity_webstuhl_wohnen_4", "oberhalb der Fabrik gebaut oder von der Fabrik gekauft.");
        _translationsEn.Add("unity_webstuhl_wohnen_4",
            "above the factory, which was purchased in 1942 and used as accommodation for workers.");

        _translationsDe.Add("unity_webstuhl_wohnen_5", "Jetzt wohnen aber auch einheimische ...");
        _translationsEn.Add("unity_webstuhl_wohnen_5", "Today these dwellings are also used by local families ...");

        _translationsDe.Add("unity_webstuhl_wohnen_6", "Fabrikarbeiterfamilien in diesen Arbeiterhäusern.");
        _translationsEn.Add("unity_webstuhl_wohnen_6",
            "who have been employed in the factory for several generations.");

        _translationsDe.Add("unity_webstuhl_wohnen_7",
            "Meist ist der Vater der Familie in unserer Fabrik beschäftigt.");
        _translationsEn.Add("unity_webstuhl_wohnen_7",
            "Usually it is the father of the family who works in our factory.");

        _translationsDe.Add("unity_webstuhl_wohnen_8", "Unser Obermeister (Betriebsleiter) ...");
        _translationsEn.Add("unity_webstuhl_wohnen_8", "Our chief foreman lives with his family ...");

        _translationsDe.Add("unity_webstuhl_wohnen_9",
            "wohnt mit seiner Familie direkt gegenüber der Fabrik an der Dorfstrasse ...");
        _translationsEn.Add("unity_webstuhl_wohnen_9", "opposite the factory on Dorfstrasse,");

        _translationsDe.Add("unity_webstuhl_wohnen_10", "und die Villa der Fabrikantenfamilie ...");
        _translationsEn.Add("unity_webstuhl_wohnen_10",
            "and the mansion belonging to the family which owns the factory ...");

        _translationsDe.Add("unity_webstuhl_wohnen_11", "steht auch gleich neben der Fabrik.");
        _translationsEn.Add("unity_webstuhl_wohnen_11", "is also located right next to the factory building.");

        _translationsDe.Add("unity_webstuhl_verabschiedung_0", "Es hat mich gefreut dir etwas von meinem Leben ...");
        _translationsEn.Add("unity_webstuhl_verabschiedung_0", "I enjoyed telling you a little bit about my life ...");

        _translationsDe.Add("unity_webstuhl_verabschiedung_1", "in der Baumwollweberei zu erzählen.");
        _translationsEn.Add("unity_webstuhl_verabschiedung_1", "at the cotton-weaving mill.");

        _translationsDe.Add("unity_webstuhl_verabschiedung_2",
            "Ich muss jetzt aber wieder weiter arbeiten. Auf Wiedersehen.");
        _translationsEn.Add("unity_webstuhl_verabschiedung_2", "I have to get back to work now. Goodbye.");

        _translationsDe.Add("unity_torfstecher_sod", "Torfsode");
        _translationsEn.Add("unity_torfstecher_sod", "Peat sod");

        _translationsDe.Add("unity_torfstecher_spade", "Torfspaten");
        _translationsEn.Add("unity_torfstecher_spade", "Peat spade");

        _translationsDe.Add("unity_torfstecher_apple_juice", "Most");
        _translationsEn.Add("unity_torfstecher_apple_juice", "Cider");

        _translationsDe.Add("unity_torfstecher_wheelbarrow", "Schubkarren");
        _translationsEn.Add("unity_torfstecher_wheelbarrow", "Wheelbarrow");

        _translationsDe.Add("unity_torfstecher_string", "Schnur");
        _translationsEn.Add("unity_torfstecher_string", "String");

        _translationsDe.Add("unity_torfstecher_vorstellung_0",
            "Hallo, ich bin Ernst, ein waschechter Torfstecher aus Ruggell.");
        _translationsEn.Add("unity_torfstecher_vorstellung_0",
            "Hello, I'm Ernst. I’m a real peat-cutter from Ruggell.");

        _translationsDe.Add("unity_torfstecher_vorstellung_1",
            "Ich zeige dir jetzt, was ich beim Torfstechen so alles gemacht habe ...");
        _translationsEn.Add("unity_torfstecher_vorstellung_1", "I'll show you how I cut the peat ...");

        _translationsDe.Add("unity_torfstecher_vorstellung_2", "und stelle dir ein paar Fragen.");
        _translationsEn.Add("unity_torfstecher_vorstellung_2", "and ask you a few questions.");

        _translationsDe.Add("unity_torfstecher_schnur_0",
            "Bevor ich den Torf stechen kann, muss ich die Oberfläche ...");
        _translationsEn.Add("unity_torfstecher_schnur_0",
            "Before I can cut the peat, first I must clear the surface ...");

        _translationsDe.Add("unity_torfstecher_schnur_1", "von Gras, Humus, Wurzeln und Moos befreien.");
        _translationsEn.Add("unity_torfstecher_schnur_1", "of grass, humus, roots and moss.");

        _translationsDe.Add("unity_torfstecher_schnur_2",
            "Dann steche ich entlang einer geraden Linie ein rund 2 m breites Feld aus.");
        _translationsEn.Add("unity_torfstecher_schnur_2",
            "Then I cut out an area about two metres wide along a straight line.");

        _translationsDe.Add("unity_torfstecher_schnur_3",
            "Welchen Gegenstand benötige ich, um die gerade Linie festzulegen?");
        _translationsEn.Add("unity_torfstecher_schnur_3", "Which item do I need to set the straight line?");

        _translationsDe.Add("unity_torfstecher_spaten_0",
            "Nachdem ich die Oberfläche mit der Schaufel freigemacht habe,");
        _translationsEn.Add("unity_torfstecher_spaten_0", "Once I have cleared the surface with a shovel,");

        _translationsDe.Add("unity_torfstecher_spaten_1", "beginne ich den Torf (\"Turba\") auszustechen.");
        _translationsEn.Add("unity_torfstecher_spaten_1", "I begin to cut the peat.");

        _translationsDe.Add("unity_torfstecher_spaten_2", "Ein Torfbalken ist rund 25 cm lang, 15 cm hoch und breit.");
        _translationsEn.Add("unity_torfstecher_spaten_2", "One piece is about 25 cm long and 15 cm high and wide.");

        _translationsDe.Add("unity_torfstecher_spaten_3", "Mit was für einem Gegenstand steche ich den Torf aus?");
        _translationsEn.Add("unity_torfstecher_spaten_3", "What kind of object do I use to cut the peat?");

        _translationsDe.Add("unity_torfstecher_karren_0",
            "Die ausgestochenen Torfbalken fahr ich dann zur Trocknungsstelle,");
        _translationsEn.Add("unity_torfstecher_karren_0",
            "When I have cut out the pieces of peat, I transport them to the drying area.");

        _translationsDe.Add("unity_torfstecher_karren_1",
            "wo ich den Torf in Doppelreihen zum Trocknen aufschichte (\"aufhüttle\").");
        _translationsEn.Add("unity_torfstecher_karren_1", "There I layer the peat in double rows and leave it to dry.");

        _translationsDe.Add("unity_torfstecher_karren_2", "Womit transportiere ich die Torfbalken?");
        _translationsEn.Add("unity_torfstecher_karren_2", "How do I transport the pieces of peat?");

        _translationsDe.Add("unity_torfstecher_most_0", "Die Arbeit ist anstrengend,");
        _translationsEn.Add("unity_torfstecher_most_0", "The work is exhausting,");

        _translationsDe.Add("unity_torfstecher_most_1", "jetzt brauche ich eine Pause und etwas Feines zu trinken.");
        _translationsEn.Add("unity_torfstecher_most_1", "now I need a break and something nice to drink.");

        _translationsDe.Add("unity_torfstecher_most_2",
            "Dann muss ich rund zwei Wochen warten bis der Torf getrocknet ist.");
        _translationsEn.Add("unity_torfstecher_most_2", "I will have to wait about two weeks for the peat to dry.");

        _translationsDe.Add("unity_torfstecher_most_3",
            "Nach dem Trocknen lade ich den Torf auf meinen Schubkarren ...");
        _translationsEn.Add("unity_torfstecher_most_3",
            "Once it has dried, I will load the peat onto my wheelbarrow ...");

        _translationsDe.Add("unity_torfstecher_most_4", "und transportiere den Torf zum Lagern ab.");
        _translationsEn.Add("unity_torfstecher_most_4", "and take it away to be stored.");

        _translationsDe.Add("unity_torfstecher_most_5", "Was trinke ich in der Pause am liebsten?");
        _translationsEn.Add("unity_torfstecher_most_5", "What do I like to drink most during my break?");

        _translationsDe.Add("unity_torfstecher_torfsode_0",
            "In der Torfhütte habe ich den Torf den ganzen Sommer über trocknen lassen.");
        _translationsEn.Add("unity_torfstecher_torfsode_0", "I leave the peat in the hut to dry over the summer.");

        _translationsDe.Add("unity_torfstecher_torfsode_1",
            "Bis im Herbst war der Torf dann nicht nur wesentlich leichter,");
        _translationsEn.Add("unity_torfstecher_torfsode_1",
            "By the time autumn comes the peat is not only much lighter ...");

        _translationsDe.Add("unity_torfstecher_torfsode_2", "sondern schrumpfte auch stark zusammen.");
        _translationsEn.Add("unity_torfstecher_torfsode_2", "but has also shrunk considerably.");

        _translationsDe.Add("unity_torfstecher_torfsode_3", "Wie sieht der getrocknete Torf aus?");
        _translationsEn.Add("unity_torfstecher_torfsode_3", "What does the dried peat look like?");

        _translationsDe.Add("unity_torfstecher_verabschiedung_0", "Vielen Dank für deinen Besuch,");
        _translationsEn.Add("unity_torfstecher_verabschiedung_0", "Thank you for your visit.");

        _translationsDe.Add("unity_torfstecher_verabschiedung_1", "du hast das super gemacht ...");
        _translationsEn.Add("unity_torfstecher_verabschiedung_1", "You've done a great job.");

        _translationsDe.Add("unity_torfstecher_verabschiedung_2", "und kannst jetzt gerade als Torfstecher beginnen.");
        _translationsEn.Add("unity_torfstecher_verabschiedung_2", "Now you can start working as a peat-cutter too!");

        _translationsDe.Add("unity_torfstecher_falsch", "Das ist falsch versuch es noch einmal.");
        _translationsEn.Add("unity_torfstecher_falsch", "Incorrect, please try again.");

        _translationsDe.Add("unity_torfstecher_richtig", "Gut gemacht.");
        _translationsEn.Add("unity_torfstecher_richtig", "Well done.");

        _translationsDe.Add("unity_gericht_dialog_0", "Wir finden uns hier vor der Kapelle Rofenberg ein,");
        _translationsEn.Add("unity_gericht_dialog_0", "We are gathered here in front of Rofenberg ...");

        _translationsDe.Add("unity_gericht_dialog_1", "um Recht zu sprechen im vorliegenden Fall ...");
        _translationsEn.Add("unity_gericht_dialog_1", "in order to rule in the present case ...");

        _translationsDe.Add("unity_gericht_dialog_2", "betreffend Hauskauf durch Bescha Hassler.");
        _translationsEn.Add("unity_gericht_dialog_2", "on the purchase of a house by Bascha Hasler.");

        _translationsDe.Add("unity_gericht_dialog_3", "Ich werde hier bei meinem Eid Recht sprechen,");
        _translationsEn.Add("unity_gericht_dialog_3", "On my oath I shall dispense justice ...");

        _translationsDe.Add("unity_gericht_dialog_4", "richten über Ehre und Gut,");
        _translationsEn.Add("unity_gericht_dialog_4", "and pass judgement on honour and property,");

        _translationsDe.Add("unity_gericht_dialog_5", "Geld und Wert und das auf gnädigem Geheiss und Befehl ...");
        _translationsEn.Add("unity_gericht_dialog_5", "money and value by the gracious command ...");

        _translationsDe.Add("unity_gericht_dialog_6", "des hochwohlgeborenen Herrn Ferdinand Karl von Hohenems,");
        _translationsEn.Add("unity_gericht_dialog_6", "of Ferdinand Karl of Hohenems,");

        _translationsDe.Add("unity_gericht_dialog_7", "Graf zu Vaduz und Herr zu Schellenberg");
        _translationsEn.Add("unity_gericht_dialog_7", "Count of Vaduz and Lord of Schellenberg,");

        _translationsDe.Add("unity_gericht_dialog_8", "als unserem gnädigen Herrn.");
        _translationsEn.Add("unity_gericht_dialog_8", "our gracious ruler.");

        _translationsDe.Add("unity_gericht_dialog_9", "Ich, Georg Lampert in meinem Namen ...");
        _translationsEn.Add("unity_gericht_dialog_9", "I, Georg Lampert, on behalf of myself ...");

        _translationsDe.Add("unity_gericht_dialog_10", "und im Namen der weiteren Erben von Stefan Halser ...");
        _translationsEn.Add("unity_gericht_dialog_10", "and on behalf of the other heirs of Stefan Hasler,");

        _translationsDe.Add("unity_gericht_dialog_11", "erhebe Anklage gegen Bascha Hasler.");
        _translationsEn.Add("unity_gericht_dialog_11", "do hereby accuse Bascha Hasler.");

        _translationsDe.Add("unity_gericht_dialog_12", "Bascha soll den Kaufzettel vom Haus vorlegen,");
        _translationsEn.Add("unity_gericht_dialog_12",
            "I demand that Bascha present written proof of purchase for the house ...");

        _translationsDe.Add("unity_gericht_dialog_13",
            "das er vom Feldkirch Stadtamman Anreas von Fröwis gekauft hat ...");
        _translationsEn.Add("unity_gericht_dialog_13",
            "he bought from the Feldkirch town administrator, Andreas von Fröwis.");

        _translationsDe.Add("unity_gericht_dialog_14", "oder andernfalls ...");
        _translationsEn.Add("unity_gericht_dialog_14", "Should he fail to present such proof,");

        _translationsDe.Add("unity_gericht_dialog_15", "soll er den Schätzwert bezahlen!");
        _translationsEn.Add("unity_gericht_dialog_15", "so must he pay the estimated value of the property.");

        _translationsDe.Add("unity_gericht_dialog_16", "Hier, ich weise euch den Kaufzettel vor.");
        _translationsEn.Add("unity_gericht_dialog_16", "Here is your proof of purchase.");

        _translationsDe.Add("unity_gericht_dialog_17", "Denselben habe ich aber beim Kauf ...");
        _translationsEn.Add("unity_gericht_dialog_17", "But let it be known that at the time of purchase ...");

        _translationsDe.Add("unity_gericht_dialog_18", "gleich wiederrufen!");
        _translationsEn.Add("unity_gericht_dialog_18", "I did immediately withdraw from the transaction.");

        _translationsDe.Add("unity_gericht_dialog_19", "Ich hoffe inständig,");
        _translationsEn.Add("unity_gericht_dialog_19", "I sincerely hope ...");

        _translationsDe.Add("unity_gericht_dialog_20", "man werde mich nicht anhalten,");
        _translationsEn.Add("unity_gericht_dialog_20", "I will not be forced ...");

        _translationsDe.Add("unity_gericht_dialog_21", "den Kauf tatsächlich tätigen zu müssen!");
        _translationsEn.Add("unity_gericht_dialog_21", "to go through with the deal.");

        _translationsDe.Add("unity_gericht_dialog_22", "Ich muss dies verneinen.");
        _translationsEn.Add("unity_gericht_dialog_22", "With this I cannot agree.");

        _translationsDe.Add("unity_gericht_dialog_23", "Bascha Hasler muss den Kauf einhalten.");
        _translationsEn.Add("unity_gericht_dialog_23", "Bascha Hasler must abide by his purchase.");

        _translationsDe.Add("unity_gericht_dialog_24", "Ich verlange dazu ein Urteil!");
        _translationsEn.Add("unity_gericht_dialog_24", "I demand a verdict!");

        _translationsDe.Add("unity_gericht_dialog_25", "Mir ist der Kauf aber gekündigt worden ...");
        _translationsEn.Add("unity_gericht_dialog_25",
            "But it was confirmed to me that the purchase would be cancelled ...");

        _translationsDe.Add("unity_gericht_dialog_26", "das will ich beweisen!");
        _translationsEn.Add("unity_gericht_dialog_26", "that I shall prove!");

        _translationsDe.Add("unity_gericht_dialog_27", "Ich habe darum ein anderes Haus gekauft,");
        _translationsEn.Add("unity_gericht_dialog_27", "For this reason I did buy another house.");

        _translationsDe.Add("unity_gericht_dialog_28", "und bin nicht schuldig ...");
        _translationsEn.Add("unity_gericht_dialog_28", "I owe nothing ...");

        _translationsDe.Add("unity_gericht_dialog_29", "und somit nicht verpflichtet,");
        _translationsEn.Add("unity_gericht_dialog_29", "and am therefore not obliged ...");

        _translationsDe.Add("unity_gericht_dialog_30", "diesen Kauf einzuhalten.");
        _translationsEn.Add("unity_gericht_dialog_30", "to go through with the purchase.");

        _translationsDe.Add("unity_gericht_dialog_31", "Ich setze das zu Recht.");
        _translationsEn.Add("unity_gericht_dialog_31", "That is my position under law.");

        _translationsDe.Add("unity_gericht_dialog_32", "Ich will auch beweisen,");
        _translationsEn.Add("unity_gericht_dialog_32", "I shall also prove something,");

        _translationsDe.Add("unity_gericht_dialog_33", "dass der Herr Landvogt dem Bascha Hasler ...");
        _translationsEn.Add("unity_gericht_dialog_33", "namely that the governor granted this house ...");

        _translationsDe.Add("unity_gericht_dialog_34", "das Haus zugesprochen hat,");
        _translationsEn.Add("unity_gericht_dialog_34", "to Bascha Hasler before he,");

        _translationsDe.Add("unity_gericht_dialog_35", "und zwar ehe er, Bascha, das andere Haus gekauft hat.");
        _translationsEn.Add("unity_gericht_dialog_35", "Bascha, purchased the other house.");

        _translationsDe.Add("unity_gericht_dialog_36", "Darüber hat das löbliche Oberamt befohlen,");
        _translationsEn.Add("unity_gericht_dialog_36", "The esteemed ruling authority, the Oberamt,");

        _translationsDe.Add("unity_gericht_dialog_37", "dass er den Kauf also ausführen muss.");
        _translationsEn.Add("unity_gericht_dialog_37",
            "has therefore ordered that he must go through with the purchase.");

        _translationsDe.Add("unity_gericht_dialog_38", "Es besteht jedoch die Möglichkeit,");
        _translationsEn.Add("unity_gericht_dialog_38", "However, it is still possible ...");

        _translationsDe.Add("unity_gericht_dialog_39", "dass Bascha Hasler den Herrn Fröwis bittet,");
        _translationsEn.Add("unity_gericht_dialog_39", "possible that Bascha Hasler may ask Mr Fröwis");

        _translationsDe.Add("unity_gericht_dialog_40", "auf den Kauf zu verzichten.");
        _translationsEn.Add("unity_gericht_dialog_40", "to release him from the purchase.");

        _translationsDe.Add("unity_gericht_dialog_41", "Meines Wissens wollte der Hasler den Kauf schon einhalten!");
        _translationsEn.Add("unity_gericht_dialog_41",
            "To my knowledge Mr Hasler did indeed want to abide by the purchase,");

        _translationsDe.Add("unity_gericht_dialog_42", "Aber nicht gemäss der  ...");
        _translationsEn.Add("unity_gericht_dialog_42", "but not at the price attributed ...");

        _translationsDe.Add("unity_gericht_dialog_43", "erst der nachträglich vorgenommen Schatzung ...");
        _translationsEn.Add("unity_gericht_dialog_43", "to the property ...");

        _translationsDe.Add("unity_gericht_dialog_44", "des Hauses.");
        _translationsEn.Add("unity_gericht_dialog_44", "in a subsequent valuation.");

        _translationsDe.Add("unity_gericht_dialog_45", "Der Bascha hat dann dem Herrn Fröwis den Kauf aufgekündigt.");
        _translationsEn.Add("unity_gericht_dialog_45",
            "It was then that Bascha cancelled the purchase from Mr Fröwis.");

        _translationsDe.Add("unity_gericht_dialog_46", "Also ich fasse zusammen:");
        _translationsEn.Add("unity_gericht_dialog_46", "I shall sum up:");

        _translationsDe.Add("unity_gericht_dialog_47", "Im Streitfall ...");
        _translationsEn.Add("unity_gericht_dialog_47", "In the dispute ...");

        _translationsDe.Add("unity_gericht_dialog_48",
            "zwischen Stefan Haslers seligem Erben als Kläger einerseits  ...");
        _translationsEn.Add("unity_gericht_dialog_48",
            "between Stefan Hasler's blessed heirs as accusers on the one hand ..");

        _translationsDe.Add("unity_gericht_dialog_49", "und Bascha Halser als Beklagten andererseits ...");
        _translationsEn.Add("unity_gericht_dialog_49", "and Bascha Halser as the accused on the other hand,");

        _translationsDe.Add("unity_gericht_dialog_50", "betreffend den Hauskauf wird nach Klage,");
        _translationsEn.Add("unity_gericht_dialog_50", "concerning the purchase of the house,");

        _translationsDe.Add("unity_gericht_dialog_51", "Rede und Widerrede ...");
        _translationsEn.Add("unity_gericht_dialog_51",
            "I hereby – after hearing the accusation, listening to both sides ...");

        _translationsDe.Add("unity_gericht_dialog_52", "und nach Einvernahme der Zeugen");
        _translationsEn.Add("unity_gericht_dialog_52", "of the argument and examining the witnesses ...");

        _translationsDe.Add("unity_gericht_dialog_53", "hiermit zu Recht erkannt:");
        _translationsEn.Add("unity_gericht_dialog_53", "do solemnly pronounce the following judgement:");

        _translationsDe.Add("unity_gericht_dialog_54", "Der Bascha Hasler muss das Haus behalten.");
        _translationsEn.Add("unity_gericht_dialog_54", "Bascha Hasler must keep the house.");

        _translationsDe.Add("unity_gericht_dialog_55", "Er muss dafür nicht den Schätzungswert bezahlen,");
        _translationsEn.Add("unity_gericht_dialog_55", "For this property he shall pay not the estimated value ...");

        _translationsDe.Add("unity_gericht_dialog_56", "sondern den im Kaufzettel festgelegten Preis von 325 Gulden.");
        _translationsEn.Add("unity_gericht_dialog_56",
            "but instead the price of 325 guilders agreed on the purchase slip.");

        _translationsDe.Add("unity_gericht_dialog_57",
            "Er hat mit dieser Summe zunächst die Ansprüche des Herrn Fröwis zu befriedigen ...");
        _translationsEn.Add("unity_gericht_dialog_57",
            "With this sum he must first satisfy the claims of Mr Fröwis ...");

        _translationsDe.Add("unity_gericht_dialog_58", "und dann die Gerichtsgebühren zu bezahlen.");
        _translationsEn.Add("unity_gericht_dialog_58", "and then pay the court fees.");

        _translationsDe.Add("unity_gericht_dialog_59",
            "Den verbleibenden Rest muss er den Klägern bezahlen und abstatten.");
        _translationsEn.Add("unity_gericht_dialog_59", "The rest he must pay to the accusers.");

        _translationsDe.Add("unity_gericht_dialog_60", "Alle anderen Kosten beider Seiten heben sich gegenseitig auf.");
        _translationsEn.Add("unity_gericht_dialog_60", "All other costs of both sides cancel each other out.");

        _translationsDe.Add("unity_gericht_dialog_61", "Ich appelliere,");
        _translationsEn.Add("unity_gericht_dialog_61", "I wish to lodge an appeal against this judgement ...");

        _translationsDe.Add("unity_gericht_dialog_62", "dass dieses Urteil vom nächst höheren Gericht und Richter,");
        _translationsEn.Add("unity_gericht_dialog_62", "so that the case may be heard by the court and judge ...");

        _translationsDe.Add("unity_gericht_dialog_63", "der nächsten Instanz, beurteilt wird.");
        _translationsEn.Add("unity_gericht_dialog_63", "of the next-highest instance.");

        _translationsDe.Add("unity_gericht_dialog_64", "Das Gericht akzeptiert die Appellation.");
        _translationsEn.Add("unity_gericht_dialog_64", "The court accepts the appeal.");

        _translationsDe.Add("unity_gericht_dialog_65", "Aber nur unter Voraussetzung,");
        _translationsEn.Add("unity_gericht_dialog_65", "But only under the condition ...");

        _translationsDe.Add("unity_gericht_dialog_66", "dass Bascha Hasler nach altem Herkommen und altem Brauch ...");
        _translationsEn.Add("unity_gericht_dialog_66", "that Bascha Hasler, according to old tradition,");

        _translationsDe.Add("unity_gericht_dialog_67",
            "noch vor Sonnenuntergang Silber und Gold als Kaution hinterlegt.");
        _translationsEn.Add("unity_gericht_dialog_67", "deposits silver and gold as a bond before sunset.");

        _translationsDe.Add("unity_gericht_dialog_68", "Wenn er das nicht tut, bleibt es beim ersten Urteil.");
        _translationsEn.Add("unity_gericht_dialog_68", "Should he fail to do so, the initial sentence shall apply.");

        _translationsDe.Add("unity_gericht_dialog_69", "Hiermit schliesse ich dieses Gericht für heute.");
        _translationsEn.Add("unity_gericht_dialog_69", "I hereby close court proceedings for the day.");

        _translationsDe.Add("unity_gericht_dialog_70",
            "Haltet euch an euer Wort, oder es kommt euch wortwörtlich teuer zu stehen.");
        _translationsEn.Add("unity_gericht_dialog_70", "Keep your word, or it will cost you dearly.");

        _translationsDe.Add("unity_gericht_angeklagter", "Angeklagter");
        _translationsEn.Add("unity_gericht_angeklagter", "Accused");

        _translationsDe.Add("unity_gericht_klaeger", "Kläger");
        _translationsEn.Add("unity_gericht_klaeger", "Plaintiff");

        _translationsDe.Add("unity_gericht_geschworener", "Geschworener");
        _translationsEn.Add("unity_gericht_geschworener", "Juror");

        _translationsDe.Add("unity_gericht_richter", "Richter");
        _translationsEn.Add("unity_gericht_richter", "Judge");

        _translationsDe.Add("unity_vaduz_library", "Bibliothek");
        _translationsEn.Add("unity_vaduz_library", "Library");

        _translationsDe.Add("unity_vaduz_bridge", "Holzbrücke");
        _translationsEn.Add("unity_vaduz_bridge", "Wooden bridge");

        _translationsDe.Add("unity_vaduz_courtyard", "Innenhof");
        _translationsEn.Add("unity_vaduz_courtyard", "Courtyard");

        _translationsDe.Add("unity_vaduz_chapel", "Kapelle");
        _translationsEn.Add("unity_vaduz_chapel", "Chapel");

        _translationsDe.Add("unity_vaduz_dining_room", "Speisesaal");
        _translationsEn.Add("unity_vaduz_dining_room", "Dining room");

        _translationsDe.Add("unity_vaduz_south_rondel", "Südrondell");
        _translationsEn.Add("unity_vaduz_south_rondel", "South rondel");

        _translationsDe.Add("unity_vaduz_vestibule", "Vestibühl");
        _translationsEn.Add("unity_vaduz_vestibule", "Vestibule");

        _translationsDe.Add("unity_vaduz_corridor", "Vorraum zum Speisesaal");
        _translationsEn.Add("unity_vaduz_corridor", "Corridor next to the dining room");

        _translationsDe.Add("unity_gutenberg_courtyard", "Innenhof");
        _translationsEn.Add("unity_gutenberg_courtyard", "Courtyard");

        _translationsDe.Add("unity_gutenberg_hall", "Palas");
        _translationsEn.Add("unity_gutenberg_hall", "Hall");

        _translationsDe.Add("unity_gutenberg_crest_room", "Wappenzimmer");
        _translationsEn.Add("unity_gutenberg_crest_room", "Crest room");

        _translationsDe.Add("unity_gutenberg_tile_stove", "Kachelofen");
        _translationsEn.Add("unity_gutenberg_tile_stove", "Tiled stove");

        _translationsDe.Add("unity_gutenberg_balcony", "Rundgang");
        _translationsEn.Add("unity_gutenberg_balcony", "Balcony");

        _translationsDe.Add("unity_gutenberg_bath", "Bad");
        _translationsEn.Add("unity_gutenberg_bath", "Bath");

        _translationsDe.Add("unity_gutenberg_kitchen", "Küche");
        _translationsEn.Add("unity_gutenberg_kitchen", "Kitchen");

        _translationsDe.Add("unity_gutenberg_bedroom", "Schlafzimmer");
        _translationsEn.Add("unity_gutenberg_bedroom", "Bedroom");

        _translationsDe.Add("unity_kastell_church", "Kirche");
        _translationsEn.Add("unity_kastell_church", "Church");

        _translationsDe.Add("unity_church_5jh", "Profaner Gebäudekomplex");
        _translationsEn.Add("unity_church_5jh", "Profane building complex");

        _translationsDe.Add("unity_church_8jh", "Erster möglicher Kirchenbau");
        _translationsEn.Add("unity_church_8jh", "Possibly first church building");

        _translationsDe.Add("unity_church_now", "Kirche");
        _translationsEn.Add("unity_church_now", "Church");

        _translationsDe.Add("unity_church_5jh_epoch", "5. - 7. Jahrhundert");
        _translationsEn.Add("unity_church_5jh_epoch", "5th - 7th century");

        _translationsDe.Add("unity_church_8jh_epoch", "8. - 9. Jahrhundet");
        _translationsEn.Add("unity_church_8jh_epoch", "8th - 9th century");

        _translationsDe.Add("unity_church_now_epoch", "heute");
        _translationsEn.Add("unity_church_now_epoch", "Today");

        _translationsDe.Add("unity_kastell_excavation", "Ausgrabung");
        _translationsEn.Add("unity_kastell_excavation", "Excavation");

        _translationsDe.Add("unity_kastell_baptistery", "Taufbecken");
        _translationsEn.Add("unity_kastell_baptistery", "Baptistry");

        _translationsDe.Add("unity_kastell_beneath", "Unter der Kirche");
        _translationsEn.Add("unity_kastell_beneath", "Beneath the church");

        _translationsDe.Add("unity_ueberflutung_dammbruch_title", "Dammbruch");
        _translationsEn.Add("unity_ueberflutung_dammbruch_title", "Breach in the dam");

        _translationsDe.Add("unity_ueberflutung_dammbruch_subtitle",
            "Eisenbahnbrücke bei Schaan\n25. September 1927 ca. 19 Uhr");
        _translationsEn.Add("unity_ueberflutung_dammbruch_subtitle",
            "Railroad bridge near Schaan\n25 September 1927, 7pm");

        _translationsDe.Add("unity_ueberflutung_meineposition_title", "Deine heutige Position");
        _translationsEn.Add("unity_ueberflutung_meineposition_title", "Your current position");

        _translationsDe.Add("unity_ueberflutung_meineposition_subtitle", "Wasserstand damals: 4.29m");
        _translationsEn.Add("unity_ueberflutung_meineposition_subtitle", "Water level at the time:  4.29m");

        // stop here:
    }
}