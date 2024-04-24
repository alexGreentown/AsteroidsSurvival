// using AsteroidsSurvival.Interfaces;
using UnityEngine;

namespace AsteroidsSurvival.View
{
    public class MainMenuUIController : MonoBehaviour, IView
    {
        [SerializeField] private GameObject _pressText;
        
        public void SetTextActive(bool isActive)
        {
            _pressText.SetActive(isActive);
        }
    }
}
