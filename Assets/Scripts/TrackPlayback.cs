using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public AudioClip correctClip;
    public AudioClip[] clips;

    public AudioSource audioSource;

    private int currentClip = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            PlayClip(currentClip == 0 ? clips.Length - 1 : currentClip - 1);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            PlayClip(currentClip == clips.Length - 1 ? 0 : currentClip + 1);
        }
    }

    private void PlayClip(int clip) {
        currentClip = clip;
        float clipTime = audioSource.time;
        audioSource.Stop();
        audioSource.clip = clips[currentClip];
        audioSource.time = clipTime;
        audioSource.Play();
    }
}
