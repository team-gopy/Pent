using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    private Image tutImage;
    private bool imageDisplayed = false;
    private int tutorialCount = 0;
    public List<Sprite> images = new List<Sprite>();

    private AudioSource source;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        tutImage = GetComponent<Image>();
        tutImage.CrossFadeAlpha(0f,0f,false);
        // tutImage.CrossFadeAlpha(1f,2f,false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadNextTutorial(Collider2D other)
    {
        if(!imageDisplayed && tutorialCount < images.Count)
        {
            tutImage.sprite = images[tutorialCount];
            source.PlayOneShot(clip);
            tutImage.CrossFadeAlpha(1f,1.5f,false);
            imageDisplayed = true;
            other.GetComponent<BoxCollider2D>().enabled = false;
            tutorialCount++;
        }
    }
    public void HideCurrentTutorial(Collider2D other)
    {
        if(imageDisplayed)
        {
            tutImage.CrossFadeAlpha(0f,1f,false);
            imageDisplayed = false;
            other.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
