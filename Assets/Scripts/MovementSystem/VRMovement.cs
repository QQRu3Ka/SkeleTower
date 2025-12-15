using UnityEngine;
using UnityEngine.InputSystem;

public class VRMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float crouchSpeed = 0.75f;
    public float gravity = -9.81f;

    public float speedSmoothTime = 0.1f;
    public float maxSlopeAngle = 45f;

    public InputActionProperty moveAction;
    public InputActionProperty runAction;
    public Transform cameraTransform;
    public Transform leftController;
    public Transform rightController;

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float currentSpeed;
    private float speedSmoothVelocity;
    private bool isGrounded;
    private bool isRunning;
    private bool isCrouching;


}
