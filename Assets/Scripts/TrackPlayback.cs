using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public Track track;
    public AudioClip[] clips;
    public int maxClipIndex;

    public AudioSource audioSource;

    public StaticWaveformDisplay currentStaticWaveform;
    public ScrollingWaveformDisplay currentScrollingWaveform;
    public StaticWaveformDisplay correctStaticWaveform;
    public ScrollingWaveformDisplay correctScrollingWaveform;

    public ScrollingWaveformDisplay thumbnail;

    private int currentClip = 0;
    private int correctClip;

    bool selected = false;
    // Eligible for player control.
    bool locked = true;

    public bool IsLocked() {
        return locked;
    }

    public void Unlock() {
        locked = false;
    }

    // Awake() happens before the Start() of any game object.
    void Awake() {
        // Needs to happen before GlobalStateContoller sets this to "true" for the appropriate track.
        SetSelected(false);
    }

    // Start is called before the first frame update
    void Start() {
        PlayClip(0);
    }

    // Update is called once per frame
    void Update() {
        if (selected) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                DecrementClip();
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                IncrementClip();
            }
        }

        currentScrollingWaveform.updateTexture(audioSource.timeSamples);
        correctScrollingWaveform.updateTexture(audioSource.timeSamples);
        if (thumbnail) {
            thumbnail.updateTexture(audioSource.timeSamples);
        }
    }

    void DecrementClip() {
        int targetClip = currentClip == 0 ? clips.Length - 1 : currentClip - 1;
        currentClip = ClampClipSelection(targetClip);
        PlayClip(currentClip);
    }

    void IncrementClip() {
        int targetClip = currentClip == clips.Length - 1 ? 0 : currentClip + 1;
        currentClip = ClampClipSelection(targetClip);
        PlayClip(currentClip);
    }

    int ClampClipSelection(int i) {
        if (maxClipIndex > -1 && i > maxClipIndex) {
            return maxClipIndex;
        }
        return i;
    }

    private void PlayClip(int clip) {
        currentClip = clip;

        currentStaticWaveform.SetClip(clips[currentClip]);
        currentScrollingWaveform.SetClip(clips[currentClip]);
        if (thumbnail) {
            thumbnail.SetClip(clips[currentClip]);
        }
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

    public void SetCorrectClip(int i) {
        correctClip = i;
        correctStaticWaveform.SetClip(clips[i]);
        correctScrollingWaveform.SetClip(clips[i]);
    }

    public bool CorrectClipSelected() {
        return (currentClip == correctClip);
    }
}
