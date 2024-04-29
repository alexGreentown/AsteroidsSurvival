using System.Collections.Generic;

namespace AsteroidsSurvival.Utils
{
    public class GenericPool<T>
    {
        private List<T> _poolObjects = new();

        public void Add(T obj)
        {
            if (!Contains(obj))
            {
                _poolObjects.Add(obj);
            }
        }

        public void Remove(T obj)
        {
            if (Contains(obj))
            {
                _poolObjects.Remove(obj);
            }
        }

        public T Get()
        {
            if (_poolObjects.Count > 0)
            {
                T tempObject = _poolObjects[0];
                _poolObjects.Remove(tempObject);
                return tempObject;
            }
            
            return default(T);
        }
        
        public bool Contains(T obj)
        {
            return _poolObjects.Contains(obj);
        }
    }
}
