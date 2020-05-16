public class Settings {

    const string LAST_LEVEL = "lastLevel";
    const string DIFFICULTY = "difficulty";
    
    # if UNITY_WEBGL
        static IPrefs _prefs = new CSVPrefs();
    # else
        static IPrefs _prefs = new UnityPrefs();
    #endif
    
    public static string LastLevel {
        get {
            return _prefs.GetString(LAST_LEVEL, "");
        }

        set {
            _prefs.SetString(LAST_LEVEL, value);
            _prefs.Save();
        }
    }
}
