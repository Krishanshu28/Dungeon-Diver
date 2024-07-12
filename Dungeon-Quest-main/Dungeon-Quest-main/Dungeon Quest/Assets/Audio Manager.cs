using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour 
{
    public Sound[] clips;
    public Slider volumeSlider;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in clips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.PlayonAwake;
        }
    }
    private void Update()
    {
        volumeSlider.onValueChanged.AddListener(delegate{ VolumeChange(); });
    }
    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(clips, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log(name + " sound not found");
            return;
        }
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(clips, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log(name + " sound not found");
            return;
        }
        s.source.Stop();
    }
    public bool isPlay(string name)
    {
        Sound s = Array.Find(clips, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log(name + " sound not found");
            return false;
        }
        return s.source.isPlaying;
    }
    public void VolumeChange()
    {
        GameObject.FindWithTag("Player").GetComponent<RPlayer>().volume = volumeSlider.value;
        foreach (Sound s in clips)
        {
            s.source.volume = s.volume * volumeSlider.value;
        }
    }
}
