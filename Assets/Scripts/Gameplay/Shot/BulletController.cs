using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsSurvival.Gameplay.Shot
{
    public enum BulletType
    {
        PLAYER,
        ENEMY
    }
    
    public class BulletController : MonoBehaviour, IMoving
    {
        #region Events
        public event Action<BulletController> OnBulletDestroy;
        #endregion
        
        
        
        #region Fields and properties
        [SerializeField]private Image _bulletImage;
        
        private Vector3 _movementVector = new();
        private float _flightDistanceCounter;
        
        private float _speedFactor = 200f;
        private float _flightDistance = 300f;
        

        public BulletType BulletType{ get; set; }
        #endregion
        
        
        
        #region Methods
        public void Initialize(float rotation, Vector2 position)
        {
            _flightDistanceCounter = _flightDistance;

            MoveTo(position);
            
            transform.rotation = Quaternion.Euler(0f, 0f, -rotation);
            
            _movementVector.x = Mathf.Sin(rotation * Mathf.Deg2Rad);
            _movementVector.y = Mathf.Cos(rotation * Mathf.Deg2Rad);
            _movementVector.z = 0;
        }

        public void SetSprite(Sprite bulletSprite)
        {
            _bulletImage.sprite  = bulletSprite;
        }
        
        public void UpdateBullet()
        {
            Vector3 tempVector = transform.position;
            tempVector.x += _movementVector.x * Time.deltaTime * _speedFactor;
            tempVector.y += _movementVector.y * Time.deltaTime * _speedFactor;

            MoveTo(tempVector);
            
            _flightDistanceCounter -= _movementVector.magnitude;
            if (_flightDistanceCounter < 0f)
            {
                DestroyBullet();
            }
        }

        public void DestroyBullet()
        {
            if (OnBulletDestroy == null)
            {
                throw new NotImplementedException("OnBulletDestroy event is missing");
            }
            OnBulletDestroy(this);
        }
        #endregion
        
        
        
        #region IMoving implementation
        public void MoveTo(Vector3 targetPosition)
        {            
            transform.position = targetPosition;
        }
        #endregion
    }
}
