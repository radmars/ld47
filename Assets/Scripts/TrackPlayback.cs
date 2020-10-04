using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public AudioClip correctClip;
    public AudioClip[] clips;

    public AudioSource audioSource;

    public StaticWaveformDisplay currentStaticWaveform;
    public ScrollingWaveformDisplay currentScrollingWaveform;
    public StaticWaveformDisplay correctStaticWaveform;
    public ScrollingWaveformDisplay correctScrollingWaveform;

    private int currentClip = 0;

    bool selected = false;

    // Start is called before the first frame update
    void Start() {
        PlayClip(0);
        correctStaticWaveform.SetClip(correctClip);
        correctScrollingWaveform.SetClip(correctClip);
        SetSelected(false);
    }

    // Update is called once per frame
    void Update() {
        if (selected) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                PlayClip(currentClip == 0 ? clips.Length - 1 : currentClip - 1);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                PlayClip(currentClip == clips.Length - 1 ? 0 : currentClip + 1);
            }
        }

        currentScrollingWaveform.updateTexture(audioSource.timeSamples);
        correctScrollingWaveform.updateTexture(audioSource.timeSamples);
    }

    private void PlayClip(int clip) {
        currentClip = clip;

        currentStaticWaveform.SetClip(clips[currentClip]);
        currentScrollingWaveform.SetClip(clips[currentClip]);
        float clipTime = audioSource.time;
        audioSource.Stop();
        audioSource.clip = clips[currentClip];
        audioSource.time = clipTime;
        audioSource.Play();
    }

    public void SetSelected(bool s) {
        selected = s;
        currentStaticWaveform.waveformRenderer.enabled = s;
        currentScrollingWaveform.waveformRenderer.enabled = s;
        correctStaticWaveform.waveformRenderer.enabled = s;
        correctScrollingWaveform.waveformRenderer.enabled = s;
    }
}
