using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QiMeter : MonoBehaviour
{
    [SerializeField] private float _timeRate = 0.03f;
    [SerializeField] private float _pointRate = 0.05f;
    [SerializeField] private Image _qiImageLow, _qiImageMid, _qiImageHigh;
    [SerializeField] private Slider _qiSlider;
    [SerializeField] private AudioClip _fullClip;
    private AudioSource _audioSource;

    public bool full { get; private set; }

    private void Awake()
    {
        ResetQiMeter();
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateVisual();
        IncreaseByTime();
    }

    private void IncreaseByTime()
    {
        if (_qiSlider.value < _qiSlider.maxValue)
        {
            _qiSlider.value += Time.deltaTime * _timeRate;
            full = false;
        }
        else
        {
            if (!full)
            {
                _audioSource.PlayOneShot(_fullClip);
            }
            _qiSlider.value = _qiSlider.maxValue;
            full = true;
        }
    }

    private void UpdateVisual()
    {
        _qiImageLow.gameObject.SetActive(_qiSlider.value < .5f);
        _qiImageMid.gameObject.SetActive((_qiSlider.value >= .5f) && !full);
        _qiImageHigh.gameObject.SetActive(full);
    }

    public void IncreaseByServing(int value)
    {
        float product = value * _pointRate;
        float sum = _qiSlider.value + product;
        if (sum < _qiSlider.maxValue)
        {
            _qiSlider.value = sum;
            full = false;
        }
        else
        {
            if (!full)
            {
                _audioSource.PlayOneShot(_fullClip);
            }
            _qiSlider.value = _qiSlider.maxValue;
            full = true;
        }
    }

    public void ResetQiMeter()
    {
        _qiSlider.value = 0;
        _qiSlider.minValue = 0;
        _qiSlider.maxValue = 1;
        full = false;
    }
}
