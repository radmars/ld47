using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStateController : MonoBehaviour {
    int selectedTrack = 0;
    // This needs to match the index first instrument used.
    int currentStageIndex = 0;
    List<Stage> stages = new List<Stage>();
    public TrackPlayback[] tracks;
    public TrackThumbnail[] thumbnails;

    // Start is called before the first frame update
    void Start() {
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Drums, 1)
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Arp, 1),
            new TrackSpec(Track.Drums, 1)
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Arp, 2),
            new TrackSpec(Track.Drums, 1)
        }));

        SetStage(currentStageIndex);
        tracks[selectedTrack].SetSelected(true);
        thumbnails[selectedTrack].SetSelected(true);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            tracks[selectedTrack].SetSelected(false);
            thumbnails[selectedTrack].SetSelected(false);
            do {
                selectedTrack--;
                if (selectedTrack < 0) {
                    selectedTrack = tracks.Length - 1;
                }
            } while (tracks[selectedTrack].IsLocked());

            tracks[selectedTrack].SetSelected(true);
            thumbnails[selectedTrack].SetSelected(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            tracks[selectedTrack].SetSelected(false);
            thumbnails[selectedTrack].SetSelected(false);
            selectedTrack = ++selectedTrack % tracks.Length;
            do {
                selectedTrack = ++selectedTrack % tracks.Length;
            } while (tracks[selectedTrack].IsLocked());

            tracks[selectedTrack].SetSelected(true);
            thumbnails[selectedTrack].SetSelected(true);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            NextStage();
        }

        if (StageComplete()) {
            NextStage();
        }
    }

    bool StageComplete() {
        foreach (TrackPlayback i in tracks) {
            if (!i.CorrectClipSelected()) {
                return false;
            }
        }
        return true;
    }

    void NextStage() {
        currentStageIndex++;
        SetStage(currentStageIndex);
    }

    void SetStage(int stageIndex) {
        Stage stage = stages[stageIndex];
        foreach (TrackSpec i in stage.tracks) {
            // Should be using a Dictionary for one or the other as an optimization.
            foreach (TrackPlayback j in tracks) {
                if (i.track == j.track) {
                    j.Unlock();
                    j.SetCorrectClip(i.correctClip);
                    j.maxClipIndex = i.maxClipIndex;
                }
            }
        }
    }
}

/**
 * Specifies the set of clips enabled and the correct clip for a given track
 * for a specific stage.
 */
public class TrackSpec {
    // What track this spec is for.
    public Track track;
    // Index of the correct clip.
    public int correctClip;
    // Maximum index of the clip enabled. If not specified, defaults to all tracks (-1).
    public int maxClipIndex;

    public TrackSpec(Track track, int correctClip) {
        this.track = track;
        this.correctClip = correctClip;
        this.maxClipIndex = -1;
    }

    // Full constructor, for when not all clips are available.
    public TrackSpec(Track track, int correctClip, int maxClipIndex) {
        this.track = track;
        this.correctClip = correctClip;
        this.maxClipIndex = maxClipIndex;
    }
}

public class Stage {
    public TrackSpec[] tracks;

    public Stage(TrackSpec[] tracks) {
        this.tracks = tracks;
    }
}
