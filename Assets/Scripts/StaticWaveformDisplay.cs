using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaticWaveformDisplay : MonoBehaviour
{
    private float[] sampleCache;
    private int numChannels;
    private int numSamples;
    private float[] waveformMins;
    private float[] waveformMaxs;
    private Texture2D texture;
    private Sprite sprite;
    public SpriteRenderer waveformRenderer;
    public Color color;

    public AudioClip lastClip;

    public int width = 800;
    public int height = 500;

    // Start is called before the first frame update
    void Start() {
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        waveformRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetClip(AudioClip clip) {
        if (clip != lastClip) {
            numSamples = clip.samples;
            numChannels = clip.channels;

            sampleCache = new float[clip.samples * clip.channels];
            waveformMins = new float[texture.width];
            waveformMaxs = new float[texture.width];
            clip.GetData(sampleCache, 0);

            updateTexture();
        }

        lastClip = clip;
    }

    public void updateTexture(float startSample = 0.0f, float numVisibleSamples = -1.0f) {
        float visible = numVisibleSamples > 0.0f ? numVisibleSamples : (float)numSamples;
        float samplesPerPixel = (float)visible / (float)texture.width;
        int samplesPerPixelRound = (int)Math.Round(samplesPerPixel);
        float sampleIndexFloat = startSample;

        for (int i = 0; i < texture.width; i++)
        {
            int sampleIndex = (int)Math.Round(sampleIndexFloat);
            for (int innerSample = 0; innerSample < samplesPerPixelRound; innerSample++)
            {
                int sample = ((sampleIndex + innerSample) % numSamples) * numChannels;

                // average value for stereo clips
                float sampleValue = 0.0f;
                for (int c = 0; c < numChannels; c++)
                {
                    sampleValue += sampleCache[sample + c];
                }
                sampleValue /= numChannels;

                waveformMins[i] = Math.Min(waveformMins[i], sampleValue);
                waveformMaxs[i] = Math.Max(waveformMaxs[i], sampleValue);
            }
            sampleIndexFloat += samplesPerPixel;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }

        for (int x = 0; x < waveformMins.Length; x++)
        {
            int min = (int)Math.Round(waveformMins[x] * height / 2) + height / 2;
            int max = (int)Math.Round(waveformMaxs[x] * height / 2) + height / 2;
            for (int y = min; y <= max; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply(false);
    }
}
