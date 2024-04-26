using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class GameplayFightLogic : ILogic
    {
        #region Fields
        private FightField _fightField;
        #endregion



        #region Methods
        public void Initialize(FightField fightField)
        {
            _fightField = fightField;
        }
        
        public void MyUpdate()
        {
            _fightField.PlayerController.UpdatePlayer();
            
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

        private void CheckBulletCollisionWithPlayer(BulletController bullet)
        {
            float distanceToPlayer = (bullet.transform.position - _fightField.PlayerController.transform.position).magnitude;
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
                float distanceToEnemy = (_fightField.PlayerController.transform.position - enemy.Transform.position).magnitude;
                if (distanceToEnemy < enemy.Radius + _fightField.PlayerController.Radius - 10f)
                {
                    _fightField.PlayerController.PlayerKilled();
                }
            }
        }
        #endregion
        
    }
}
