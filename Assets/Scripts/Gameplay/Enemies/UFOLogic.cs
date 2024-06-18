using UnityEngine;

namespace AsteroidsSurvival.Gameplay.Enemy
{
    public class UFOLogic : PortalMovingLogic
    {
        #region Fields

        private UFOController _UFOController;

        private Vector3 _movementVector;
        private float _speedFactor = 25f;

        private float _shootingInterval = 5f;
        private float _shootingTimer;
        
        private float _angleToPlayer;
        #endregion



        #region Methods

        public void Initialize(UFOController controller)
        {
            _UFOController = controller;
            
            // Start flying in a random direction
            float rotation = Random.Range(0f, 360f);
            _movementVector.x = Mathf.Sin(rotation * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(rotation * Mathf.Deg2Rad);
            
            // reset shooting timer
            _shootingTimer = 0f;
        }

        public void MyUpdate()
        {
            UpdatePosition();
            
            UpdateAim();

            UpdateShot();
        }

        private void UpdatePosition()
        {
            _movementVector.x = Mathf.Sin(_angleToPlayer * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(_angleToPlayer * Mathf.Deg2Rad);
            
            Vector3 tempVector = _UFOController.transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime * _speedFactor;
            tempVector.y += _movementVector.y * Time.deltaTime * _speedFactor;

            _UFOController.MoveTo(tempVector);
        }

        private void UpdateAim()
        {
            Vector3 relative = _UFOController.PlayerController.transform.position - _UFOController.transform.position;
            _angleToPlayer = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            
            _UFOController.Content.localEulerAngles = new Vector3(0f, 180f, _angleToPlayer);
        }
        
        private void UpdateShot()
        {
            _shootingTimer += Time.deltaTime;
            
            if (_shootingTimer >= _shootingInterval)
            {
                _shootingTimer = 0f;

                // we add distance from center point to the nose of the ship from where the shot exits
                Vector2 bulletOffset = new Vector2();
                bulletOffset.x = Mathf.Sin(_angleToPlayer * Mathf.Deg2Rad) * _UFOController.Radius;
                bulletOffset.y = Mathf.Cos(_angleToPlayer * Mathf.Deg2Rad) * _UFOController.Radius;

                Vector2 shipNosePosition = (Vector2)_UFOController.transform.position + bulletOffset;
                
                _UFOController.MakeShot(_angleToPlayer, shipNosePosition);
            }
        }
        #endregion
    }
}