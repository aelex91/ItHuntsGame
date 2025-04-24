using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private Transform[] SpawnPositions;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

        }

        public Transform[] GetSpawnPositions()
        {
            Debug.Log("getting stuff");
            if (SpawnPositions == null)
            {
                var gameObjects = GameObject.FindGameObjectsWithTag("SpawnPosition");

                SpawnPositions = gameObjects
                    .Select(x => x.transform)
                    .ToArray();
            } 

            Debug.Log("do we have spawn positions?" + SpawnPositions.Any());

            return SpawnPositions;
        }
    }
}
