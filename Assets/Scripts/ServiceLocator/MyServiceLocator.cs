using System;
using System.Collections.Generic;
using AsteroidsSurvival.Services;

namespace AsteroidsSurvival.ServiceLocator
{
    public static class MyServiceLocator
    {
        #region Fields
        static Dictionary<Type, IService> _itemsMap { get; } = new();
        #endregion



        #region Methods
        public static T Register<T>(T newService) where T: IService
        {
            var type = newService.GetType();

            if (_itemsMap.ContainsKey(type))
            {
                throw new Exception($"Items Map already contains this Service");
            }

            _itemsMap[type] = newService;

            return newService;
        }

        public static void Unregister<T>(T service) where T : IService
        {
            var type = service.GetType();

            if (_itemsMap.ContainsKey(type))
            {
                _itemsMap.Remove(type);
            }
            else
            {
                throw new Exception($"Items Map doesnt contain this Service");
            }
        }

        public static T Get<T>() where T : IService
        {
            var type = typeof(T);

            if (!_itemsMap.ContainsKey(type))
            {
                throw new Exception($"Items Map doesnt contain this Service");
            }
            
            return(T)_itemsMap[type];
        }
        #endregion
        
    }
}
