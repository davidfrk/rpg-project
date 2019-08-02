using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackSoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public List<AudioClip> attackSounds;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AttackEndEvent()
    {
        if (attackSounds.Count != 0)
        {
            int audioIndex = Random.Range(0, attackSounds.Count);

            audioSource.clip = attackSounds[audioIndex];
            audioSource.Play();
        }
    }
}
