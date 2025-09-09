using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame.Gameplay.Skinning
{
    [CreateAssetMenu(fileName = "SkinCatalog", menuName = "Snake Game/Skin Catalog", order = 0)]
    public class SkinCatalog : ScriptableObject
    {
        [Serializable]
        public class SkinEntry
        {
            public string skinId;
            public string name;
        }

        [SerializeField] private List<SkinEntry> _entries = new List<SkinEntry>();

        public IEnumerable<string> GetAllIds()
        {
            if (_entries == null) yield break;
            foreach (var e in _entries)
            {
                if (e != null && !string.IsNullOrEmpty(e.skinId))
                    yield return e.skinId;
            }
        }

        public bool TryResolve(string skinId, out string name)
        {
            name = null;
            if (string.IsNullOrEmpty(skinId)) return false;

            if (_entries != null)
            {
                foreach (var e in _entries)
                {
                    if (e == null || e.skinId != skinId) continue;
                    name = string.IsNullOrEmpty(e.name) ? skinId : e.name;
                    return true;
                }
            }

            // Not found
            return false;
        }
    }
}


