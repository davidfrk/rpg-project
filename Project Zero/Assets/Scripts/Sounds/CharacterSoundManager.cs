using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    public AudioSource attackAudioSource;
    public List<AudioClip> attackSounds;
    public AudioClip attackCritSound;
    public AudioSource movementAudioSource;
    public float movementSpeed;
    public AudioSource speechAudioSource;

    private UnitController unitController;
    private MovementController movementController;

    void Awake()
    {
        unitController = GetComponent<UnitController>();
        movementController = GetComponent<MovementController>();

        unitController.OnDeathCallback += OnDeath;
        unitController.OnCritCallback += OnCrit;
    }

    public void AttackEndEvent()
    {
        if (attackAudioSource != null)
        {
            if (attackSounds.Count != 0 && unitController.unit.alive)
            {
                int audioIndex = Random.Range(0, attackSounds.Count);

                attackAudioSource.PlayOneShot(attackSounds[audioIndex]);
            }
        }
    }

    void OnDeath(Unit killer)
    {
        if (speechAudioSource != null)
        {
            speechAudioSource.Play();
        }
    }

    void OnCrit(float damage)
    {
        if (attackAudioSource != null)
        {
            attackAudioSource.PlayOneShot(attackCritSound);
        }
    }
    
    void Update()
    {
        if (movementAudioSource != null)
        {
            if (movementController.MovementState == MovementState.Running)
            {
                if (!movementAudioSource.isPlaying)
                {
                    movementAudioSource.Play();
                }
            }
            else
            {
                if (movementAudioSource.isPlaying)
                {
                    movementAudioSource.Stop();
                }
            }
        }
    }

    public void Play(AudioClip clip)
    {
        attackAudioSource.PlayOneShot(clip);
    }
}
