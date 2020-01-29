using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVariation : MonoBehaviour
{

    private static Dictionary<string, float> audioFilePlayTimes = new Dictionary<string, float>();
    private static float globalAudioCooldownPerPhrase = 30;



    AudioSource audioSource;
    bool playing = false;

    public AudioClip[] clips;
    [Range(0.5f, 1.0f)]
    public float minPitch = 1.0f;
    [Range(1.0f, 1.5f)]
    public float maxPitch = 1.0f;
    public bool globalCooldown = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (clips != null && clips.Length > 0)
        {
            audioSource.clip = clips[Mathf.FloorToInt(Random.value * clips.Length)];
        }
        audioSource.pitch = (maxPitch - minPitch) * Random.value + minPitch;
        if (audioSource.playOnAwake)
        {
            Play();
        }
    }

    void Update()
    {
        if (audioSource)
        {
            if (!playing && audioSource.isPlaying)
            {
                playing = true;
                if (clips != null && clips.Length > 0)
                {
                    audioSource.clip = clips[Mathf.FloorToInt(Random.value * clips.Length)];
                }
                audioSource.pitch = (maxPitch - minPitch) * Random.value + minPitch;
                Play();
            }
            if (playing && !audioSource.isPlaying)
            {
                playing = false;
            }
        }
    }

    void Play()
    {
        if (globalCooldown)
        {
            if (audioFilePlayTimes.ContainsKey(audioSource.clip.name)
            && audioFilePlayTimes[audioSource.clip.name] + globalAudioCooldownPerPhrase < Time.time)
            {
                audioSource.Play();
                audioFilePlayTimes[audioSource.clip.name] = Time.time;
            }
            else if (!audioFilePlayTimes.ContainsKey(audioSource.clip.name))
            {
                audioSource.Play();
                audioFilePlayTimes.Add(audioSource.clip.name, Time.time);
            }
            else
            {
                audioSource.Stop();
            }
        }
        else
        {
            audioSource.Play();
        }
    }
}
