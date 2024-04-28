using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.Services;
using UnityEngine.UI;

namespace AsteroidsSurvival.View.Gameplay.Asteroid
{
    public class AsteroidStrategyBig : IAsteroidStrategy
    {
        public float Radius { get;} = 40f;
        public float SpeedFactor { get;} = 50f;

        public void SetGraphics(Image image)
        {
            PrefabsDataService prefabsDataService = MyServiceLocator.Get<PrefabsDataService>();
            image.sprite = prefabsDataService.PrefabsData.GraphicsAsteroidBig;
        }
    }
}
