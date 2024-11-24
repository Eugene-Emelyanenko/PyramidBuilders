using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoseMaterial : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Image currentMaterialImage;
    [SerializeField] private GameManager gameManager;

    [Header("Materials")]
    [SerializeField] private Material sandMaterial;
    [SerializeField] private Material bricksMaterial;
    [SerializeField] private Material roadMaterial;
    [SerializeField] private Material concreteMaterial;
    [SerializeField] private Material grassMaterial;
    [SerializeField] private Material raisedPatternMaterial;
    
    [Header("Sprites")]
    [SerializeField] private Sprite sandSprite;
    [SerializeField] private Sprite bricksSprite;
    [SerializeField] private Sprite roadSprite;
    [SerializeField] private Sprite concreteSprite;
    [SerializeField] private Sprite grassSprite;
    [SerializeField] private Sprite raisedPatternSprite;

    private void Start()
    {
        currentMaterialImage.sprite = sandSprite;
        gameManager.currentMaterial = sandMaterial;
    }

    private void SetMaterialAndImage(Material material, Sprite sprite)
    {
        gameManager.SetMaterial(material);
        currentMaterialImage.sprite = sprite;
    }

    public void SetSandMaterial()
    {
        SetMaterialAndImage(sandMaterial, sandSprite);
    }

    public void SetBricksMaterial()
    {
        SetMaterialAndImage(bricksMaterial, bricksSprite);
    }

    public void SetRoadMaterial()
    {
        SetMaterialAndImage(roadMaterial, roadSprite);
    }

    public void SetConcreteMaterial()
    {
        SetMaterialAndImage(concreteMaterial, concreteSprite);
    }

    public void SetGrassMaterial()
    {
        SetMaterialAndImage(grassMaterial, grassSprite);
    }

    public void SetRaisedPatternMaterial()
    {
        SetMaterialAndImage(raisedPatternMaterial, raisedPatternSprite);
    }
}
