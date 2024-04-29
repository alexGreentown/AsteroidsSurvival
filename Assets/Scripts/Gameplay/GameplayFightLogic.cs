using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class GameplayFightLogic : ILogic, IVisitor
    {
        #region Fields
        private FightField _fightField;
        private Transform _playerTransform;
        
        public Vector2[] LaserRectangle = new Vector2[4];
        #endregion



        #region Methods
        public void Initialize(FightField fightField)
        {
            _fightField = fightField;
        }

        public void SetPlayerTransform()
        {
            _playerTransform = _fightField.PlayerController.transform;
        }
        
        public void MyUpdate()
        {
            _fightField.PlayerController.UpdatePlayer();

            if (_fightField.PlayerController.IsLaserActive)
            {
                CheckPlayerLaserCollisions();
            }
            
            foreach (var bullet in _fightField.BulletsList)
            {
                bullet.UpdateBullet();

                if (bullet.BulletType == BulletType.ENEMY)
                {
                    CheckBulletCollisionWithPlayer(bullet);
                }

                if (bullet.BulletType == BulletType.PLAYER)
                {
                    CheckBulletCollisionWithEnemies(bullet);
                }
            }

            CheckPlayerCollisionWithEnemies();

            foreach (IEnemy enemy in _fightField.EnemiesList)
            {
                enemy.UpdateEnemy();
            }
        }

        private void CheckPlayerLaserCollisions()
        {
            Transform[] LaserRectanglePoints = _fightField.PlayerController.LaserController.LaserRectanglePoints; 
            for (var i = 0; i < LaserRectanglePoints.Length; i++) 
            { 
                LaserRectangle[i] = LaserRectanglePoints[i].position;
            }
            
            foreach (IEnemy enemy in _fightField.EnemiesList)
            {
                if (IsPointInside(LaserRectangle, enemy))
                {
                    _fightField.EnemyKilled(enemy);
                }
            }
        }
        
        public bool IsPointInside(Vector2[] rectanglePoints, IEnemy enemy)
        {
            bool isInside = false;
            
            // calculate if enemy points are inside LaserRectangle
            for (int k = 0; k < 8; k++)
            {
                float rotation = k * 45f;
                Vector2 offsetVector = new();
                offsetVector.x = Mathf.Sin(rotation * Mathf.Deg2Rad) * enemy.Radius;
                offsetVector.y = Mathf.Cos(rotation * Mathf.Deg2Rad) * enemy.Radius;
                
                Vector2 enemyPoint = (Vector2)enemy.Transform.position + offsetVector;
                
                // Calculating point intersections with LaserRectangle
                for (int i = 0, j = 3; i < 4; j = i++)
                {
                    if (((rectanglePoints[i].y > enemyPoint.y) != (rectanglePoints[j].y > enemyPoint.y)) &&
                        (enemyPoint.x < (rectanglePoints[j].x - rectanglePoints[i].x) * (enemyPoint.y - rectanglePoints[i].y) / (rectanglePoints[j].y - rectanglePoints[i].y) + rectanglePoints[i].x))
                    {
                        isInside = !isInside;
                    }
                }
                
                if(isInside)
                {
                    return true;
                }
            }

            return isInside;
        }

        private void CheckBulletCollisionWithPlayer(BulletController bullet)
        {
            float distanceToPlayer = (bullet.transform.position - _playerTransform.position).magnitude;
            if (distanceToPlayer < _fightField.PlayerController.Radius)
            {
                _fightField.PlayerController.PlayerKilled();
            }
        }
        
        private void CheckBulletCollisionWithEnemies(BulletController bullet)
        {
            foreach (IEnemy enemy in _fightField.EnemiesList)
            {
                float distanceToEnemy = (bullet.transform.position - enemy.Transform.position).magnitude;
                if (distanceToEnemy < enemy.Radius)
                {
                    _fightField.EnemyKilled(enemy);
                    bullet.DestroyBullet();
                }
            }
        }
        
        private void CheckPlayerCollisionWithEnemies()
        {
            foreach (IEnemy enemy in _fightField.EnemiesList)
            {
                float distanceToEnemy = (_playerTransform.position - enemy.Transform.position).magnitude;
                if (distanceToEnemy < enemy.Radius + _fightField.PlayerController.Radius - 15f)
                {
                    _fightField.PlayerController.PlayerKilled();
                }
            }
        }
        #endregion
        
    }
}
