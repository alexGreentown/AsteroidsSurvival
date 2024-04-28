using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.Services;
using UnityEngine.UI;

namespace AsteroidsSurvival.View.Gameplay.Asteroid
{
    public class AsteroidStrategySmall : IAsteroidStrategy
    {
        public float Radius { get;} = 20f;
        public float SpeedFactor { get;} = 80f;

        public void SetGraphics(Image image)
        {
            PrefabsDataService prefabsDataService = MyServiceLocator.Get<PrefabsDataService>();
            image.sprite = prefabsDataService.PrefabsData.GraphicsAsteroidSmall;
        }
    }
}
