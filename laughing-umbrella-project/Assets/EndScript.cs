using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class EndScript : MonoBehaviour
{
    public TMP_Text timeText;
    VideoPlayer clip;
    float displayTimeTime = 37f;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        timeText.enabled = false;
        startTime = Time.time;
        var ts = TimeSpan.FromSeconds(MainScript.totalTime);
        timeText.text = "final time: \n" + string.Format("{0:XX}:{1:XX}:{2:XX}:{3:XXX}",ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

        clip = gameObject.GetComponent<VideoPlayer>();
        clip.waitForFirstFrame = true;
        clip.loopPointReached += EndReached;

        if (!FindObjectOfType<AudioManager>().IsPlaying("MusicMain"))
        {
            FindObjectOfType<AudioManager>().Play("MusicMain");
        }
    }
    private void Update()
    {
        if (Time.time - startTime > displayTimeTime)
        {
            timeText.enabled = true;
        }
    }



    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }
}
