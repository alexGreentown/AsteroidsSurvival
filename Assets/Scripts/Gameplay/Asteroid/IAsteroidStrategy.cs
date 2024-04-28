using UnityEngine.UI;

namespace AsteroidsSurvival.View.Gameplay.Asteroid
{
    public interface IAsteroidStrategy
    {
        float Radius { get;}

        float SpeedFactor { get;}

        void SetGraphics(Image image);
    }
}
