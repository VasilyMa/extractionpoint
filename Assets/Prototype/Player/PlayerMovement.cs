using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Joystick joystick;
    private Vector3 moveInput;
    private void Update()
    {
        moveInput = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

        Vector3 localMoveDirection = transform.InverseTransformDirection(moveInput);

        animator.SetFloat("Forward", localMoveDirection.z, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", localMoveDirection.x, 0.1f, Time.deltaTime);
    }
    public Vector3 GetMoveInput()
    {
        return moveInput;
    }
}
