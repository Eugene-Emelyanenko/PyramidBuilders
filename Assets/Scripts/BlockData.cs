using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockData
{
    public Vector3 position;
    public string materialName; // Строка для хранения имени материала

    public BlockData(Vector3 position, string materialName)
    {
        this.position = position;
        this.materialName = MaterialHelper.GetBaseMaterialName(materialName); // Сохраняем имя материала
    }
}
