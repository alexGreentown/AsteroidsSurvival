using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay.Shot
{
    public class BulletLogic : PortalMovingLogic
    {
        #region Fields
        private float _speedFactor = 200f;
        private float _flightDistance = 300f;
        
        private float _flightDistanceCounter;
        
        private BulletController _bulletController;

        private Vector3 _movementVector = new();
        #endregion



        #region Methods
        
        public void Initialize(BulletController bulletController, float rotation)
        {
            _bulletController = bulletController;
            
            _flightDistanceCounter = _flightDistance;
            
            _movementVector.x = Mathf.Sin(rotation * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(rotation * Mathf.Deg2Rad);
            _movementVector.z = 0;
        }
        
        public void MyUpdate()
        {
            Vector3 tempVector = _bulletController.transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime * _speedFactor;
            tempVector.y += _movementVector.y * Time.deltaTime * _speedFactor;
            
            CheckPortalMoving(ref tempVector);
            
            _bulletController.MoveTo(tempVector);
            
            _flightDistanceCounter -= _movementVector.magnitude;
            if (_flightDistanceCounter < 0f)
            {
                _bulletController.DestroyBullet();
            }
        }

        #endregion
    }
}
