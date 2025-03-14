using TMPro;
using UnityEngine;

public class DogCompanion : MonoBehaviour
{
    public Transform player;        // Reference to the player's position
    public Transform targetPosition; // The fixed point near the player the dog should aim to reach
    public Animator animator;        // Animator controlling the dog's animations
    public float movementDamping = 0.1f; // Damping for acceleration and deceleration
    public float playerDogDistance = 2;  // Damping for acceleration and deceleration
    public float sprintMultiplier = 1.0f;
    public float dampingAroundPlayer;
    private float currentMovementValue = 0f; // Current value of Movement parameter (0 = idle, 0.5 = walk, 1 = sprint)
    private float desiredMovementValue = 0f; // Target Movement value based on distance from target
    private void Start()
    {
        targetPosition = new GameObject("Dog Target").transform;
        targetPosition.parent = player;
        targetPosition.position = player.position + player.right * playerDogDistance;
        targetPosition.position = new Vector3(targetPosition.position.x, transform.position.y, targetPosition.position.z);
    }
    private void Update()
    {
        // Calculate distance to target position
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < dampingAroundPlayer) return;
        // Set desiredMovementValue based on distance thresholds
        if (distanceToTarget > 5f) // For longer distances, sprint
        {
            // Adjust sprintMultiplier based on distance
            float distanceCoefficient = Mathf.Clamp(distanceToTarget / 10f, 1f, 1.5f); // Limits the coefficient between 1 and 1.5
            desiredMovementValue = 1f * distanceCoefficient * sprintMultiplier;
        }
        else if (distanceToTarget > 2f) // For medium distances, walk
        {
            desiredMovementValue = 0.5f;
        }
        else // Close to target, idle
        {
            desiredMovementValue = 0f;
        }

        // Smoothly interpolate current movement value towards the desired value
        currentMovementValue = Mathf.Lerp(currentMovementValue, desiredMovementValue, movementDamping * Time.deltaTime);
        
        // Update the animator's Movement parameter
        animator.SetFloat("Movement_f", currentMovementValue);

        if (desiredMovementValue <= 0)
            return;
        // Rotate smoothly to face the target position
        Vector3 direction = (targetPosition.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, movementDamping * Time.deltaTime);
    }
}
