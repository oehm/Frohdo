using UnityEngine;
using System.Collections;

public class GUI_HELP : MonoBehaviour
{

    private delegate void Menudelegate();
    private Menudelegate menufunction;

    public GUISkin skin;

    public Texture Basics;
    public Texture ColorMixing;
    public Texture Controlls;
    public Texture Editor;
    public Texture Support;

    void Awake()
    {
        menufunction = main;
    }

    void OnGUI()
    {
        menufunction();
    }

    void Update()
    {
        skin.box.fixedHeight = ForceAspectRatio.screenHeight - 80;
    }


    private void main()
    {
        GUILayout.BeginArea(new Rect((ForceAspectRatio.screenWidth) / 2 - 300 + ForceAspectRatio.xOffset, (ForceAspectRatio.screenHeight) / 2 - 200 + ForceAspectRatio.yOffset, 600, 400));

        if (SoundButton.newSoundButton("The Basics", skin.button))
        {
            menufunction = basics;
        }
        if (SoundButton.newSoundButton("Mixing Colors", skin.button))
        {
            menufunction = colors;
        }
        if (SoundButton.newSoundButton("Controlls", skin.button))
        {
            menufunction = controlls;
        }
        if (SoundButton.newSoundButton("Editor", skin.button))
        {
            menufunction = editor;
        }
        if (SoundButton.newSoundButton("Support and Bug report", skin.button))
        {
            menufunction = support;
        }
        if (SoundButton.newSoundButton("Back", skin.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
        }
        GUILayout.EndArea();
    }

    private void areaFullScreen()
    {
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, ForceAspectRatio.screenWidth, ForceAspectRatio.screenHeight));
    }

    private void backButton()
    {
        if (SoundButton.newSoundButton("Back", skin.button))
        {
            menufunction = main;
        }
    }

    private void basics()
    {
        areaFullScreen();

        GUILayout.Box(Basics, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void colors()
    {
        areaFullScreen();

        GUILayout.Box(ColorMixing, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void controlls()
    {
        areaFullScreen();

        GUILayout.Box(Controlls, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void editor()
    {
        areaFullScreen();

        GUILayout.Box(Editor, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void support()
    {
        areaFullScreen();

        GUILayout.Box(Support, skin.box);

        backButton();
        GUILayout.EndArea();
    }
}
