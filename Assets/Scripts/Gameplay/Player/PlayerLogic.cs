using AsteroidsSurvival.Services;
using System;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.ServiceLocator;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay.Player
{
    public class PlayerLogic : PortalMovingLogic
    {
        #region Fields
        private PlayerController _playerController;
        
        private InputControllerService _inputController;
        
        private float _rotation = 0f;
        
        private float _radius;

        private Vector3 _movementVector;

        public Vector3 MovementVector => _movementVector;
        
        private bool _isLaserDepleted;

        private float _bulletIncreaseTimer;
        private float _bulletIncreaseInterval = 4f;
        #endregion
        
        
        
        #region Methods
        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
            
            _inputController = MyServiceLocator.Get<InputControllerService>();

            _radius = _playerController.Radius;
        }

        public void MyUpdate()
        {
            UpdateShot();
            UpdateBulletsIncrease();
            MovePlayer();
            UpdateSpeed();
            UpdateFlame();
            UpdateLaser();
            UpdateRotation();
        }

        private void UpdateBulletsIncrease()
        {
            _bulletIncreaseTimer -= Time.deltaTime;
            if (_bulletIncreaseTimer <= 0f)
            {
                _bulletIncreaseTimer = _bulletIncreaseInterval;
                _playerController.AcceptVisit();
            }
        }

        private void UpdateRotation()
        {
            float rotationFactor = 150f;
            if (_inputController.MoveValue.x > 0)
            {
                _rotation += Time.deltaTime * rotationFactor;
            }
            else if (_inputController.MoveValue.x < 0)
            {
                _rotation -= Time.deltaTime * rotationFactor;
            }

            _playerController.Rotation = _rotation;
            _playerController.ContentTransform.rotation = Quaternion.Euler(0, 180, _rotation);
        }
        
        private void UpdateLaser()
        {
            if (_inputController.LaserValue && _playerController.LaserFill > 0f && !_isLaserDepleted)
            {
                float laserSpendSpeed = 1f;
                _playerController.LaserFill -= Time.deltaTime * laserSpendSpeed;
                if (_playerController.LaserFill <= 0f)
                {
                    _isLaserDepleted = true;
                    _playerController.LaserFill = 0f;
                }
                
                if (_playerController.IsLaserActive)
                {
                    _playerController.LaserController.UpdateSparks();
                    return;
                }
                _playerController.IsLaserActive = true;
                
                // Initialize Laser
                // we add distance from center point to the nose of the ship from where the shot exits
                Vector2 bulletOffset = new Vector2();
                float laserOffset = 12f;
                bulletOffset.x = Mathf.Sin(_rotation * Mathf.Deg2Rad) * laserOffset;
                bulletOffset.y = Mathf.Cos(_rotation * Mathf.Deg2Rad) * laserOffset;

                Vector2 shipNosePosition = (Vector2)_playerController.transform.position + bulletOffset;
                
                _playerController.LaserLines.SetActive(true);
                
                _playerController.LaserController.Initialize(_rotation, shipNosePosition);
            }
            else
            {
                // switch off Laser
                _playerController.IsLaserActive = false;
                _playerController.LaserLines.SetActive(false);
                _playerController.LaserController.SwitchOff();
                if (_playerController.LaserFill > 0.2f)
                {
                    _isLaserDepleted = false;
                }
            }
            
            float laserFillSpeed = .2f;
            _playerController.LaserFill += Time.deltaTime * laserFillSpeed;
            if (_playerController.LaserFill > 1f)
            {
                _playerController.LaserFill = 1f;
            }
        }
        
        private void UpdateFlame()
        {
            bool isFlameActive = _inputController.MoveValue.y > 0;
            _playerController.FlameObject.SetActive(isFlameActive);
        }
        
        private void MovePlayer()
        {
            Vector3 tempVector = _playerController.transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime;
            tempVector.y += _movementVector.y * Time.deltaTime;

            CheckPortalMoving(ref tempVector);
            
            _playerController.MoveTo(tempVector);
        }
        
        private void UpdateShot()
        {
            if (_inputController.ShootValue)
            {
                if (_playerController.BulletsCount == 0)
                {
                    return;
                }
                _playerController.BulletsCount--;
                
                _inputController.ShootValue = false;

                // we add distance from center point to the nose of the ship from where the shot exits
                Vector2 bulletOffset = new Vector2();
                bulletOffset.x = Mathf.Sin(_rotation * Mathf.Deg2Rad) * _radius;
                bulletOffset.y = Mathf.Cos(_rotation * Mathf.Deg2Rad) * _radius;

                Vector2 shipNosePosition = (Vector2)_playerController.transform.position + bulletOffset;

                _playerController.MakeShot(_rotation, shipNosePosition);
            }
        }
        
        private void UpdateSpeed()
        {
            float accelerationFactor = 200f;
            if (_inputController.MoveValue.y > 0)
            {
                Vector3 forceVector = new Vector3(Mathf.Sin(_rotation * Mathf.Deg2Rad), Mathf.Cos(_rotation * Mathf.Deg2Rad));
                forceVector *= Time.deltaTime * accelerationFactor;
                _movementVector += forceVector;
            }
        }
        #endregion
    }
}
