using UnityEngine;

namespace SnakeGame.Core.AssetBundles.Pooling
{
    public abstract class PoolableMonoBehaviour : MonoBehaviour, IPoolableAsset
    {
        public virtual void OnSpawnFromPool()
        {
            // Реактивація об'єкта
            gameObject.SetActive(true);
        }

        public virtual void OnReturnToPool()
        {
            // Деактивація та скидання стану
            gameObject.SetActive(false);
            
            // Скидання трансформа
            transform.SetParent(null);
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}