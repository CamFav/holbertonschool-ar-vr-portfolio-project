using UnityEngine;

public static class PlayerPrefsArrayUtils
{
    /// <summary>
    /// Sauvegarde un tableau de chaînes dans PlayerPrefs.
    /// </summary>
    public static void SetStringArray(string key, string[] array)
    {
        PlayerPrefs.SetInt(key + "_Length", array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            PlayerPrefs.SetString(key + "_" + i, array[i]);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Récupère un tableau de chaînes depuis PlayerPrefs.
    /// </summary>
    public static string[] GetStringArray(string key)
    {
        int length = PlayerPrefs.GetInt(key + "_Length", 0);
        string[] array = new string[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = PlayerPrefs.GetString(key + "_" + i, string.Empty);
        }
        return array;
    }
}
