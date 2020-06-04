using UnityEngine;

public class MenuValues
{
    public static float position = 7f;
    public static bool moveRight = false;
    public static bool muteMusic = GetMuseValue();

    private static bool GetMuseValue(){
        if (PlayerPrefs.GetInt("Mute") == 0)
            return false;
        else
            return true;
    }
}
