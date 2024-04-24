using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class GameplayFightLogic : ILogic
    {
        private GameplayFightController _fightController;

        public void Initialize(GameplayFightController fightController)
        {
            _fightController = fightController;
        }
        
        public void MyUpdate()
        {
            _fightController.PlayerController.UpdatePlayer();
            
            foreach (var bullet in _fightController.BulletsList)
            {
                bullet.UpdateBullet();

                if (bullet.BulletType == BulletType.ENEMY)
                {
                    CheckCollisionWithPlayer(bullet);
                }

                if (bullet.BulletType == BulletType.PLAYER)
                {
                    CheckCollisionWithEnemies(bullet);
                }
            }

            foreach (IEnemy enemy in _fightController.EnemiesList)
            {
                enemy.UpdateEnemy();
            }
        }

        private void CheckCollisionWithPlayer(BulletController bullet)
        {
            float distanceToPlayer = (bullet.transform.position - _fightController.PlayerController.transform.position).magnitude;
            if (distanceToPlayer < 25f)
            {
                _fightController.PlayerController.PlayerKilled();
            }
        }
        
        private void CheckCollisionWithEnemies(BulletController bullet)
        {
            foreach (IEnemy enemy in _fightController.EnemiesList)
            {
                float distanceToEnemy = (bullet.transform.position - enemy.Transform.position).magnitude;
                if (distanceToEnemy < enemy.Radius)
                {
                    _fightController.EnemyKilled(enemy);
                    bullet.DestroyBullet();
                }
            }
        }
        
    }
    
}
