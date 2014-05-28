using UnityEngine;
using System.Collections;

public class CharacterPuke : MonoBehaviour
{

    //public
    public float pukeForceSide_;
    public float pukeForceUp_;
    public float pukeForceDown_;

    public GameObject pukePrefab_;

    public Vector2 pukeOffSide_;
    public Vector2 pukeOffUp_;
    public Vector2 pukeOffDown_;

    public AudioSource pukesound_;


    //private
    private Character character_;
    private Colorable colorable_;
    private Animator animator_;


    private bool pukeInput_;
    private bool isPuking_;
    private float animationTimeCount_;
    private bool playPukeSound_;


    private int ratios_;
    public int Ratios { get { return ratios_; } }

	// Use this for initialization
    void Start()
    {
        character_ = gameObject.GetComponent<Character>();
        colorable_ = character_.Colorable;
        animator_ = character_.gameObject.GetComponentInChildren<Animator>();
        //colorable_.gameObject.GetComponent<Renderer>().material.SetTextureOffset("_OverlayTex", new Vector2(0.0f, 0.5f));
        ratios_ = 0;
        colorable_.colorString = "W";
        isPuking_ = false;
        playPukeSound_ = false;
        animationTimeCount_ = 0.0f;
        pukesound_.volume = PlayerPrefs.GetFloat("MiscVolume");
	}


    public void InputMovement(bool puke)
    {
        pukeInput_ = puke;
    }

    void Update()
    {
        if (playPukeSound_)
        {
            playPukeSound_ = false;
            pukesound_ = gameObject.GetComponentInChildren<AudioSource>();
            if (pukesound_.isPlaying) pukesound_.Stop();
            pukesound_.clip = SoundController.Instance.getRandomPukeSound();
            if (pukesound_.clip != null) pukesound_.Play();
        }

        if (isPuking_)
        {
            animationTimeCount_ += Time.deltaTime;
            if (animationTimeCount_ >= GlobalVars.Instance.pukeTime)
            {
                Puke();
                isPuking_ = false;
            }
        }
    }


    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
        if (pukeInput_ && ratios_ > 0)
        {
            isPuking_ = true;
            animationTimeCount_ = 0.0f;
            animator_.SetTrigger("puke");
            playPukeSound_ = true;
        }
    }


    private void Puke()
    {
        ScoreController.Instance.CountAPuke();

        GameObject pukeObject = (GameObject)Instantiate(pukePrefab_);

        pukeObject.GetComponentInChildren<Colorable>().colorString = colorable_.colorString;

        Vector2 pukePos = character_.transform.position;

        if (character_.lookUp)
        {
            pukePos += pukeOffUp_;
            pukeObject.rigidbody2D.AddForce(new Vector2(0.0f, pukeForceUp_));
        }
        else if (character_.lookDown)
        {
            pukePos += pukeOffDown_;
            pukeObject.rigidbody2D.AddForce(new Vector2(0.0f, -pukeForceDown_));
        }
        else
        {
            if (character_.lookLeft)
            {
                pukePos.x -= pukeOffSide_.x;
                pukePos.y += pukeOffSide_.y;
                pukeObject.rigidbody2D.AddForce(new Vector2(-pukeForceSide_, 0.0f));
            }
            else
            {
                pukePos += pukeOffSide_;
                pukeObject.rigidbody2D.AddForce(new Vector2(pukeForceSide_, 0.0f));
            }
        }


        pukeObject.transform.position = pukePos;
        pukeObject.transform.parent = character_.gameObject.transform.parent;

        pukeObject.GetComponent<Rigidbody2D>().velocity = character_.gameObject.GetComponent<Rigidbody2D>().velocity;

        ratios_--;

        if (ratios_ == 0)
        {
            colorable_.colorIn("W");
        }


        //colorable_.gameObject.GetComponent<Renderer>().material.SetTextureOffset("_OverlayTex", new Vector2(0.0f, 0.5f - 0.25f * ratios_));
    }

    public void AddRatio(string color)
    {
        string newColor = LevelObjectController.Instance.GetMixColor(colorable_.colorString, color);
        colorable_.colorIn(newColor);

        if(ratios_ < GlobalVars.Instance.maxRatios)
            ratios_++;

        //colorable_.gameObject.GetComponent<Renderer>().material.SetTextureOffset("_OverlayTex", new Vector2(0.0f, 0.5f - 0.25f * ratios_));

    }
}
