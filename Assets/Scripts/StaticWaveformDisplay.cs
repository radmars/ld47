using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaticWaveformDisplay : MonoBehaviour
{
    private float[] sampleCache;
    private float[] waveformMins;
    private float[] waveformMaxs;
    public Texture2D texture;
    public Color color;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetClip(AudioClip clip) {
        sampleCache = new float[clip.samples];
        waveformMins = new float[texture.width];
        waveformMaxs = new float[texture.width];
        clip.GetData(sampleCache, 0);

        float samplesPerPixel = (float)clip.samples / (float)texture.width;
        int samplesPerPixelRound = (int)Math.Round(samplesPerPixel);
        float sampleIndexFloat = 0.0f;

        for (int i = 0; i < texture.width; i++) {
            int sampleIndex = (int)Math.Round(sampleIndexFloat);
            for (int innerSample = 0; innerSample < samplesPerPixelRound && sampleIndex + innerSample < clip.samples; innerSample++)
            {
                int sample = (sampleIndex + innerSample) * clip.channels;

                // average value for stereo clips
                float sampleValue = 0.0f;
                for (int c = 0; c < clip.channels; c++)
                {
                    sampleValue += sampleCache[sample + c];
                }
                sampleValue /= clip.channels;

                waveformMins[i] = Math.Min(waveformMins[i], sampleValue);
                waveformMaxs[i] = Math.Max(waveformMaxs[i], sampleValue);
            }
            sampleIndexFloat += samplesPerPixel;
        }

        updateTexture();
    }

    void updateTexture() {
        int width = texture.width;
        int height = texture.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }

        /*for (int x = 0; x < waveformCache.Length; x++)
        {
            for (int y = 0; y <= waveformCache[x] * ((float)height * .75f); y++)
            {
                texture.SetPixel(x, (height / 2) + y, color);
                texture.SetPixel(x, (height / 2) - y, color);
            }
        }*/
        texture.Apply();
    }
}
