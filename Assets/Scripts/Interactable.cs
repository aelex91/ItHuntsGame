using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField]
        public string Name;

        public bool CanInteract;

        public bool IsInteracting;

        private InteractPopupUI interactPopupUI;

        protected virtual void Awake()
        {
            interactPopupUI = FindFirstObjectByType<InteractPopupUI>(FindObjectsInactive.Include);
        }

        public virtual void Interact()
        {
            IsInteracting = true;

            interactPopupUI.IsInteracting = true;

            Debug.Log("Interacting with " + Name);
        }

        public void PreInteract()
        {
            interactPopupUI.SetActive(true);
        }

        public virtual void LeaveInteraction()
        {
            interactPopupUI.IsInteracting = false;

            IsInteracting = false;

            interactPopupUI.SetActive(false);
        }
    }
}
