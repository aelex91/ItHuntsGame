using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private Interactable currentFocus;
        private Camera currentCamera;

        private InputActionAsset inputActions;
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
                Ray ray = currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 3f)) // 3f = interaction distance
                {
                    if (hit.collider.TryGetComponent(out Interactable interactable))
                    {
                        if (currentFocus != null)
                        {
                            currentFocus.Interact();
                            IsInteracting = true;
                            return;
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.parent.TryGetComponent<Interactable>(out var component) == false)
                return;

            if (component.IsInteracting)
                return;

            currentFocus = component;
            component.PreInteract();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.transform.parent.TryGetComponent<Interactable>(out var component) == false)
                return;

            if (currentFocus == component)
            {
                currentFocus = null;
                component.LeaveInteraction();
                IsInteracting = false;
            }
        }
    }
}
