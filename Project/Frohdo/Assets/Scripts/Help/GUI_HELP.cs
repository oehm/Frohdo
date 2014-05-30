using UnityEngine;
using System.Collections;

public class GUI_HELP : MonoBehaviour {

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


    private void main()
    {
        GUILayout.BeginArea(new Rect((ForceAspectRatio.screenWidth) / 2 - 300 + ForceAspectRatio.xOffset, (ForceAspectRatio.screenHeight) / 2 - 200 + ForceAspectRatio.yOffset, 600, 400));

        if (GUILayout.Button("The Basics", skin.button))
        {
            menufunction = basics;
        }
        if (GUILayout.Button("Mixing Colors", skin.button))
        {
            menufunction = colors;
        }
        if (GUILayout.Button("Controlls", skin.button))
        {
            menufunction = controlls;
        }
        if (GUILayout.Button("Editor", skin.button))
        {
            menufunction = editor;
        }
        if (GUILayout.Button("Support and Bug report", skin.button))
        {
            menufunction = support;
        }
        if (GUILayout.Button("Back", skin.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
        }
        GUILayout.EndArea();
    }

    private void backButton()
    {
        if(GUILayout.Button("Back", skin.button))
        {
            menufunction = main;
        }
    }

    private void basics()
    {
        GUILayout.BeginArea(ForceAspectRatio.screenRect);

        GUILayout.Box(Basics, skin.box);

        backButton();
        GUILayout.EndArea();        
    }

    public void colors()
    {
        GUILayout.BeginArea(ForceAspectRatio.screenRect);

        GUILayout.Box(ColorMixing, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void controlls()
    {
        GUILayout.BeginArea(ForceAspectRatio.screenRect);

        GUILayout.Box(Controlls, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void editor()
    {
        GUILayout.BeginArea(ForceAspectRatio.screenRect);

        GUILayout.Box(Editor, skin.box);

        backButton();
        GUILayout.EndArea();
    }

    public void support()
    {
        GUILayout.BeginArea(ForceAspectRatio.screenRect);

        GUILayout.Box(Support, skin.box);

        backButton();
        GUILayout.EndArea();
    }
}
