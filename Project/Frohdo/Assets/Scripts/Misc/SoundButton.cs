using UnityEngine;
using System.Collections;

public class SoundButton
{
    private SoundButton() { }

    public static bool newSoundButton(GUIContent ButtonText, string style)
    {
        if (GUILayout.Button(ButtonText, style))
        {
            SoundController.Instance.playClickSound();
            return true;
        }
        return false;
    }

    public static bool newSoundButton(string ButtonText, string style)
    {
        if (GUILayout.Button(ButtonText, style))
        {
            SoundController.Instance.playClickSound();
            return true;
        }
        return false;
    }

    public static bool newSoundButton(GUIContent ButtonText, GUIStyle style)
    {
        if (GUILayout.Button(ButtonText, style))
        {
            SoundController.Instance.playClickSound();
            return true;
        }
        return false;
    }

    public static bool newSoundButton(string ButtonText, GUIStyle style)
    {
        if (GUILayout.Button(ButtonText, style))
        {
            SoundController.Instance.playClickSound();
            return true;
        }
        return false;
    }

    public static bool newSoundButton(GUIContent ButtonText)
    {
        if (GUILayout.Button(ButtonText))
        {
            SoundController.Instance.playClickSound();
            return true;
        }
        return false;
    }

    public static bool newSoundButton(string ButtonText)
    {
        if (GUILayout.Button(ButtonText))
        {
            SoundController.Instance.playClickSound();
            return true;
        }
        return false;
    }
}
