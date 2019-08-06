using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;

    public AudioClip Equip;
    public AudioClip Swap;
    public AudioClip Drop;
    public AudioClip OpenShop;
    public AudioClip CloseShop;
    public AudioClip BuyItem;
    public AudioClip SellItem;
    public AudioClip QuestComplete;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(UISound sound)
    {
        switch (sound)
        {
            case UISound.Equip: Play(Equip); break;
            case UISound.Swap: Play(Swap); break;
            case UISound.Drop: Play(Drop); break;
            case UISound.OpenShop: Play(OpenShop); break;
            case UISound.CloseShop: Play(CloseShop); break;
            case UISound.BuyItem: Play(BuyItem); break;
            case UISound.SellItem: Play(SellItem); break;
            case UISound.QuestComplete: Play(QuestComplete); break;
            default: break;
        }
    }

    private void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public enum UISound
    {
        Equip,
        Swap,
        Drop,
        OpenShop,
        CloseShop,
        BuyItem,
        SellItem,
        QuestComplete,
    }
}
