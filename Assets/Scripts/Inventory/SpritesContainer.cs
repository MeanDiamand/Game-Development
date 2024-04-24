using System.Collections.Generic;
using UnityEngine;

public class SpritesContainer
{
    private Dictionary<int, int> idMap = new Dictionary<int, int>();

    private Sprite[] _sprites;
    
    [field: SerializeField]
    public Sprite[] Sprites
    {
        private get { return _sprites; }
        set
        {
            _sprites = value;
            GenerateIdMap();
        }
    }

    public SpritesContainer(Sprite[] sprites)
    {
        Sprites = sprites;
    }

    private void GenerateIdMap()
    {
        idMap = new Dictionary<int, int>();

        if (_sprites == null) return;

        for  (int i = 0; i < Sprites.Length; i++)
        {
            string name = Sprites[i].name;

            int lastIndex = name.LastIndexOf('_');
            if (lastIndex == -1)
                continue;

            string numberPart = name.Substring(lastIndex + 1);

            // Checking if the extracted part is a valid number
            if (int.TryParse(numberPart, out int extractedNumber))
            {
                idMap[extractedNumber] = i;
            }
        }
    }

    public Sprite GetByIndex(int index) {
        if (index < 0 || index >= _sprites.Length) return null;
        return Sprites[index]; 
    }
    public Sprite GetByID(int id) 
    {
        try
        {
            return Sprites[idMap[id]];
        }
        catch (KeyNotFoundException ex)
        {
            return null;
        }
    }
}
