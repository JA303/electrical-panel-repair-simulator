using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerControlGate controlGate;
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private CharacterController characterController;
        
        [Header("Parameters")]
        [SerializeField] private float walkSpeed = 5.5f;
        [SerializeField] private float runningSpeed = 9.0f;

        [SerializeField] private float gravity = 9.8f;

        [SerializeField] private float lookSensitivity = 0.2f;
        [SerializeField] private float lookAngleLimit = 90.0f;
        
        private float currentMoveSpeed = 0f;
        private float lookAngle = 0f;

        private void Update()
        {
            if (controlGate.IsLocked)
             return;
            
            currentMoveSpeed = playerInput.RunBool ?  runningSpeed : walkSpeed;
            HandleMovement(playerInput.MoveVector);
            HandleLooking(playerInput.LookVector);
            
        }

        private void HandleMovement(Vector2 moveVector)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            
            Vector2 newSpeed = new Vector2(moveVector.y * currentMoveSpeed, moveVector.x * currentMoveSpeed);

            var moveDirection = (forward * newSpeed.x) + (right * newSpeed.y);

            if (!characterController.isGrounded)
                moveDirection.y -= gravity * Time.deltaTime;
            
            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleLooking(Vector2 rotateDelta)
        {
            lookAngle += -rotateDelta.y * lookSensitivity;
            lookAngle = Mathf.Clamp(lookAngle, -lookAngleLimit, lookAngleLimit);

            mainCameraTransform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
            transform.rotation *= Quaternion.Euler(0, rotateDelta.x * lookSensitivity, 0);
        }
    }
}
