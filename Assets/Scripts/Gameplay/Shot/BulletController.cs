using System;
using AsteroidsSurvival.Interfaces;
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
        
        private BulletLogic _bulletLogic = new();

        public BulletType BulletType{ get; set; }
        #endregion
        
        
        
        #region Methods
        public void Initialize(float rotation, Vector2 position)
        {
            _bulletLogic.Initialize(this, rotation);

            MoveTo(position);
            
            transform.rotation = Quaternion.Euler(0f, 0f, -rotation);
        }

        public void SetSprite(Sprite bulletSprite)
        {
            _bulletImage.sprite  = bulletSprite;
        }
        
        public void UpdateBullet()
        {
            _bulletLogic.MyUpdate();
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
