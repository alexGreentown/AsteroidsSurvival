using System;
using AsteroidsSurvival.Gameplay.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AsteroidsSurvival.Gameplay.Enemy
{
    public class UFOController : PortalMovingBase, IEnemy
    {
        #region Events
        public event Action<float, Vector2> OnMakeShot;
        #endregion
        
        
        
        #region Fields
        [SerializeField] private float _radius = 5f;
        [SerializeField] private Transform _content;
        
        private Vector3 _movementVector;
        private float _speedFactor = 25f;

        private PlayerController _playerController;
        private float _angleToPlayer;

        private float _shootingInterval = 5f;
        private float _shootingTimer;

        public bool IsDestroyed { get; set; }
        public bool IsDivided { get; set; }

        public float Radius => _radius;
        public Transform Transform => transform;
        #endregion
        
        
        
        #region Methods
        
        public void Initialize(PlayerController player)
        {
            _playerController = player;

            IsDestroyed = false;
            
            // Start flying in a random direction
            float rotation = Random.Range(0f, 360f);
            _movementVector.x = Mathf.Sin(rotation * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(rotation * Mathf.Deg2Rad);
            
            // reset shooting timer
            _shootingTimer = 0f;
        }

        private void UpdatePosition()
        {
            _movementVector.x = Mathf.Sin(_angleToPlayer * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(_angleToPlayer * Mathf.Deg2Rad);
            
            Vector3 tempVector = transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime * _speedFactor;
            tempVector.y += _movementVector.y * Time.deltaTime * _speedFactor;

            MoveTo(tempVector);
        }

        private void UpdateAim()
        {
            Vector3 relative = _playerController.transform.position - transform.position;
            _angleToPlayer = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            
            _content.localEulerAngles = new Vector3(0f, 180f, _angleToPlayer);
        }
        
        private void UpdateShot()
        {
            _shootingTimer += Time.deltaTime;
            
            if (_shootingTimer >= _shootingInterval)
            {
                _shootingTimer = 0f;

                // we add distance from center point to the nose of the ship from where the shot exits
                Vector2 bulletOffset = new Vector2();
                bulletOffset.x = Mathf.Sin(_angleToPlayer * Mathf.Deg2Rad) * Radius;
                bulletOffset.y = Mathf.Cos(_angleToPlayer * Mathf.Deg2Rad) * Radius;

                Vector2 shipNosePosition = (Vector2)transform.position + bulletOffset;
                
                if (OnMakeShot == null)
                {
                    throw new NotImplementedException("OnMakeShot event is missing");
                }
                OnMakeShot.Invoke(_angleToPlayer, shipNosePosition);
            }
        }
        #endregion



        #region IEnemy implementation
        public void UpdateEnemy()
        {
            if (IsDestroyed)
            {
                return;
            }
            
            UpdatePosition();
            
            UpdateAim();

            UpdateShot();
        }
        #endregion

    }
}
