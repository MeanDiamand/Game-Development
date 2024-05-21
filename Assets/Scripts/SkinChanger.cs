using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite[] body;

    private List<SpritesContainer> upgrades = new List<SpritesContainer>();

    [SerializeField]
    private SpritesContainer sword;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private PlayerController playerController;

    private Sprite[] spriteSheet;

    private const int SMALL_SPRITE_SIZE = 64;
    private const int BIG_SPRITE_SIZE = 192;
    private const int MAX_SPRITE_ID = 202;
    private const int THRESHHOLD = 179;
    private const int PPU = 16;
    private static readonly string[] subfolders = { "Sprites/", "Sprites/Boots/", "Sprites/Chestplates/",
            "Sprites/Helmets/", "Sprites/Leggins/", "Sprites/Weapons/" };

    private void Start()
    {
        PlayerEvents.GetInstance().OnArmourChanged += ArmourChanged;
        PlayerEvents.GetInstance().OnWeaponChanged += WeaponChanged;

        SpritesContainer[] containers = playerController.GetWearableSprites();

        for (int i = 0; i < 7; i++)
        {
            upgrades.Add(containers[i]);
        }
        sword = containers[4];

        GenerateSpriteSheet();
    }

    private void OnDestroy()
    {
        PlayerEvents.GetInstance().OnArmourChanged -= ArmourChanged;
        PlayerEvents.GetInstance().OnWeaponChanged -= WeaponChanged;
    }

    private void ArmourChanged(SpritesContainer sprites, int index)
    {
        upgrades[index] = sprites;
        GenerateSpriteSheet();
    }

    private void WeaponChanged(SpritesContainer sprites)
    {
        sword = sprites;
        GenerateSpriteSheet();
    }

    private void GenerateSpriteSheet()
    {
        spriteSheet = new Sprite[MAX_SPRITE_ID + 1];

        for (int n = 0; n <= MAX_SPRITE_ID; n++)
        {
            int lut_n = LUT(n);
            Sprite bodySprite = body[lut_n];

            List<Sprite> overlaySprites = new List<Sprite>();
            for (int i = 0; i < upgrades.Count; i++)
            {
                if (upgrades[i] != null)
                {
                    Sprite upgrade = upgrades[i].GetByID(lut_n);
                    if (upgrade != null)
                        overlaySprites.Add(upgrade);
                }
            }

            Sprite spriteSword = sword?.GetByID(n);
            bool isOverlayed = false;
            if (sword != null)
                isOverlayed = sword.isOverlayed(n);

            if (spriteSword != null)
                if (n < THRESHHOLD)
                    spriteSheet[n] = CombineSprites(spriteSword, CombineSprites(bodySprite, overlaySprites), isOverlayed);
                else
                    spriteSheet[n] = CombineSprites(spriteSword, CombineSprites(bodySprite, overlaySprites), isOverlayed, BIG_SPRITE_SIZE);
            else
                spriteSheet[n] = CombineSprites(bodySprite, overlaySprites);
        }
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
            spriteRenderer.sprite = spriteSheet[n];
        }
    }

    private int LUT(int n)
    {
        if (n < THRESHHOLD)
            return n;
        if (n > 2 * THRESHHOLD)
            return THRESHHOLD - 1;
        return n - THRESHHOLD + 97;
    }

    private static Sprite[] SpritesLoadAll(string itemName)
    {
        foreach (string folder in subfolders)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(folder + itemName);

            if (sprites != null && sprites.Length > 0)
            {
                return sprites;
            }
        }
        return null;
    }

    public static Sprite[] FindSpriteSheetByFullName(string name)
    {
        if (name == null)
            return null;
        string[] parts = name.Split('_');

        if (parts.Length < 2)
        {
            Debug.LogError("NameToSprite: Invalid input format");
            return null;
        }
        string itemName = string.Join("_", parts.Take(parts.Length - 1)); // Join all parts except the last one

        return SpritesLoadAll(itemName);
    }

    public static Sprite FullNameToSprite(string name)
    {
        string[] parts = name.Split('_');

        if (parts.Length < 2)
        {
            Debug.LogError("NameToSprite: Invalid input format");
            return null;
        }
        string itemName = string.Join("_", parts.Take(parts.Length - 1)); // Join all parts except the last one
        int itemNumber = int.Parse(parts[parts.Length - 1]); // Get the last part

        return FindSpriteWithNumberEnding(SpritesLoadAll(itemName), itemNumber);
    }

    public static Sprite FindSpriteWithNumberEnding(Sprite[] sprites, int number)
    {
        if (sprites == null) return null;
        foreach (Sprite sprite in sprites)
        {
            if (EndsWithNumber(sprite.name, number))
                return sprite;
        }

        // Return null if not found
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

    private Sprite CombineSprites(Sprite baseSprite, Sprite overlaySprite, bool order = true, int baseSize = SMALL_SPRITE_SIZE)
    {
        // Create a new texture with the size of the base sprite
        Texture2D combinedTexture = new Texture2D(baseSize, baseSize);
        combinedTexture.filterMode = FilterMode.Point;

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

                if (!order)
                {
                    if (overlayPixels[overlayIndex].a > 0)
                        basePixels[baseIndex] = overlayPixels[overlayIndex];
                }
                else
                {
                    if (basePixels[baseIndex].a <= 0)
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

    private Sprite CombineSprites(Sprite baseSprite, List<Sprite> overlaySprites)
    {
        return CombineSprites(baseSprite, overlaySprites.ToArray());
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
}
