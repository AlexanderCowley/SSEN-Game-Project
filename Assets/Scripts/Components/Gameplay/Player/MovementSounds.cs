using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepClips;
    [SerializeField] private float _repeatDelay = 0.25f;
    private float _repeatTimer;
    private bool _canPlay;
    private Animator _playerAnimator;
    private AudioSource _audioSource; 
    private int currentClipElement = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        SoundTimer();
        PlaySequencialSounds();
    }

    private void SoundTimer()
    {
        if (_repeatTimer < _repeatDelay)
        {
            _repeatTimer += Time.deltaTime;
            _canPlay = false;
        }
        else
        {
            _repeatTimer = 0;
            _canPlay = true;
        }
    }

    private void NextClip()
    {
        if (currentClipElement < _footstepClips.Length - 1)
        {
            currentClipElement++;            
        }
        else
        {
            currentClipElement = 0;
        }
        // _audioSource.clip = _footstepClips[currentClipElement];
    }

    private void PlaySequencialSounds()
    {
        if (_playerAnimator.GetBool("isMoving") && _canPlay)
        {
            _canPlay = false;
            _audioSource.PlayOneShot(_footstepClips[currentClipElement]);
            NextClip();
        }        
    }
}
