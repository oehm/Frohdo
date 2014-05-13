using UnityEngine;
using System.Collections;

public class CharacterPickup : MonoBehaviour {

    private Character character_;

    public bool Enabled { get; set; }

	// Use this for initialization
	void Start () {
        character_ = gameObject.transform.parent.gameObject.GetComponentInChildren<Character>();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (Enabled)
        {
            character_.pickUp(coll.gameObject);
        }
    }
}
