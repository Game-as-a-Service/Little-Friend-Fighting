using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    public int knockBackForce;
    [SerializeField] private  AudioSource _audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var animator = other.GetComponent<Animator>();
        animator.SetTrigger("damaged");
        var audioClip = _audioManager.GetAudioClip("OnDamage");
        _audioSource.PlayOneShot(audioClip);
        // _audioManager.PlaySoundFromTo(12f, 12.095192743764173f);
        other.SendMessage("OnDamage",5f);
    }
}
