using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public AudioClip correctClip;
    public AudioClip[] clips;

    public AudioSource audioSource;

    public StaticWaveformDisplay staticWaveform;

    private int currentClip = 0;

    //float numVisibleSamples = 4285.0f;

    // Start is called before the first frame update
    void Start() {
        PlayClip(0);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            PlayClip(currentClip == 0 ? clips.Length - 1 : currentClip - 1);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            PlayClip(currentClip == clips.Length - 1 ? 0 : currentClip + 1);
        }

        //staticWaveform.updateTexture(audioSource.time, numVisibleSamples);
    }

    private void PlayClip(int clip) {
        currentClip = clip;

        staticWaveform.SetClip(clips[currentClip]);
        float clipTime = audioSource.time;
        audioSource.Stop();
        audioSource.clip = clips[currentClip];
        audioSource.time = clipTime;
        audioSource.Play();
    }
}
