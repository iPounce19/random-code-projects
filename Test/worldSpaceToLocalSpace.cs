using UnityEngine;

public class worldSpaceToLocalSpace : MonoBehaviour
{
    /// <summary>
    /// To make sure that the animator can convert the Input.GetAxisRaw to local space direction, based on the rotation of the model.
    /// </summary>
    public Animator animator;

    void Update()
    {
        // Get input
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(inputX, 0f, inputY);

        // Convert to local space
        Vector3 localDirection = transform.InverseTransformDirection(inputDirection);

        // Normalize for consistent speed
        if (localDirection.magnitude > 1)
            localDirection.Normalize();

        // Update Animator
        animator.SetFloat("horizontalInput", localDirection.x);
        animator.SetFloat("verticalInput", localDirection.z);
    }
}
