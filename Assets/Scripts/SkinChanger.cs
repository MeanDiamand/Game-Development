using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

public class SkinChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite[] body;

    [SerializeField]
    private Sprite[] helmet;
    [SerializeField]
    private Sprite[] chestplate;
    [SerializeField]
    private Sprite[] leggins = null;
    [SerializeField]
    private Sprite[] boots = null;

    [SerializeField]
    private Sprite[] sword = null;

    //[SerializeField]
    //private Inventory inventory;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private const int SMALL_SPRITE_SIZE = 64;
    private const int BIG_SPRITE_SIZE = 192;
    private const int THRESHHOLD = 179;
    private const int PPU = 16;

    private void Start()
    {
        PlayerEvents.GetInstance().OnArmourChanged += ArmourChanged;
        PlayerEvents.GetInstance().OnWeaponChanged += WeaponChanged;
    }

    private void ArmourChanged(Sprite[] sprites, int index)
    {
        Debug.Log("ArmourChanged: " + index);
        switch (index)
        {
            case 0:
                helmet = sprites;
                break;
            case 1: 
                chestplate = sprites;
                break;
            case 2: 
                leggins = sprites;
                break;
            case 3: 
                boots = sprites; 
                break;
        }
    }

    private void WeaponChanged(Sprite[] sprites)
    {
        Debug.Log("WeaponChanged");
        sword = sprites;
    }

    public void SkinChoice()
    {
        string spriteName = spriteRenderer.sprite.name;

        string pattern = @"\d+$"; // This pattern matches one or more digits at the end of the string
        Match match = Regex.Match(spriteName, pattern);
        if (match.Success)
        {
            spriteName = match.Value;
            int n = int.Parse(spriteName);
            int lut_n = LUT(n);
            //spriteRenderer.sprite = body[n];
            Sprite sprite1 = body[lut_n];

            Sprite[] armour = new Sprite[4];
            if (helmet != null && helmet.Length > 0)
                armour[0] = helmet[lut_n];
            if (chestplate != null && chestplate.Length > 0)
                armour[1] = chestplate[lut_n];
            if (leggins != null && leggins.Length > 0)
                armour[2] = leggins[lut_n];
            if (boots != null && boots.Length > 0)
                armour[3] = boots[lut_n];

            Sprite spriteSword = FindSpriteWithNumberEnding(sword, n);

            //spriteRenderer.sprite = CombineSprites(sprite1, sprite2);
            if (spriteSword != null)
                if (n < THRESHHOLD)
                    spriteRenderer.sprite = CombineSprites(spriteSword, CombineSprites(sprite1, armour));
                else
                    spriteRenderer.sprite = CombineSprites(spriteSword, CombineSprites(sprite1, armour), BIG_SPRITE_SIZE);
            else
                spriteRenderer.sprite = CombineSprites(sprite1, armour);
        }
    }

    private int LUT(int n)
    {
        if (n < THRESHHOLD)
            return n;
        if (n > 2 * THRESHHOLD)
            return THRESHHOLD;
        return n - THRESHHOLD + 97;
    }

    public Sprite FindSpriteWithNumberEnding(Sprite[] sprites, int number)
    {
        if (sprites == null) return null;
        Debug.Log(sprites);
        foreach (Sprite sprite in sprites) {
            if (EndsWithNumber(sprite.name, number))
                return sprite;
        }

        // Return the found sprite (or null if not found)
        return null;
    }
    static bool EndsWithNumber(string input, int targetNumber)
    {
        // Extracting the last part of the string after '_'
        int lastIndex = input.LastIndexOf('_');
        if (lastIndex == -1)
            return false;

        string numberPart = input.Substring(lastIndex + 1);

        // Checking if the extracted part is a valid number
        if (int.TryParse(numberPart, out int extractedNumber))
        {
            // Comparing with the target number
            return extractedNumber == targetNumber;
        }

        return false;
    }

    private Sprite CombineSprites(Sprite baseSprite, Sprite overlaySprite, int baseSize = SMALL_SPRITE_SIZE)
    {
        // Create a new texture with the size of the base sprite
        Texture2D combinedTexture = new Texture2D(baseSize, baseSize);
        combinedTexture.filterMode = FilterMode.Point;

        //int baseSize = (int)baseSprite.rect.width;

        // Get pixels from base sprite
        Color[] basePixels = baseSprite.texture.GetPixels((int)baseSprite.rect.x, (int)baseSprite.rect.y, baseSize, baseSize);

        // Calculate the offset to center the small sprite within the big sprite
        int xOffset = (int)(baseSize - SMALL_SPRITE_SIZE) / 2;
        int yOffset = (int)(baseSize - SMALL_SPRITE_SIZE) / 2;

        // Get pixels from overlay sprite
        Color[] overlayPixels = overlaySprite.texture.GetPixels((int)overlaySprite.rect.x, (int)overlaySprite.rect.y, SMALL_SPRITE_SIZE, SMALL_SPRITE_SIZE);

        // Iterate through each pixel of the small sprite and overlay it onto the corresponding pixel of the big sprite
        for (int y = 0; y < SMALL_SPRITE_SIZE; y++)
        {
            for (int x = 0; x < SMALL_SPRITE_SIZE; x++)
            {
                int baseIndex = (y + yOffset) * baseSize + (x + xOffset);
                int overlayIndex = y * SMALL_SPRITE_SIZE + x;

                if (overlayPixels[overlayIndex].a > 0)
                {
                    basePixels[baseIndex] = overlayPixels[overlayIndex];
                }
            }
        }

        // Set the combined pixels to the new texture
        combinedTexture.SetPixels(basePixels);

        // Apply changes and create a new sprite
        combinedTexture.Apply();

        return Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f), PPU);
    }


    private Sprite CombineSprites(Sprite baseSprite, Sprite[] overlaySprites)
    {
        // Create a new texture with the size of the base sprite
        Texture2D combinedTexture = new Texture2D((int)baseSprite.rect.width, (int)baseSprite.rect.height);
        combinedTexture.filterMode = FilterMode.Point;

        int baseSize = (int)baseSprite.rect.width;

        // Get pixels from base sprite
        Color[] basePixels = baseSprite.texture.GetPixels((int)baseSprite.rect.x, (int)baseSprite.rect.y, baseSize, baseSize);

        // Iterate over each overlay sprite
        foreach (Sprite overlaySprite in overlaySprites)
        {
            if (overlaySprite == null)
                continue;
            // Calculate the offset to center the small sprite within the big sprite
            int xOffset = (int)(baseSize - SMALL_SPRITE_SIZE) / 2;
            int yOffset = (int)(baseSize - SMALL_SPRITE_SIZE) / 2;

            // Get pixels from overlay sprite
            Color[] overlayPixels = overlaySprite.texture.GetPixels((int)overlaySprite.rect.x, (int)overlaySprite.rect.y, SMALL_SPRITE_SIZE, SMALL_SPRITE_SIZE);

            // Iterate through each pixel of the small sprite and overlay it onto the corresponding pixel of the big sprite
            for (int y = 0; y < SMALL_SPRITE_SIZE; y++)
            {
                for (int x = 0; x < SMALL_SPRITE_SIZE; x++)
                {
                    int baseIndex = (y + yOffset) * (int)baseSprite.rect.width + (x + xOffset);
                    int overlayIndex = y * SMALL_SPRITE_SIZE + x;

                    if (overlayPixels[overlayIndex].a > 0)
                    {
                        basePixels[overlayIndex] = overlayPixels[overlayIndex];
                    }
                }
            }
        }

        // Set the combined pixels to the new texture
        combinedTexture.SetPixels(basePixels);

        // Apply changes and create a new sprite
        combinedTexture.Apply();

        return Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f), PPU);
    }



    //private Sprite CombineSprites(Sprite bodySprite, Sprite helmetSprite)
    //{
    //    // Create a new texture with the size of the body sprite
    //    Texture2D combinedTexture = new Texture2D((int)bodySprite.rect.width, (int)bodySprite.rect.height);

    //    combinedTexture.filterMode = FilterMode.Point;

    //    // Get pixels from body sprite
    //    Color[] bodyPixels = bodySprite.texture.GetPixels((int)bodySprite.rect.x, (int)bodySprite.rect.y, (int)bodySprite.rect.width, (int)bodySprite.rect.height);

    //    // Get pixels from helmet sprite
    //    Color[] helmetPixels = helmetSprite.texture.GetPixels((int)helmetSprite.rect.x, (int)helmetSprite.rect.y, (int)helmetSprite.rect.width, (int)helmetSprite.rect.height);

    //    // Iterate through each pixel and overlay helmet pixels onto body pixels
    //    for (int i = 0; i < bodyPixels.Length; i++)
    //    {
    //        if (helmetPixels[i].a > 0)
    //        {
    //            bodyPixels[i] = helmetPixels[i];
    //        }
    //    }

    //    // Set the combined pixels to the new texture
    //    combinedTexture.SetPixels(bodyPixels);

    //    // Apply changes and create a new sprite
    //    combinedTexture.Apply();

    //    return Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f), PPU);
    //}

    //private Sprite CombineSprites(Sprite bodySprite, Sprite[] additionalSprites)
    //{
    //    // Create a new texture with the size of the body sprite
    //    Texture2D combinedTexture = new Texture2D((int)bodySprite.rect.width, (int)bodySprite.rect.height);

    //    combinedTexture.filterMode = FilterMode.Point;

    //    // Get pixels from body sprite
    //    Color[] bodyPixels = bodySprite.texture.GetPixels((int)bodySprite.rect.x, (int)bodySprite.rect.y, SMALL_SPRITE_SIZE, SMALL_SPRITE_SIZE);

    //    // Iterate through each additional sprite
    //    foreach (Sprite additionalSprite in additionalSprites)
    //    {
    //        // Get pixels from the additional sprite
    //        Color[] additionalPixels = additionalSprite.texture.GetPixels((int)additionalSprite.rect.x, (int)additionalSprite.rect.y, SMALL_SPRITE_SIZE, SMALL_SPRITE_SIZE);

    //        // Iterate through each pixel and overlay additional pixels onto body pixels
    //        for (int i = 0; i < bodyPixels.Length && i < additionalPixels.Length; i++)
    //        {
    //            if (additionalPixels[i].a > 0)
    //            {
    //                bodyPixels[i] = additionalPixels[i];
    //            }
    //        }
    //    }

    //    // Set the combined pixels to the new texture
    //    combinedTexture.SetPixels(bodyPixels);

    //    // Apply changes and create a new sprite
    //    combinedTexture.Apply();

    //    return Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f), PPU);
    //}

}
