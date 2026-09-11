using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MoveVector { get; private set; }
        public Vector2 LookVector { get; private set; }
        public bool RunBool { get; private set; }
        
        private InputAction moveInput;
        private InputAction runInput;
        private InputAction lookInput;

        private void Start()
        {
            InitializeInputActions();
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }

        private void InitializeInputActions()
        {
            moveInput = InputSystem.actions.FindAction("Move");
            runInput = InputSystem.actions.FindAction("Sprint");
            lookInput = InputSystem.actions.FindAction("Look");
        }

        private void Update()
        {
            MoveVector = moveInput.ReadValue<Vector2>();
            LookVector = lookInput.ReadValue<Vector2>();
            RunBool = runInput.IsPressed();
        }
    }
}
