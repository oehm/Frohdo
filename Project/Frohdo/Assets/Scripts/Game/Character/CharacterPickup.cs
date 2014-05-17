using UnityEngine;
using System.Collections;

public class CharacterPickup : MonoBehaviour {

    private Character character_;

    public bool EnabledUp { get; set; }
    public bool EnabledDown { get; set; }


	// Use this for initialization
	void Start () {
        character_ = gameObject.transform.parent.gameObject.GetComponentInChildren<Character>();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (EnabledUp && coll.gameObject.GetComponentInChildren<Usable>() != null)
        {
            character_.use(coll.gameObject);
        }

        if (EnabledDown && coll.gameObject.GetComponentInChildren<Collectable>() != null)
        {
            character_.pickUp(coll.gameObject);
        }
    }
}
