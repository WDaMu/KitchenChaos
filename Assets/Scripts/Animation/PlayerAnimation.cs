using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player playerController;

    private const string IS_WALKING = "IsWalking";

    private void Update()
    {
        animator.SetBool(IS_WALKING, playerController.IsWalking());
    }
}
