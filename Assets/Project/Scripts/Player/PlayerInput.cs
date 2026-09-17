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
        
        public InputAction InteractInput {get; private set;}
        public InputAction UseInput {get; private set;}
        public InputAction DropInput {get; private set;}

        private void Start()
        {
            InitializeInputActions();
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void InitializeInputActions()
        {
            moveInput = InputSystem.actions.FindAction("Move");
            runInput = InputSystem.actions.FindAction("Sprint");
            lookInput = InputSystem.actions.FindAction("Look");
            
            InteractInput = InputSystem.actions.FindAction("Interact");
            UseInput = InputSystem.actions.FindAction("Use");
            DropInput = InputSystem.actions.FindAction("Drop");
        }

        private void Update()
        {
            MoveVector = moveInput.ReadValue<Vector2>();
            LookVector = lookInput.ReadValue<Vector2>();
            RunBool = runInput.IsPressed();
        }
    }
}
