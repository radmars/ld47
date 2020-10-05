using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class ScrollingWaveformDisplay : MonoBehaviour {
    private float[] sampleCache;
    private int numChannels;
    private int numSamples = 0;
    private Texture2D texture;
    private Sprite sprite;
    public SpriteRenderer waveformRenderer;
    public Color color;

    public int width = 800;
    public int height = 500;

    private int internalWidth;
    private int internalHeight;
    private Color32[] internalTexture;
    public float numVisibleSamples = 4285.0f; // MAGIC NUMBER! samples of 1/32 note at 84bpm at 48kHz
    private float scaleFactor; // ratio of visible samples to total samples
    private float samplesPerPixel;

    private Color32 color32;
    private Color32 transparent;

    public float gain = 1.0f;

    private AudioClip lastClip;

    // Start is called before the first frame update
    void Start() {
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        waveformRenderer.sprite = sprite;
        SetColor(color);
        transparent.a = 0;
    }

    public void SetColor(Color c) {
        color = c;
        color32.r = (byte)Math.Round(color.r * 255.0);
        color32.g = (byte)Math.Round(color.g * 255.0);
        color32.b = (byte)Math.Round(color.b * 255.0);
        color32.a = (byte)Math.Round(color.a * 255.0);
        generateCache();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetClip(AudioClip clip) {
        if (clip != lastClip) {
            lastClip = clip;

            numSamples = clip.samples;
            numChannels = clip.channels;

            sampleCache = new float[clip.samples * clip.channels];
            clip.GetData(sampleCache, 0);

            generateCache();
        }
    }

    private void generateCache() {
        if (numSamples == 0) {
            return;
        }

        scaleFactor = numSamples / numVisibleSamples;
        internalWidth = (int)Math.Round(width * scaleFactor);
        internalHeight = height;
        internalTexture = new Color32[internalWidth * internalHeight];

        samplesPerPixel = (float)numSamples / (float)internalWidth;
        int samplesPerPixelRound = (int)Math.Round(samplesPerPixel);
        float sampleIndexFloat = 0.0f;

        for (int x = 0; x < internalWidth; x++)
        {
            int sampleIndex = (int)Math.Round(sampleIndexFloat);
            float min = 0.0f;
            float max = 0.0f;

            for (int innerSample = 0; innerSample < samplesPerPixelRound; innerSample++)
            {
                int sample = ((sampleIndex + innerSample) % numSamples) * numChannels;

                // average value for stereo clips
                float sampleValue = 0.0f;
                for (int c = 0; c < numChannels; c++)
                {
                    sampleValue += sampleCache[sample + c] * gain;
                }
                sampleValue /= numChannels;

                min = Math.Min(min, sampleValue);
                max = Math.Max(max, sampleValue);
            }

            int imin = (int)Math.Round(min * height / 2) + height / 2;
            int imax = (int)Math.Round(max * height / 2) + height / 2;

            for (int y = 0; y < internalHeight; y++)
            {
                int index = y * internalWidth + x;
                internalTexture[index] = (y >= imin && y <= imax) ? color32 : transparent;
            }

            sampleIndexFloat += samplesPerPixel;
        }
    }

    public void updateTexture(float startSample = 0.0f) {
        var data = texture.GetRawTextureData<Color32>();
        int xStart = (int)Math.Floor(startSample / samplesPerPixel);
        if (xStart > internalWidth - texture.width)
        {
            xStart = internalWidth - texture.width;
        }
        for (int y = 0; y < internalHeight; y++)
        {
            NativeArray<Color32>.Copy(internalTexture, y * internalWidth + xStart, data, y * texture.width, texture.width);
        }
        // upload to the GPU
        texture.Apply(false);
    }
}
