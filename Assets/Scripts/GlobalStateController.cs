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

    public TextMesh stageText;

    void Awake() {
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 3, 4),
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 3, 4),
            new TrackSpec(Track.Arp, 4),
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 3, 4),
            new TrackSpec(Track.Arp, 4),
            new TrackSpec(Track.Melody, 3),
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 3, 4),
            new TrackSpec(Track.Arp, 4),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 4),
        }));
        // Bass options +more, bass answer -> correct2
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 7, 7),
            new TrackSpec(Track.Arp, 4),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 4),
        }));
        // Bass options +more, bass answer -> correct3
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 8, 9),
            new TrackSpec(Track.Arp, 4),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 4),
            new TrackSpec(Track.Pad3, 2),
        }));
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 2),
            new TrackSpec(Track.Bass, 8, 9),
            new TrackSpec(Track.Arp, 4),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 4),
            new TrackSpec(Track.Pad3, 2),
            new TrackSpec(Track.Lead, 2),
        }));
        // Pad2 answer -> silence
        // Drums answer -> correct2
        // Lead answer -> silence
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 2),
            new TrackSpec(Track.Pad2, 0),
            new TrackSpec(Track.Bass, 8, 9),
            new TrackSpec(Track.Arp, 4),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 2),
            new TrackSpec(Track.Pad3, 2),
            new TrackSpec(Track.Lead, 0),
        }));
        // Lead answer -> silence
        // Pad1 answer -> silence
        // Pad3 answer -> silence
        // Arp answer -> silence
        // Drums answer -> silence
        // Bass answer -> correct4
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 0),
            new TrackSpec(Track.Pad2, 0),
            new TrackSpec(Track.Bass, 9, 9),
            new TrackSpec(Track.Arp, 0),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 0),
            new TrackSpec(Track.Pad3, 0),
            new TrackSpec(Track.Lead, 0),
        }));
        // Bass answer -> silence
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 0),
            new TrackSpec(Track.Pad2, 0),
            new TrackSpec(Track.Bass, 0),
            new TrackSpec(Track.Arp, 0),
            new TrackSpec(Track.Melody, 3),
            new TrackSpec(Track.Drums, 0),
            new TrackSpec(Track.Pad3, 0),
            new TrackSpec(Track.Lead, 0),
        }));
        // Melody answer -> silence
        stages.Add(new Stage(new TrackSpec[] {
            new TrackSpec(Track.Pad1, 0),
            new TrackSpec(Track.Pad2, 0),
            new TrackSpec(Track.Bass, 0),
            new TrackSpec(Track.Arp, 0),
            new TrackSpec(Track.Melody, 0),
            new TrackSpec(Track.Drums, 0),
            new TrackSpec(Track.Pad3, 0),
            new TrackSpec(Track.Lead, 0),
        }));
    }

    // Start is called before the first frame update
    void Start() {
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
            do {
                selectedTrack = ++selectedTrack % tracks.Length;
            } while (tracks[selectedTrack].IsLocked());

            tracks[selectedTrack].SetSelected(true);
            thumbnails[selectedTrack].SetSelected(true);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            if (StageComplete()) {
                NextStage();
            }
        }
    }

    // Checks if stage is complete (ie: all tracks are correct), also sets track thumbnail state for correctness
    bool StageComplete() {
        bool complete = true;

        int thumb = 0;
        foreach (TrackPlayback i in tracks) {
            if (!i.CorrectClipSelected()) {
                complete = false;
                thumbnails[thumb].SetCorrectnessState(TrackThumbnail.CorrectnessState.Wrong);
            } else {
                thumbnails[thumb].SetCorrectnessState(TrackThumbnail.CorrectnessState.Correct);
            }
            thumb++;
        }

        return complete;
    }

    void NextStage() {
        currentStageIndex = (currentStageIndex + 1) % stages.Count;
        SetStage(currentStageIndex);
    }

    void SetStage(int stageIndex) {
        stageText.text = "stage " + (stageIndex+1);
        Stage stage = stages[stageIndex];
        int index = 0;
        foreach (TrackPlayback i in tracks) {
            thumbnails[index].SetCorrectnessState(TrackThumbnail.CorrectnessState.Unknown);

            bool trackInLevel = false;
            // Should be using a Dictionary for one or the other as an optimization.
            foreach (TrackSpec j in stage.tracks) {
                if (i.track == j.track) {
                    trackInLevel = true;
                    i.Unlock();
                    thumbnails[index].SetActive(true);
                    i.SetCorrectClip(j.correctClip);
                    i.maxClipIndex = j.maxClipIndex;
                    break;
                }
            }

            if (!trackInLevel) {
                thumbnails[index].SetActive(false);
                i.Lock();

                // try to make sure selection isn't selecting a locked track
                if (selectedTrack == index) {
                    i.SetSelected(false);
                    thumbnails[index].SetSelected(false);
                    while (tracks[selectedTrack].IsLocked()) {
                        selectedTrack = (selectedTrack + 1) % tracks.Length;
                    }
                    tracks[selectedTrack].SetSelected(true);
                    thumbnails[selectedTrack].SetSelected(true);
                }

                i.PlayClip(0); // Silent track
            }
            index++;
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
