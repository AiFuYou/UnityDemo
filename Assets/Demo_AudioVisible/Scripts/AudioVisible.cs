using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisible : MonoBehaviour {
    AudioSource audioSource;
    public  float[] samples = new float[512];
      float[] freqBand = new float[8];
      float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float _Amplitude, _AmplitudeBuffer;
    float _AmplitudeHighest;
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = freqBand[i];
            }
            _audioBand[i] = (freqBand[i]/_freqBandHighest[i]);
            _audioBandBuffer[i] = (bandBuffer[i]/_freqBandHighest[i]);
        }
    }

	void Update () {
        GetSpectrumAduioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }
    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_CurrentAmplitude > _AmplitudeHighest)
            _AmplitudeHighest = _CurrentAmplitude;
        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }
    void GetSpectrumAduioSource()
    {
//        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }
    void MakeFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2,i+1);
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count]*(count+1);
                count++;
            }
            average /= count;
            freqBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }
            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
}