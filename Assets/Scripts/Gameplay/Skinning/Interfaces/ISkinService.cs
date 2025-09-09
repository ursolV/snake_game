using System;

namespace SnakeGame.Gameplay.Skinning
{
    public interface ISkinService
    {
        string CurrentSkinId { get; }

        event Action<string> SkinChanged;

        bool SetSkin(string skinId);
        string GetBundleName(string skinId);
    }
}


