﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSheet : MonoBehaviour
{
    public float FPS = 35f;
    public Image outputRenderer;
    public Sprite[] frames;
    public bool doLoop = true;

    [HideInInspector]
    public bool destroyThis = false;
    [HideInInspector]
    public int currentFrame;
    //[HideInInspector]
    public int startingFrame = -1;

    private float secondsToWait;
    private bool stopped = false;

    public void Awake()
    {
        currentFrame = 0;
        if (FPS > 0)
            secondsToWait = 1 / FPS;
        else
            secondsToWait = 0f;
    }

    public void Update()
    {
        if(destroyThis)
        {
            Destroy(gameObject);
        }
    }

    public void Play()
    {
        if (startingFrame != -1)
        {
            currentFrame = startingFrame;
        }
        else
        {
            currentFrame = 0;
        }

        stopped = false;
        outputRenderer.enabled = true;

        if (frames.Length > 1)
        {
            Animate();
        }
        else if (frames.Length > 0)
        {
            outputRenderer.sprite = frames[0];
        }
    }

    public void Stop()
    {
        stopped = true;
        currentFrame = 0;
    }

    public virtual void Animate()
    {
        CancelInvoke("Animate");
        if (currentFrame >= frames.Length)
        {
            if (doLoop)
            {
                currentFrame = 0;
            }
            else
            {
                Stop();
            }

        }

        outputRenderer.sprite = frames[currentFrame];

        if (!stopped)
        {
            currentFrame++;
        }

        if (!stopped && secondsToWait > 0)
        {
            Invoke("Animate", secondsToWait);
        }
    }

    private void OnEnable()
    {
        Play();
    }

    private void OnDisable()
    {
        Stop();
    }

}
