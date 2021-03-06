﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackThumbnail : MonoBehaviour {
    public enum CorrectnessState {
        Unknown,
        Wrong,
        Correct
    }

    private Color[] stateColors;
    public Color unknown;
    public Color wrong;
    public Color correct;

    public ScrollingWaveformDisplay waveform;
    public SpriteRenderer selection;

    // Awake() happens before the Start() of any game object.
    void Awake() {
        // Needs to happen before GlobalStateContoller sets this to "true" for the appropriate track.
        SetSelected(false);
        stateColors = new Color[3];
        stateColors[(int)CorrectnessState.Unknown] = unknown;
        stateColors[(int)CorrectnessState.Wrong] = wrong;
        stateColors[(int)CorrectnessState.Correct] = correct;
        SetCorrectnessState(CorrectnessState.Unknown);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetCorrectnessState(CorrectnessState s) {
        waveform.SetColor(stateColors[(int)s]);
    }

    public void SetActive(bool active) {
        waveform.waveformRenderer.enabled = active;
    }

    public void SetSelected(bool selected) {
        selection.enabled = selected;
    }
}
