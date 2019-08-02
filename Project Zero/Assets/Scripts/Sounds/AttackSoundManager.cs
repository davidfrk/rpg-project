using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> attackSounds;
    private UnitController unitController;

    void Awake()
    {
        unitController = GetComponent<UnitController>();
    }

    public void AttackEndEvent()
    {
        if (attackSounds.Count != 0 && unitController.unit.alive)
        {
            int audioIndex = Random.Range(0, attackSounds.Count);

            audioSource.clip = attackSounds[audioIndex];
            audioSource.Play();
        }
    }
}
