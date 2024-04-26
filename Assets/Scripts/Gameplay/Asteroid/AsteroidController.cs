using AsteroidsSurvival.Gameplay;
using AsteroidsSurvival.Gameplay.Player;
using UnityEngine;

namespace AsteroidsSurvival.View.Gameplay.Enemy
{
    public class AsteroidController : PortalMovingBase, IEnemy
    {
        #region Fields and properties
        [SerializeField] private float _radius = 5f;
        [SerializeField] private Transform _content;
        
        private Vector3 _movementVector = new();
        private float _speedFactor = 50f;

        private Vector3 _rotationVector;

        public bool IsDestroyed { get; set; }
        
        public float Radius => _radius;
        
        public Transform Transform => transform;

        #endregion
        
        
        
        #region Methods
        
        public void Initialize()
        {
            // place Asteroid on a random position
            Vector2 position = new();
            position.x = Random.Range(0f, Screen.width);
            position.y = Random.Range(0f, Screen.height);
            
            transform.position = position;
            
            // Start flying in a random direction
            float direction = Random.Range(0f, 360f);
            _movementVector.x = Mathf.Sin(direction * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(direction * Mathf.Deg2Rad);

            float rotationSpeed = Random.Range(-50f, 50f);
            _rotationVector = new Vector3(0f, 0f, rotationSpeed);
        }

        private void UpdatePosition()
        {
            Vector3 tempVector = transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime * _speedFactor;
            tempVector.y += _movementVector.y * Time.deltaTime * _speedFactor;

            MoveTo(tempVector);
        }

        private void UpdateRotation()
        {
            _content.Rotate(_rotationVector * Time.deltaTime);
        }
        #endregion
        
        
        
        #region IEnemy implementation
        
        public void UpdateEnemy()
        {
            UpdatePosition();

            UpdateRotation();
        }
        
        #endregion
    }
}
