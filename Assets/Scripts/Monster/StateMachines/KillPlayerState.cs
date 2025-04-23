using Assets.Scripts.StateMachines;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Monster.StateMachines
{
    public class KillPlayerState : EnemyState
    {
        public KillPlayerState(EnemyAI enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            var player = enemy.Player;
            var cam = player.GetComponentInChildren<Camera>();
            if (cam == null) throw new NullReferenceException(nameof(Camera));

            // 1. Stop player movement
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
                playerController.enabled = false;

            // 2. Teleport enemy in front of player
            Vector3 forward = player.forward;
            Vector3 faceToFacePos = player.position + forward * 1.0f - Vector3.up * 0.5f; // adjust offset as needed
            enemy.transform.position = faceToFacePos;
            enemy.transform.rotation = Quaternion.LookRotation(-forward);

            // 3. Zoom camera in (can use a coroutine for smooth transition)
            enemy.StartCoroutine(ZoomCameraIn(cam, 30f, 1.0f)); // 30 FOV over 1 sec

            // 4. Trigger animation / death scene
            enemy.Animator.SetTrigger("Kill");

            // 5. After delay → load end scene
            enemy.StartCoroutine(LoadEndSceneAfterDelay(3f));
        }

        IEnumerator ZoomCameraIn(Camera cam, float targetFOV, float duration)
        {
            float startFOV = cam.fieldOfView;
            float time = 0;

            while (time < duration)
            {
                cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            cam.fieldOfView = targetFOV;
        }

        IEnumerator LoadEndSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            //SceneManager.LoadScene("GameOver"); // Replace with your end scene
        }



    }
}
