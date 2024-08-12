using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("�⺻ ����� �ҽ�")]
    [SerializeField] private AudioSource audioSource;
    [Header("����� �ͼ�")]
    [SerializeField] private AudioMixer audioMixer;

    private GameObject effectObject;

    private List<AudioClip> effectClips = new();

    private Slider masterSlider;

    

    #region Singleton
    public static AudioManager instance;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start()
    {
        AudioSetting();
    }

    private void AudioSetting()
    {
        effectObject = new GameObject($"Effect Sounds");
        effectObject.transform.SetParent(transform);
    }

    #region Utility

    public void BgmPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        Play();
    }

    public void EffectPlay(AudioClip clip)
    {
        var dummySource = effectObject.AddComponent<AudioSource>();
        dummySource.clip = clip;
        effectClips.Add(clip);
        dummySource.Play();

        StartCoroutine(DestroyDummySource(dummySource, clip));
    }

    private IEnumerator DestroyDummySource(AudioSource dummuySource, AudioClip clip)
    {
        yield return new WaitUntil(() => !dummuySource.isPlaying);
        effectClips.Remove(clip);
        Destroy(dummuySource);
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
    }

    #endregion

    // TODO : SliderController�� �߰��Ͽ� �ش� Ŭ������ �����̴��� �Ҵ�
    #region Volume Setting

    public Slider SetMasterSlider(Slider slider) => masterSlider = slider;

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20f);
    }

    #endregion
}
