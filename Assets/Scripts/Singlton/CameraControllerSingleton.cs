using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraControllerSingleton : MonoBehaviour
    {
        [SerializeField] private float _startRadius;
        [SerializeField] private float _endRadius;
        [SerializeField] private float _startHeight;
        [SerializeField] private float _endHeight;
        [SerializeField] private float _speed;
        
        public static CameraControllerSingleton instance;

        public float RadiusAtScale(float scale) => Mathf.Lerp(_startRadius, _endRadius, 1 - scale);
        public float HeightAtScale(float scale) => Mathf.Lerp(_startHeight, _endHeight, 1 - scale);

        public float Speed => _speed;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }
    }
}