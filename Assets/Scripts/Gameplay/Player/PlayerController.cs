using AsteroidsSurvival.Services;
using System;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay.Player
{
    public class PlayerController : PortalMovingBase
    {
        #region Events
        public event Action<float, Vector2> OnMakeShot;
        public event Action OnPlayerKilled;
        #endregion
        
        
        
        #region Fields and Properties
        private InputControllerService _inputController;

        private float _rotation = 0f;

        private float _laserFill = 0f;

        private Vector3 _movementVector = new ();

        [SerializeField] private Transform _contentTransform;
        [SerializeField] private GameObject _flameObject;

        public InputControllerService InputController
        {
            set => _inputController = value;
        }

        public int EnemiesKilled { get; set; }

        #endregion
        
        
        
        #region Methods

        public void Initialize()
        {
            EnemiesKilled = 0;
        }
        
        public void UpdatePlayer()
        {
            UpdateSpeed();
            UpdateFlame();
            UpdateRotation();
            UpdateShot();
            MovePlayer();
        }

        private void UpdateShot()
        {
            if (_inputController.ShootValue || _inputController.LaserValue)
            {
                _inputController.ShootValue = false;
                _inputController.LaserValue = false;

                // we add distance from center point to the nose of the ship from where the shot exits
                float shipLength = 15f;
                Vector2 bulletOffset = new Vector2();
                bulletOffset.x = Mathf.Sin(_rotation * Mathf.Deg2Rad) * shipLength;
                bulletOffset.y = Mathf.Cos(_rotation * Mathf.Deg2Rad) * shipLength;

                Vector2 shipNosePosition = (Vector2)transform.position + bulletOffset;
                
                OnMakeShot(_rotation, shipNosePosition);
            }
        }
        
        private void UpdateLaser()
        {
            if (_inputController.LaserValue)
            {
                MakeLaser();
            }
            
            void MakeLaser()
            {
                
            }
        }

        private void UpdateSpeed()
        {
            float accelerationFactor = 100f;
            if (_inputController.MoveValue.y > 0)
            {
                Vector3 forceVector = new Vector3(Mathf.Sin(_rotation * Mathf.Deg2Rad), Mathf.Cos(_rotation * Mathf.Deg2Rad));
                forceVector *= Time.deltaTime * accelerationFactor;
                _movementVector += forceVector;
            }
        }

        private void UpdateFlame()
        {
            bool isFlameActive = _inputController.MoveValue.y > 0;
            _flameObject.SetActive(isFlameActive);
        }
        
        private void UpdateRotation()
        {
            float rotationFactor = 80f;
            if (_inputController.MoveValue.x > 0)
            {
                _rotation += Time.deltaTime * rotationFactor;
            }
            else if (_inputController.MoveValue.x < 0)
            {
                _rotation -= Time.deltaTime * rotationFactor;
            }  
            
            _contentTransform.rotation = Quaternion.Euler(0, 180, _rotation);
        }

        private void MovePlayer()
        {
            Vector3 tempVector = transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime;
            tempVector.y += _movementVector.y * Time.deltaTime;
            
            MoveTo(tempVector);
        }

        public void GetPlayerData(in PlayerDataSet playerDataSet)
        {
            playerDataSet.Angle = _rotation % 360f;
            playerDataSet.Coordinates.x = transform.position.x;
            playerDataSet.Coordinates.y = transform.position.y;
            playerDataSet.Speed = _movementVector.magnitude;
            playerDataSet.LaserCount = 22;
            playerDataSet.LaserFill = 0.5f;
            playerDataSet.Score = EnemiesKilled;
        }

        public void PlayerKilled()
        {
            OnPlayerKilled();
        }
        
        #endregion
    }
}
