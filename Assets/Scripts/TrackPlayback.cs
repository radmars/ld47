using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public Track track;
    public AudioClip[] clips;
    public int maxClipIndex;

    private AudioSource[] audioSources;

    public ScrollingWaveformDisplay currentScrollingWaveform;
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

        audioSources = new AudioSource[clips.Length];
        for (int clipIndex = 0; clipIndex < clips.Length; clipIndex++) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clips[clipIndex];
            source.loop = true;
            audioSources[clipIndex] = source;
            source.Play();
        }
    }

    // Start is called before the first frame update
    void Start() {
        PlayClip(0);
    }

    // Update is called once per frame
    void Update() {
        if (selected) {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                DecrementClip();
            } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                IncrementClip();
            }
        }

        int samples = audioSources[currentClip].timeSamples;

        currentScrollingWaveform.updateTexture(samples);
        correctScrollingWaveform.updateTexture(samples);
        if (thumbnail) {
            thumbnail.updateTexture(samples);
        }
    }

    void ClearThumbnailCorrectness() {
        TrackThumbnail thumb = thumbnail.GetComponentInParent<TrackThumbnail>();
        if (thumb) {
            thumb.SetCorrectnessState(TrackThumbnail.CorrectnessState.Unknown);
        }
    }

    void DecrementClip() {
        int max = maxClipIndex > -1 ? maxClipIndex : clips.Length - 1;
        int targetClip = currentClip == 0 ? max : currentClip - 1;
        PlayClip(targetClip);
        ClearThumbnailCorrectness();
    }

    void IncrementClip() {
        int max = maxClipIndex > -1 ? maxClipIndex : clips.Length - 1;
        int targetClip = currentClip == max ? 0 : currentClip + 1;
        PlayClip(targetClip);
        ClearThumbnailCorrectness();
    }

    public void PlayClip(int clip) {
        currentClip = clip;

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
        currentScrollingWaveform.waveformRenderer.enabled = s;
        correctScrollingWaveform.waveformRenderer.enabled = s;
    }

    public void SetCorrectClip(int i) {
        correctClip = i;
        correctScrollingWaveform.SetClip(clips[i]);
    }

    public bool CorrectClipSelected() {
        return (currentClip == correctClip);
    }
}
