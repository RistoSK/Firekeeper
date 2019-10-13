using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomOffset : MonoBehaviour
{
    [SerializeField] private float _offsetMax = 1;
    [SerializeField] private float _delayBetween = 4f;
    [SerializeField] private float _pitchRange = 1f;
    [SerializeField] private float _deleteAmount = 0.8f;
    [SerializeField] private bool _canPlay = true;

    private AudioSource _audio;

    private void Start()
    {
        float chance = Random.Range(0f, 1f);

        if (chance > _deleteAmount)
        {
            Destroy(_audio);
            Destroy(this);
        }
        else
        {
            _audio = GetComponent<AudioSource>();
            _audio.Stop();

            float pitch = 1 + Random.Range(-_pitchRange, _pitchRange);
            _audio.pitch = pitch;

            float delay = Random.Range(0, _offsetMax);
            StartCoroutine(Delay(delay));
        }
    }

    private IEnumerator Delay(float delay)
    {
        _canPlay = false;
        yield return new WaitForSeconds(delay);
        _audio.Play();
        _canPlay = true;
    }

    private void Update()
    {
        if (!_audio.isPlaying && _canPlay)
        {
            float delay = Random.Range(0, _delayBetween);
            StartCoroutine(Delay(delay));
        }
    }

}
