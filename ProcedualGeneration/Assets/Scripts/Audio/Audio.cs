using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour
{
    AudioSource audio;

    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    float[] samples = new float[512];
    float[] frequencyBand = new float[8];
    float[] frequencyBandHighest = new float[8];
    public static float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
        BandBuffer();
        CreateAudioBands();
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < audioBand.Length; i++)
        {
            if(frequencyBand[i] > frequencyBandHighest[i])
            {
                frequencyBandHighest[i] = frequencyBand[i];
            }
            audioBand[i] = (frequencyBand[i] / frequencyBandHighest[i]);
            audioBandBuffer[i] = (frequencyBand[i] / frequencyBandHighest[i]);
        }
    }

    void GetSpectrumAudioSource()
    {
        audio.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBand()
    {
        int count = 0;
        for(int i = 0; i < frequencyBand.Length; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2,i) * 2;
            if(i == 7)
            {
                sampleCount += 2;
            }

            for(int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            frequencyBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        if (bandBuffer.Length != frequencyBand.Length)
        {
            Debug.Log("Band Buffer and Frequency Band have different length");
            return;
        }

        for (int i = 0; i < bandBuffer.Length; i++)
        {
            if(frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
                bufferDecrease[i] = .005f;
            }else if (frequencyBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f; 
            }
        }
    }
}
//Reference:https://www.youtube.com/watch?v=5pmoP1ZOoNs&list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo&index=1