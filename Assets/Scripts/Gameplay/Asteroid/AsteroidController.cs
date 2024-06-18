using AsteroidsSurvival.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsSurvival.View.Gameplay.Asteroid
{
    public class AsteroidController : MonoBehaviour, IMoving, IEnemy
    {
        #region Fields and properties
        [SerializeField] private float _radius = 5f;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private Image _graphics;
        
        public float Radius => _radius;
        
        public Transform Transform => transform;
        
        private AsteroidLogic _asteroidLogic = new();
        
        public bool IsDivided { get; set; }
        
        private IAsteroidStrategy _asteroidStrategy = new AsteroidStrategyBig();
        public IAsteroidStrategy AsteroidStrategy
        {
            get => _asteroidStrategy;
            set => _asteroidStrategy = value;
        }

        public Transform ContentTransform
        {
            get => _contentTransform;
        }
        #endregion
        
        
        
        #region Methods
        public void Initialize(float? directionAngle = null)
        {
            _asteroidLogic.Initialize(this, directionAngle);
            _radius = _asteroidStrategy.Radius;
            
            _asteroidStrategy.SetGraphics(_graphics);
        }
        #endregion
        
        
        
        #region IMoving implementation
        public void MoveTo(Vector3 targetPosition)
        {            
            transform.position = targetPosition;
        }
        #endregion
        
        
        
        #region IEnemy implementation
        public void UpdateEnemy()
        {
            _asteroidLogic.MyUpdate();
        }
        #endregion
    }
}
