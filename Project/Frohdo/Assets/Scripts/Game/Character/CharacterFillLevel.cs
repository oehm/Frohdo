using UnityEngine;
using System.Collections;

public class CharacterFillLevel : MonoBehaviour {

    public string textureName_;


    private Character character_;
    private CharacterPuke characterPuke_;
    private Material material_;

	// Use this for initialization
	void Start () {
        character_ = gameObject.GetComponentInChildren<Character>();
        characterPuke_ = character_.gameObject.GetComponentInChildren<CharacterPuke>();
        material_ = character_.gameObject.GetComponentInChildren<Renderer>().material;
	}

    void Update()
    {
        material_.SetTextureOffset(textureName_, new Vector2(0.0f, 0.5f - 0.25f * characterPuke_.Ratios));
    }
	
}
