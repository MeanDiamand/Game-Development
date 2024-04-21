using System.Text.RegularExpressions;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite[] body;

    [SerializeField]
    private Sprite[] helmet;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SkinChoice()
    {
        string spriteName = spriteRenderer.sprite.name;

        string pattern = @"\d+$"; // This pattern matches one or more digits at the end of the string
        Match match = Regex.Match(spriteName, pattern);
        if (match.Success)
        {
            spriteName = match.Value;
            int n = int.Parse(spriteName);

            //spriteRenderer.sprite = body[n];

            Sprite sprite1 = body[n];
            Sprite sprite2 = helmet[n];

            spriteRenderer.sprite = CombineSprites(sprite1, sprite2);
        }
    }
    private Sprite CombineSprites(Sprite bodySprite, Sprite helmetSprite)
    {
        // Create a new texture with the size of the body sprite
        Texture2D combinedTexture = new Texture2D((int)bodySprite.rect.width, (int)bodySprite.rect.height);

        combinedTexture.filterMode = FilterMode.Point;

        // Get pixels from body sprite
        Color[] bodyPixels = bodySprite.texture.GetPixels((int)bodySprite.rect.x, (int)bodySprite.rect.y, (int)bodySprite.rect.width, (int)bodySprite.rect.height);

        // Get pixels from helmet sprite
        Color[] helmetPixels = helmetSprite.texture.GetPixels((int)helmetSprite.rect.x, (int)helmetSprite.rect.y, (int)helmetSprite.rect.width, (int)helmetSprite.rect.height);

        // Iterate through each pixel and overlay helmet pixels onto body pixels
        for (int i = 0; i < bodyPixels.Length; i++)
        {
            if (helmetPixels[i].a > 0)
            {
                bodyPixels[i] = helmetPixels[i];
            }
        }

        // Set the combined pixels to the new texture
        combinedTexture.SetPixels(bodyPixels);

        // Apply changes and create a new sprite
        combinedTexture.Apply();

        return Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f), 16);
    }

    private Sprite CombineSprites(Sprite bodySprite, Sprite[] additionalSprites)
    {
        // Create a new texture with the size of the body sprite
        Texture2D combinedTexture = new Texture2D((int)bodySprite.rect.width, (int)bodySprite.rect.height);

        combinedTexture.filterMode = FilterMode.Point;

        // Get pixels from body sprite
        Color[] bodyPixels = bodySprite.texture.GetPixels((int)bodySprite.rect.x, (int)bodySprite.rect.y, (int)bodySprite.rect.width, (int)bodySprite.rect.height);

        // Iterate through each additional sprite
        foreach (Sprite additionalSprite in additionalSprites)
        {
            // Get pixels from the additional sprite
            Color[] additionalPixels = additionalSprite.texture.GetPixels((int)additionalSprite.rect.x, (int)additionalSprite.rect.y, (int)additionalSprite.rect.width, (int)additionalSprite.rect.height);

            // Iterate through each pixel and overlay additional pixels onto body pixels
            for (int i = 0; i < bodyPixels.Length && i < additionalPixels.Length; i++)
            {
                if (additionalPixels[i].a > 0)
                {
                    bodyPixels[i] = additionalPixels[i];
                }
            }
        }

        // Set the combined pixels to the new texture
        combinedTexture.SetPixels(bodyPixels);

        // Apply changes and create a new sprite
        combinedTexture.Apply();

        return Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f), 16);
    }

}
