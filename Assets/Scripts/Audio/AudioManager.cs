using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _daySongs;
    [SerializeField] private List<AudioClip> _nightSongs;

    private AudioSource _audio;
    private int _index = 0;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        DayNightCycleController.DayNightCycleControllerInstance.OnDayTimeStarted += PlayDayAudio;
        DayNightCycleController.DayNightCycleControllerInstance.OnNightTimeStarted += PlayNightAudio;
    }

    private void Update()
    {
        if (!_audio.isPlaying)
        {
            if (DayNightCycleController.IsDay)
            {
                PlayNext(_daySongs);
            }
            else
            {
                PlayNext(_nightSongs);
            }
        }
    }

    public void PlayDayAudio()
    {
        PlayNext(_daySongs);
    }

    public void PlayNightAudio()
    {
        PlayNext(_nightSongs);
    }

    private void PlayNext(List<AudioClip> clips)
    {
        _index++;

        if (_index > clips.Count)
        {
            _index = 0;
        }

        _audio.clip = clips[_index];
        _audio.Play();
    }
}
