using CardGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICardEffectRenderer : MonoBehaviour
{
    public TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh ??= GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Render(Card cardData)
    {
        string text = "";
        foreach(var effect in cardData.effects)
        {
            text += effect.text + "\n";
        }
        textMesh.text = text;
    }
}
