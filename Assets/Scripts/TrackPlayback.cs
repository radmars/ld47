using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public Track track;
    public AudioClip[] clips;
    public int maxClipIndex;

    private AudioSource[] audioSources;

    public StaticWaveformDisplay currentStaticWaveform;
    public ScrollingWaveformDisplay currentScrollingWaveform;
    public StaticWaveformDisplay correctStaticWaveform;
    public ScrollingWaveformDisplay correctScrollingWaveform;

    public ScrollingWaveformDisplay thumbnail;

    private int currentClip = 0;
    private int correctClip;

    bool selected = false;
    // Only unlocked tracks are eligible for player control.
    bool locked = true;

    public bool IsLocked() {
        return locked;
    }

    public void Unlock() {
        locked = false;
    }

    public void Lock() {
        locked = true;
    }

    // Awake() happens before the Start() of any game object.
    void Awake() {
        // Needs to happen before GlobalStateContoller sets this to "true" for the appropriate track.
        SetSelected(false);
    }

    // Start is called before the first frame update
    void Start() {
        audioSources = new AudioSource[clips.Length];
        for (int clipIndex = 0; clipIndex < clips.Length; clipIndex++) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clips[clipIndex];
            source.loop = true;
            audioSources[clipIndex] = source;
            source.Play();
        }
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

        currentScrollingWaveform.updateTexture(audioSources[currentClip].timeSamples);
        correctScrollingWaveform.updateTexture(audioSources[currentClip].timeSamples);
        if (thumbnail) {
            thumbnail.updateTexture(audioSources[currentClip].timeSamples);
        }
    }

    void DecrementClip() {
        int targetClip = currentClip == 0 ? clips.Length - 1 : currentClip - 1;
        SetClip(ClampClipSelection(targetClip));
    }

    void IncrementClip() {
        int targetClip = currentClip == clips.Length - 1 ? 0 : currentClip + 1;
        SetClip(ClampClipSelection(targetClip));
    }

    int ClampClipSelection(int i) {
        if (maxClipIndex > -1 && i > maxClipIndex) {
            return maxClipIndex;
        }
        return i;
    }

    public void SetClip(int clip) {
        currentClip = clip;
        PlayClip(currentClip);
    }

    private void PlayClip(int clip) {
        currentClip = clip;

        currentStaticWaveform.SetClip(clips[currentClip]);
        currentScrollingWaveform.SetClip(clips[currentClip]);
        if (thumbnail) {
            thumbnail.SetClip(clips[currentClip]);
        }
        
        foreach (AudioSource source in audioSources) {
            source.mute = true;
        }
        audioSources[currentClip].mute = false;
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
