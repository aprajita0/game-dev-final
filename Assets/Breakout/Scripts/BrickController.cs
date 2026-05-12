using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class BrickController : MonoBehaviour
{
    public SpriteRenderer SR;
    public int ScoreValue = 100;
    public int HitsToBreak = 1;
    private int currentHits;

    public bool UseRandomColor = true;
    public Color NormalColor = Color.white;
    public Color DamagedColor = Color.gray;
    public AudioClip BreakSound;
    public float BreakSoundVolume = 1f;
    
    void Awake()
    {
        if (BreakoutManager.Me != null)
        {
            BreakoutManager.Me.AllBricks.Add(this);
        }   
    }
    
    
    void Start()
    {
        currentHits = HitsToBreak;
        UpdateColor();
    }

    public void SetupBrick(int hitsToBreak, int scoreValue, Color normalColor, Color damagedColor)
    {
        HitsToBreak = hitsToBreak;
        ScoreValue = scoreValue;
        NormalColor = normalColor;
        DamagedColor = damagedColor;
        currentHits = HitsToBreak;
        UpdateColor();
    }

    public void TakeHit()
    {
        currentHits--;
        if (currentHits <= 0)
        {
            Break();
        }
        else
        {
            UpdateColor();
        }
    }


    private void UpdateColor()
    {
        if (SR == null)
            return;
        if (currentHits < HitsToBreak)
        {
            SR.color = DamagedColor;
        }

        else
        {
            SR.color = NormalColor;
        }
        
    }

    //This code makes the brick break
    public void Break()
    {
        //Destroy the brick
        //If we wanted to make any fancy effects, we could do that here

        //Play a sound when break
        if (BreakSound != null)
        {
            AudioSource.PlayClipAtPoint(BreakSound, transform.position, BreakSoundVolume);
        }
        
        //Add score to the player
        if (BreakoutManager.Me != null)
        {
            BreakoutManager.Me.AddScore(ScoreValue);
        }

        Destroy(gameObject);
    }

    //This gets called by Unity when the object is destroyed
    private void OnDestroy()
    {
        //Remove me from the list of existing bricks
        if (BreakoutManager.Me != null)
        {
            BreakoutManager.Me.AllBricks.Remove(this);
        }
       
    }
}
