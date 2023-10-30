using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementMarker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3Int position;
    public Sprite spriteDefault;
    public Sprite spriteSelected;
    protected SpriteRenderer spriteRenderer;
    protected Func<bool> response;
    protected Func<bool> onHover;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ShowDefault();
    }

    public MovementMarker SetResponse(Func<bool> _response)
    {
        this.response = _response;
        return this;
    }

    public MovementMarker SetOnHover(Func<bool> _onHover)
    {
        this.onHover = _onHover;
        return this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (response())
        {
            Destroy(this);
        }
    }

    //Detect if the Cursor starts to pass over the Tile
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        onHover();
    }

    //Detect when Cursor leaves the Tile
    public void OnPointerExit(PointerEventData pointerEventData)
    {
    }

    public void ShowDefault()
    {
        spriteRenderer.sprite = spriteDefault;
    }

    public void ShowSelected()
    {
        spriteRenderer.sprite = spriteSelected;
    }
}
