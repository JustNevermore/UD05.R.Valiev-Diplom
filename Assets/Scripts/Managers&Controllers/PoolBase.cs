﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers_Controllers
{
    public class PoolBase<T> where T: MonoBehaviour
    {
        private T _prefab;
        private bool _autoExpand;
        private Transform _container;

        private List<T> _pool;

        public PoolBase(T prefab, int count, bool flag, Transform container)
        {
            _prefab = prefab;
            _autoExpand = flag;
            _container = container;

            CreatePool(count);
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateElement();
            }
        }

        private T CreateElement()
        {
            var newObject = Object.Instantiate(_prefab, _container);
            newObject.gameObject.SetActive(false);
            _pool.Add(newObject);
            return newObject;
        }

        private bool GetFreeElement(out T element)
        {
            foreach (var obj in _pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    element = obj;
                    obj.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }
        
        public T GetPoolElement()
        {
            if (GetFreeElement(out var element))
            {
                return element;
            }

            if (_autoExpand)
            {
                var obj = CreateElement();
                obj.gameObject.SetActive(true);
                return obj;
            }

            throw new Exception("Object pool doesnt have free elements");
        }
    }
}