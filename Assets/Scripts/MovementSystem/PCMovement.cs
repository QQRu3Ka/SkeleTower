using UnityEngine;

public class PCMovement : MonoBehaviour
{
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float yRotationLimit = 88f;

    private Vector2 _rotation = Vector2.zero;
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            Vector3 cameraForward = transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            Vector3 moveDirection = (cameraForward * vertical) + (cameraRight * horizontal);

            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }

            transform.Translate(moveDirection * 10 * Time.deltaTime, Space.World);
        }
        Rotate();
    }

    private void Rotate()
    {
        _rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        _rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -yRotationLimit, yRotationLimit);

        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
    }
}
