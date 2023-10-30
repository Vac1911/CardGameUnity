using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        public RawImage healthBarImage;

        public UICharacterInfo characterInfo;

        public Sprite pattern;
        public Sprite patternEmpty;

        Texture2D texture;

        public Character character
        {
            get { return characterInfo.character; }
        }

        void Start()
        {
            healthBarImage = GetComponent<RawImage>();
            UpdateHealthBar();
            character.OnTakeDamage += c => UpdateHealthBar();
        }

        void Update()
        {
            /*UpdateHealthBar();*/
        }

        public TextureCanvas CreateDividedImage(int width, int partCount)
        {
            // Calculate the width of each part
            int partWidth = width / partCount;
            var fillPattern = pattern.texture;
            var fillPatternEmpty = patternEmpty.texture;

            TextureCanvas canvas = new TextureCanvas(partWidth * partCount - 1 + fillPattern.width, 12);

            Color[] pixels = Enumerable.Repeat(Color.white, partWidth * partCount - 1).ToArray();

            // Fill each part with the fillPattern
            for (int p = 0; p < partCount; p++)
            {
                for (int x = 1; x < partWidth; x++)
                {
                    canvas.AddPixels(p < character.health ? fillPattern : fillPatternEmpty, p * partWidth + x, 0);
                }
            }

            return canvas;
        }

        public void SaveTexture()
        {
            var dirPath = Application.dataPath + "/Textures/Ui/Cache/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var fullPath = dirPath + "HealthBar_" + character.health.ToString() + "_" + character.maxHealth.ToString() + ".png";
            byte[] _bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(fullPath, _bytes);
            /*Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + fullPath);*/
        }

        public void UpdateHealthBar()
        {
            /*Debug.Log("UpdateHealthBar");*/
            TextureCanvas canvas = CreateDividedImage(100, character.maxHealth);
            texture = new Texture2D(canvas.width, canvas.height);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(canvas.GetPixelData());
            texture.Apply(false);
            healthBarImage.texture = texture;
            SaveTexture();
        }
    }
}