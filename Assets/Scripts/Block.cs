using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerClickHandler
{
    public bool IsPlaced { get; set; }
    private MeshRenderer meshRenderer;
    private BlockData blockData;
    private GameManager gameManager;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            Debug.LogError("MeshRenderer component is not assigned.");
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.LogError("GameManager component is not assigned.");
    }

    public void SetData()
    {
        blockData = new BlockData(transform.position, meshRenderer.material.name);
    }
    
    public void SetProperties(BlockData data)
    {
        blockData = data;
        transform.position = blockData.position;
        SetMaterial(Data.LoadMaterial(blockData.materialName));
    }

    public BlockData GetData()
    {
        if (blockData == null)
            Debug.LogError("Block data is null");
        return blockData;
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
        if(blockData != null)
            blockData.materialName = meshRenderer.material.name;
    }

    public void SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.SetCurrentBlock(this);
    }
}
