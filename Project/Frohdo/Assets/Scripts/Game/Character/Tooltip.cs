using UnityEngine;
using System.Collections;

public class Tooltip : MonoBehaviour
{

    private GameObject curtooltip;
    private float timetoLive;

    public GameObject Tooltip_color_;
    public GameObject character;

    public void setTooltip(float ttl, string colorOfRatio)
    {
        if (curtooltip != null)
        {
            Destroy(curtooltip);
        }

        curtooltip = (GameObject)Instantiate(Tooltip_color_);
        timetoLive = ttl;
        curtooltip.GetComponent<Colorable>().color_ = LevelObjectController.Instance.GetMixColor(character.GetComponentInChildren<Colorable>().colorString, colorOfRatio);
        curtooltip.transform.parent = character.transform;
        curtooltip.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + character.GetComponentInChildren<Gridable>().height/2+0.3f, character.transform.position.z);
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curtooltip != null)
        {
            timetoLive -= Time.deltaTime;
            if (timetoLive < 0)
            {
                Destroy(curtooltip);
            }
            else
            {
                Color color = curtooltip.GetComponent<Renderer>().material.color;
                color.a = Mathf.Min(timetoLive, 1);
                curtooltip.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
