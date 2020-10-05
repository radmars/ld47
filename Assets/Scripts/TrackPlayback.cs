using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayback : MonoBehaviour {
    public AudioClip correctClip;
    public AudioClip[] clips;

    private AudioSource[] audioSources;

    public StaticWaveformDisplay currentStaticWaveform;
    public ScrollingWaveformDisplay currentScrollingWaveform;
    public StaticWaveformDisplay correctStaticWaveform;
    public ScrollingWaveformDisplay correctScrollingWaveform;

    public ScrollingWaveformDisplay thumbnail;

    private int currentClip = 0;

    bool selected = false;

    // Start is called before the first frame update
    void Start() {
        correctStaticWaveform.SetClip(correctClip);
        correctScrollingWaveform.SetClip(correctClip);
        SetSelected(false);

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
                PlayClip(currentClip == 0 ? clips.Length - 1 : currentClip - 1);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                PlayClip(currentClip == clips.Length - 1 ? 0 : currentClip + 1);
            }
        }

        currentScrollingWaveform.updateTexture(audioSources[currentClip].timeSamples);
        correctScrollingWaveform.updateTexture(audioSources[currentClip].timeSamples);
        if (thumbnail) {
            thumbnail.updateTexture(audioSources[currentClip].timeSamples);
        }
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
}
