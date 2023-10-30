using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TextureCanvas
{
    public readonly int width;
    public readonly int height;
    public readonly Color[,] pixels;

    public TextureCanvas(Texture2D texture)
    {
        this.width = texture.width;
        this.height = texture.height;
        pixels = ConvertTo2DArray(texture.GetPixels(), texture.width);
    }

    public TextureCanvas(Color[,] _pixels)
    {
        this.width = _pixels.GetLength(0);
        this.height = _pixels.GetLength(1);
        pixels = _pixels;
    }

    public TextureCanvas(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.pixels = GeneratePixels(width, height, Color.clear);
    }

    public TextureCanvas(int width, int height, Color fill)
    {
        this.width = width;
        this.height = height;
        this.pixels = GeneratePixels(width, height, fill);
    }

    public void AddPixels(TextureCanvas canvas, int startX, int startY)
    {
        int subArrayWidth = canvas.width;
        int subArrayHeight = canvas.height;

        var mode = CompositingMode.Overlay;

        for (int i = 0; i < subArrayWidth; i++)
        {
            for (int j = 0; j < subArrayHeight; j++)
            {
                int x = startX + i;
                int y = startY + j;

                // Check if the indices are within the bounds of the source array
                if (x >= 0 && x < pixels.GetLength(0) && y >= 0 && y < pixels.GetLength(1))
                {
                    pixels[x, y] = mode(pixels[x, y], canvas.pixels[i, j]);
                }
            }
        }
    }

    public void AddPixels(Color[,] pixels, int startX, int startY)
    {
        AddPixels(new TextureCanvas(pixels), startX, startY);
    }

    public void AddPixels(Texture2D texture, int startX, int startY)
    {
        AddPixels(new TextureCanvas(texture), startX, startY);
    }

    public Color[] GetPixelData()
    {
        Color[] outputArray = new Color[width * height];

        int index = 0;
        for (int y = 0; y < pixels.GetLength(1); y += 1)
        {
            for (int x = 0; x < pixels.GetLength(0); x += 1)
            {
                outputArray[index] = pixels[x, y];
                index++;
            }
        }

        return outputArray;
    }

    public static Color[,] GeneratePixels(int width, int height, Color fill)
    {
        Color[,] pixels = new Color[width, height];

        for (int x = 0; x < pixels.GetLength(0); x += 1)
        {
            for (int y = 0; y < pixels.GetLength(1); y += 1)
            {
                pixels[x, y] = fill;
            }
        }

        return pixels;
    }

    public static Color[,] ConvertTo2DArray(Color[] inputArray, int rows)
    {
        int columns = inputArray.Length / rows;

        Color[,] outputArray = new Color[rows, columns];
        int index = 0;

        for (int j = 0; j < columns; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                outputArray[i, j] = inputArray[index];
                index++;
            }
        }

        return outputArray;
    }
}

static class CompositingMode
{
    public static Func<Color, Color, Color> Overlay = (Color c1, Color c2) => c2.a > 0 ? c2 : c1;

    // Todo: add simple alpha compositing https://www.w3.org/TR/compositing-1/#simplealphacompositing
    public static Func<Color, Color, Color> Blend = (Color c1, Color c2) => c1;

    static Func<Color, Color, Color> Additive = (Color c1, Color c2) => c1 + c2;
}