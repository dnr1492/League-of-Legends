using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private Dictionary<string, Sprite> dicSprites = new Dictionary<string, Sprite>();

    private void Awake()
    {
        Add(Resources.LoadAll<Sprite>("Champion_Sprites"));
    }

    public void Add(Sprite[] sprites)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] == null) return;
            string name = sprites[i].name;
            if (dicSprites.ContainsKey(name)) continue;
            dicSprites.Add(name, sprites[i]);
        }
    }

    public Sprite Get(string name)
    {
        if (dicSprites.ContainsKey(name)) return dicSprites[name];
        return null;
    }
}
