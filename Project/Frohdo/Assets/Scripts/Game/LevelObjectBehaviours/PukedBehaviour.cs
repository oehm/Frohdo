using UnityEngine;
using System.Collections;

public class PukedBehaviour : MonoBehaviour {

    public enum Behaviour { normal, lacquered, funnel };


    public Behaviour behaviour_;


    void Start()
    {

    }

    public void recievePuke(string color, GameObject puke)
    {
        switch (behaviour_)
        {
            case Behaviour.normal:
                gameObject.GetComponentInChildren<Colorable>().colorIn(color);
                Destroy(puke);
                break;

            case Behaviour.lacquered:
                Destroy(puke);
                break;

            case Behaviour.funnel:
                string colorBackground = SceneManager.Instance.background.GetComponentInChildren<Colorable>().colorString;
                string colorPuke = puke.GetComponentInChildren<Colorable>().colorString;

                string colorMix = LevelObjectController.Instance.GetMixColor(colorBackground, colorPuke);

                SceneManager.Instance.background.GetComponentInChildren<Colorable>().colorIn(colorMix);
                Destroy(puke);
                break;

            default:

                break;
        }
    }
    
}
