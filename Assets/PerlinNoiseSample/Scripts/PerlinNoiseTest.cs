﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTest : MonoBehaviour
{
    [SerializeField]
    private GameObject _quad;

    [SerializeField]
    [Range(1, 16)]
    private int _octaves = 5;

    [SerializeField]
    [Range(0.1f, 64.0f)]
    private float _frequency = 32.0f;

    [SerializeField]
    private float _persistence = 0.5f;

    [SerializeField]
    private int _width = 512;

    [SerializeField]
    private int _height = 512;

    [SerializeField]
    private uint _seed = 1000;

    private PerlinNoise _noise;
    private PerlinNoise Noise
    {
        get
        {
            if (_noise == null)
            {
                _noise = new PerlinNoise(_seed);
            }
            return _noise;
        }
    }
    private Texture2D _texture;

    private void Start()
    {
        CreateNoise();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            CreateNoise();
        }
    }

    private void CreateNoise()
    {
        _texture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);

        Noise.Frequency = _frequency;

        Color[] pixels = new Color[_width * _height];
        float fx = 1f / (float)_width;// / _frequency;
        float fy = 1f / (float)_height; // / _frequency;
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % _width;
            int y = i / _width;
            float n = Noise.OctaveNoise(x * fx, y * fy, _octaves, _persistence);
            float c = Mathf.Clamp(218f * (0.5f + n * 0.5f), 0f, 255f) / 255f;
            pixels[i] = new Color(c, c, c, 1f);
        }

        _texture.SetPixels(0, 0, _width, _height, pixels);
        _texture.Apply();

        Renderer renderer = _quad.GetComponent<Renderer>();
        renderer.material.mainTexture = _texture;
    }
}