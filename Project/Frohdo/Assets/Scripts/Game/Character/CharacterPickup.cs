using UnityEngine;
using System.Collections;

public class CharacterPickup : MonoBehaviour {

    private Character character_;

    public bool EnabledUp { get{return enabledUp_;} set{enabledUp_ = value; if(value == false) disabledUp_ = false;} }
    public bool EnabledDown { get{return enabledDown_;} set{enabledDown_ = value; if(value == false) disabledDown_ = false;} }
    
    private bool enabledUp_;
    private bool enabledDown_;

    private bool disabledUp_;
    private bool disabledDown_;

	// Use this for initialization
	void Start () {
        character_ = gameObject.transform.parent.gameObject.GetComponentInChildren<Character>();

        disabledUp_ = false;
        disabledDown_ = false;
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!disabledUp_ && EnabledUp && coll.gameObject.GetComponentInChildren<Usable>() != null)
        {
            character_.use(coll.gameObject);
            disabledUp_ = true;
        }

        if (!disabledDown_ && EnabledDown && coll.gameObject.GetComponentInChildren<Collectable>() != null)
        {
            character_.pickUp(coll.gameObject);
            disabledDown_ = true;

        }
    }
}
