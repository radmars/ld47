using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStateController : MonoBehaviour {
    int selectedTrack = 0;
    public TrackPlayback[] tracks;
    public TrackThumbnail[] thumbnails;

    // Start is called before the first frame update
    void Start() {
        tracks[0].SetSelected(true);
        thumbnails[0].SetSelected(true);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            tracks[selectedTrack].SetSelected(false);
            thumbnails[selectedTrack].SetSelected(false);
            selectedTrack--;
            if (selectedTrack < 0) {
                selectedTrack = tracks.Length - 1;
            }
            tracks[selectedTrack].SetSelected(true);
            thumbnails[selectedTrack].SetSelected(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            tracks[selectedTrack].SetSelected(false);
            thumbnails[selectedTrack].SetSelected(false);
            selectedTrack = ++selectedTrack % tracks.Length;
            tracks[selectedTrack].SetSelected(true);
            thumbnails[selectedTrack].SetSelected(true);
        }
    }
}
