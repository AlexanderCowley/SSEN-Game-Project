using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChoppableItems : Item
{
    [SerializeField] private GameObject _uncutModel;
    [SerializeField] private GameObject _halfCutModel;
    [SerializeField] private GameObject _fullCutModel;
    [SerializeField] private int _halfCutThreshold;
    [SerializeField] private int _fullCutThreshold;
    [SerializeField] private GameObject _chopTextCanvas;
    [SerializeField] private TextMeshProUGUI _chopText;
    [SerializeField] private float _helperUIDistance;
    private ParticleSystem _particles;
    private GameObject _player;

    private void Start()
    {
        _player = GameplayController.Instance.PlayerObject;
        _particles = GetComponent<ParticleSystem>();
        _particles.Stop();
    }

    private void Update()
    {
        ChangeModel();
        EmitPerfectParticles();
        UpdateChopText();
    }

    private void ChangeModel()
    {
        if (!PerfectChops)
        {
            if ((ChopValue < _halfCutThreshold))
            {
                _uncutModel.SetActive(true);
                _halfCutModel.SetActive(false);
                _fullCutModel.SetActive(false);
            }
            else if ((ChopValue >= _halfCutThreshold) && (ChopValue < _fullCutThreshold))
            {
                _uncutModel.SetActive(false);
                _halfCutModel.SetActive(true);
                _fullCutModel.SetActive(false);
            }
            else if (ChopValue >= _fullCutThreshold)
            {
                _uncutModel.SetActive(false);
                _halfCutModel.SetActive(false);
                _fullCutModel.SetActive(true);
            }
        }
        else
        {
            _uncutModel.SetActive(false);
            _halfCutModel.SetActive(false);
            _fullCutModel.SetActive(true);
        }
    }

    private void EmitPerfectParticles()
    {
        if (PerfectChops)
            if (_particles.isStopped)
                _particles.Play();
    }

    private void UpdateChopText()
    {
        float distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        if (distance < _helperUIDistance)
        {
            _chopTextCanvas.gameObject.SetActive(true);
        }
        else
        {
            _chopTextCanvas.gameObject.SetActive(false);
        }

        _chopText.text = PerfectChops ? "PERFECT" : ChopValue.ToString();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(0, 0.5f, 0), $"ChopValue = {ChopValue} \nPerfect Chops: {PerfectChops}");
    }
#endif
}
