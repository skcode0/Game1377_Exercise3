using UnityEngine;

public class RocketBoost : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        EnableBoostAnimation();
    }

    public void EnableBoostAnimation()
    {
        animator.SetBool("IsThrusting", SpaceshipController.isThrusting);
    }
}