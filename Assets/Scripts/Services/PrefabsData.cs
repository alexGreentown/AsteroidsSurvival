using UnityEngine;

namespace AsteroidsSurvival.Services
{
    [CreateAssetMenu(fileName = "PrefabsData", menuName = "Data/PrefabsData", order = 1)]
    public class PrefabsData : ScriptableObject
    {
        public GameObject MainMenuUIController;
        public GameObject GameplayUIController;

        public GameObject PlayerController;
        public GameObject AsteroidController;
        public GameObject UFOController;
        public GameObject BulletController;

        public GameObject Explosion;
        
        public float AsteroidsCreateDelayValue = 5f;
        public float UFOCreateDelayValue = 8f;
        
        public Sprite GraphicsEnemyBullet;
        public Sprite GraphicsPlayerBullet;
        
    }
}