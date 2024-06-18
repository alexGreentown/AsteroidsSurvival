using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.View.Gameplay.Asteroid
{
    public class AsteroidLogic : ILogic
    {
        #region Fields
        private Vector3 _movementVector = new();
        private float _speedFactor = 50f;

        private Vector3 _rotationVector;

        private AsteroidController _asteroidController;
        #endregion
        
        
        
        #region Methods
        public void Initialize(AsteroidController asteroidController, float? directionAngle = null)
        {
            _asteroidController = asteroidController;
            
            _speedFactor = _asteroidController.AsteroidStrategy.SpeedFactor;
            
            float rotationSpeed = Random.Range(-50f, 50f);
            _rotationVector = new Vector3(0f, 0f, rotationSpeed);
            
            float randomDirection = Random.Range(0f, 360f);
            float movementAngle = directionAngle ?? randomDirection;
            _movementVector.x = Mathf.Sin(movementAngle * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(movementAngle * Mathf.Deg2Rad);
        }
        
        public void MyUpdate()
        {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            Vector3 tempVector = _asteroidController.transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime * _speedFactor;
            tempVector.y += _movementVector.y * Time.deltaTime * _speedFactor;

            _asteroidController.MoveTo(tempVector);
        }

        private void UpdateRotation()
        {
            _asteroidController.ContentTransform.Rotate(_rotationVector * Time.deltaTime);
        }
        #endregion
    }
}
