using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private Interactable currentFocus;
        private Camera currentCamera;

        public InputActionAsset inputActions;
        private InputAction interactAction;

        [HideInInspector]
        public bool IsInteracting;

        private void Start()
        {
            var actionMap = GetComponent<PlayerController>().inputActions.FindActionMap("Player"); // your map na
            interactAction = actionMap.FindAction("Interact"); // your action name
            currentCamera = Camera.main;
        }

        private void Update()
        {
            TryInteract();
        }

        private void TryInteract()
        {
            if (interactAction.IsPressed())
            {

                if (currentFocus != null)
                {
                    currentFocus.Interact();
                    IsInteracting = true;
                    return;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Interactable>(out var component) == false)
                return;

            if (component.CanInteract == false)
                return;

            currentFocus = component;

            component.PreInteract();
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<Interactable>(out var component) == false)
                return;

            currentFocus = null;

            component.LeaveInteraction();
            IsInteracting = false;
        }
    }
}
