using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Utility.Singleton;

public class C_ResourceManager : MonoSingleton<C_ResourceManager>
{
    SpriteAtlas _atlas;

    public Sprite GetSprite(string resourceName)
    {
        if(_atlas == null)
        {
            _atlas = Resources.Load<SpriteAtlas>("Item");
        }

        return _atlas.GetSprite(resourceName);
    }
}
