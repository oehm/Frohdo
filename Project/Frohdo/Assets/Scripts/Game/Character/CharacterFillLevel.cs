using UnityEngine;
using System.Collections;

public class CharacterFillLevel : MonoBehaviour {

    public string textureName_;


    private Character character_;
    private CharacterPuke characterPuke_;
    private Material material_;

    private int level_;

	// Use this for initialization
	void Start () {
        character_ = gameObject.GetComponentInChildren<Character>();
        characterPuke_ = character_.gameObject.GetComponentInChildren<CharacterPuke>();
        material_ = character_.gameObject.GetComponentInChildren<Renderer>().material;
        level_ = characterPuke_.Ratios;
        material_.SetTextureOffset(textureName_, new Vector2(0.0f, 0.5f - 0.25f * characterPuke_.Ratios));
	}

    void Update()
    {
        if (level_ != characterPuke_.Ratios)
        {
            material_.SetTextureOffset(textureName_, new Vector2(0.0f, 0.5f - 0.25f * characterPuke_.Ratios));
            level_ = characterPuke_.Ratios;
        }

    }
	
}
