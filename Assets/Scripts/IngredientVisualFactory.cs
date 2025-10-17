using System.Collections.Generic;
using UnityEngine;

public static class IngredientVisualFactory
{
    private static readonly Dictionary<IngredientType, Sprite> SpriteCache = new();

    public static Sprite GetSprite(IngredientType type)
    {
        if (SpriteCache.TryGetValue(type, out Sprite cachedSprite))
        {
            return cachedSprite;
        }

        Sprite sprite = type switch
        {
            IngredientType.Chili => CreateChiliSprite(),
            IngredientType.Butter => CreateButterSprite(),
            IngredientType.Bread => CreateBreadSprite(),
            _ => null
        };

        if (sprite != null)
        {
            SpriteCache[type] = sprite;
        }

        return sprite;
    }

    public static Vector3 GetScale(IngredientType type, float baseScale)
    {
        return type switch
        {
            IngredientType.Chili => new Vector3(baseScale * 1.05f, baseScale * 0.75f, 1f),
            IngredientType.Butter => new Vector3(baseScale * 0.85f, baseScale * 0.85f, 1f),
            IngredientType.Bread => new Vector3(baseScale * 0.7f, baseScale * 0.6f, 1f),
            _ => new Vector3(baseScale, baseScale, 1f)
        };
    }

    private static Sprite CreateChiliSprite()
    {
        const int width = 72;
        const int height = 52;
        Texture2D texture = CreateBlankTexture(width, height);

        float centerX = (width - 1) * 0.5f;
        float centerY = (height - 1) * 0.5f;
        float radiusX = width * 0.45f;
        float radiusY = height * 0.45f;

        Color bodyColor = new Color(0.88f, 0.24f, 0.16f);
        Color highlightColor = new Color(1f, 0.55f, 0.35f);

        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float dx = (x - centerX) / radiusX;
                float dy = (y - centerY) / radiusY;
                float ellipse = (dx * dx) + (dy * dy);

                if (ellipse <= 1f)
                {
                    float highlightAmount = Mathf.Clamp01((centerY - y) / radiusY * 0.5f + 0.5f);
                    Color pixel = Color.Lerp(bodyColor, highlightColor, highlightAmount * 0.6f);
                    pixels[(y * width) + x] = pixel;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        texture.name = "RuntimeChiliOval";
        return CreateSprite(texture);
    }

    private static Sprite CreateButterSprite()
    {
        const int width = 72;
        const int height = 72;
        Texture2D texture = CreateBlankTexture(width, height);

        float centerX = (width - 1) * 0.5f;
        float centerY = (height - 1) * 0.5f;
        float radiusX = width * 0.45f;
        float radiusY = height * 0.45f;

        Color bodyColor = new Color(0.99f, 0.91f, 0.47f);
        Color shadowColor = new Color(0.85f, 0.74f, 0.28f);
        Color highlightColor = new Color(1f, 0.98f, 0.74f);

        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float dx = Mathf.Abs((x - centerX) / radiusX);
                float dy = Mathf.Abs((y - centerY) / radiusY);
                float diamond = dx + dy;

                if (diamond <= 1f)
                {
                    float edgeBlend = Mathf.Clamp01(1f - diamond);
                    Color basePixel = Color.Lerp(shadowColor, bodyColor, edgeBlend);
                    float highlightAmount = Mathf.Clamp01((centerY - y) / radiusY * 0.65f + 0.35f);
                    Color finalPixel = Color.Lerp(basePixel, highlightColor, highlightAmount * 0.5f);
                    pixels[(y * width) + x] = finalPixel;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        texture.name = "RuntimeButterRhombus";
        return CreateSprite(texture);
    }

    private static Sprite CreateBreadSprite()
    {
        const int width = 60;
        const int height = 46;
        Texture2D texture = CreateBlankTexture(width, height);

        Color crustColor = new Color(0.74f, 0.47f, 0.27f);
        Color crumbColor = new Color(0.95f, 0.9f, 0.74f);
        Color highlightColor = new Color(1f, 0.97f, 0.86f);

        int borderThickness = 5;

        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool insideBorder = x >= borderThickness &&
                                    x < width - borderThickness &&
                                    y >= borderThickness &&
                                    y < height - borderThickness;

                if (!insideBorder)
                {
                    pixels[(y * width) + x] = crustColor;
                    continue;
                }

                float verticalHighlight = Mathf.Clamp01(1f - (float)(y - borderThickness) / (height - borderThickness * 2));
                Color crumb = Color.Lerp(crumbColor, highlightColor, verticalHighlight * 0.4f);
                pixels[(y * width) + x] = crumb;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        texture.name = "RuntimeBreadSlice";
        return CreateSprite(texture);
    }

    private static Texture2D CreateBlankTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        Color[] clearPixels = new Color[width * height];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.clear;
        }

        texture.SetPixels(clearPixels);
        texture.Apply();

        return texture;
    }

    private static Sprite CreateSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);
    }
}
