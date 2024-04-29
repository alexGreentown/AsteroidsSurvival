using AsteroidsSurvival.Gameplay;
using AsteroidsSurvival.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsSurvival.View.Gameplay.Asteroid
{
    public class AsteroidController : PortalMovingBase, IEnemy
    {
        #region Fields and properties
        [SerializeField] private float _radius = 5f;
        [SerializeField] private Transform _content;
        [SerializeField] private Image _graphics;
        
        private Vector3 _movementVector = new();
        private float _speedFactor = 50f;

        private Vector3 _rotationVector;
        
        public float Radius => _radius;
        
        public Transform Transform => transform;
        
        public bool IsDivided { get; set; }
        
        private IAsteroidStrategy _asteroidStrategy = new AsteroidStrategyBig();
        public IAsteroidStrategy AsteroidStrategy
        {
            get => _asteroidStrategy;
            set => _asteroidStrategy = value;
        }

        #endregion
        
        
        
        #region Methods
        
        public void Initialize(float? directionAngle = null)
        {
            _radius = AsteroidStrategy.Radius;
            _speedFactor = AsteroidStrategy.SpeedFactor;
            AsteroidStrategy.SetGraphics(_graphics);

            float randomDirection = Random.Range(0f, 360f);
            float movementAngle = directionAngle ?? randomDirection;
            _movementVector.x = Mathf.Sin(movementAngle * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(movementAngle * Mathf.Deg2Rad);

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
