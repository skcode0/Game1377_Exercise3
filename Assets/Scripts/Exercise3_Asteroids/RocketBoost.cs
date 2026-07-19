using UnityEngine;

public class RocketBoost : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip boostSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = boostSound;
        audioSource.loop = true;
    }

    void Update()
    {
        EnableBoost();
    }

    public void EnableBoost()
    {
        bool isTrhusting = SpaceshipController.isThrusting;

        animator.SetBool("IsThrusting", isTrhusting);

        if (isTrhusting && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isTrhusting && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}