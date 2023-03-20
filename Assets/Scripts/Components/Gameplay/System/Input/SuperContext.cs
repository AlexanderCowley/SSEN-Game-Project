using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperContext : MonoBehaviour
{
    [SerializeField] private AudioClip _bladeDanceClip, _dragonFlameClip;
    [SerializeField] private Animator _bladeDanceSplash, _dragonFlameSplash;
    private AudioSource _audioSource;
    private QiMeter _qiMeter;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _qiMeter = GetComponent<QiMeter>();
    }

    public void Super()
    {
        if (_qiMeter.full)
        {
            switch(GameplayController.Instance.ForbiddenTechniqueID)
            {
                case 2:
                    Debug.Log("Dragon Flame unleashed!");
                    DragonFlame();
                    _qiMeter.ResetQiMeter();
                    break;
                case 1:
                    Debug.Log("Blade Dance unleashed!");
                    BladeDance();
                    _qiMeter.ResetQiMeter();
                    break;
                case 0:
                    Debug.Log("No Forbidden Technique selected!");
                    break;
                default:
                    Debug.Log("Something went really wrong with Forbidden Techniques!");
                    break;
            }
            GameplayController.Instance.UsedSuper();
        }
    }

    private void BladeDance()
    {
        foreach (Item i in GameplayController.Instance.AllIngredients)
        {
            if (i.gameObject.activeInHierarchy)
            {
                i.ForcePerfectChops();
            }
        }
        _audioSource.PlayOneShot(_bladeDanceClip);
        _bladeDanceSplash.Play("BladeDanceExecute");
    }

    private void DragonFlame()
    {
        foreach (StoveTopStation s in GameplayController.Instance.AllStoves)
        {
            s.EnablePerfectHeating();
        }
        _audioSource.PlayOneShot(_dragonFlameClip);
        _dragonFlameSplash.Play("DragonFlameExecute");
    }


}
