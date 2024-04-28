using System;
using UnityEngine;
using Random = System.Random;

namespace AsteroidsSurvival.Gameplay.Shot
{
    public class LaserController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _laserObject;
        [SerializeField] private Transform _laserContainer;
        [SerializeField] private GameObject _sparkObject;

        private Transform[] _sparks = new Transform[5];
        
        private bool _beamsBuilt;
        private float _beamsSideOffset = 0.17f;
        
        public Transform[] LaserRectanglePoints = new Transform[4];
        #endregion
        
        
        
        #region Methods

        public void Initialize(float rotation, Vector2 position)
        {
            gameObject.SetActive(true);
            
            _laserObject.SetActive(true);

            if (!_beamsBuilt)
            {
                _beamsBuilt = true;
                BuildLaserBeams();
                BuildSparks();
            }
            
            void BuildLaserBeams()
            {
                float beamPartHeight = 0.20f;
                int beamElementsLength = 55;
                for (int i = 0; i < beamElementsLength; i++)
                {
                    Vector2 beamPosition;
                    beamPosition.x = _beamsSideOffset;
                    beamPosition.y = i * beamPartHeight;
                    GameObject newBeam = Instantiate(_laserObject, _laserContainer);
                    newBeam.transform.localPosition = beamPosition;
                    if (i == 0)
                    {
                        LaserRectanglePoints[0] = newBeam.transform;
                    }
                    if (i == beamElementsLength - 1)
                    {
                        LaserRectanglePoints[2] = newBeam.transform;
                    }
                    
                    beamPosition.x = -_beamsSideOffset;
                    newBeam = Instantiate(_laserObject, _laserContainer);
                    newBeam.transform.localPosition = beamPosition;
                    
                    if (i == 0)
                    {
                        LaserRectanglePoints[1] = newBeam.transform;
                    }
                    if (i == beamElementsLength - 1)
                    {
                        LaserRectanglePoints[3] = newBeam.transform;
                    }
                }
            }

            void BuildSparks()
            {
                for (int i = 0; i < _sparks.Length; i++)
                {
                    GameObject newSpark =  Instantiate(_sparkObject, _laserContainer);
                    _sparks[i] = newSpark.transform;
                }
            }

            _laserObject.SetActive(false);
            
            transform.position = position;
            
            transform.rotation = Quaternion.Euler(0f, 0f, -rotation);
        }

        public void SwitchOff()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateSparks()
        {
            int random = UnityEngine.Random.Range(0, _sparks.Length);
            Vector2 randomPosition = new();
            randomPosition.x = UnityEngine.Random.Range(-_beamsSideOffset, _beamsSideOffset);
            randomPosition.y = UnityEngine.Random.Range(0f, 20f);
            _sparks[random].localPosition = randomPosition;
        }

        #endregion

    }
}
