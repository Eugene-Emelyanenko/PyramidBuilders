using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Data
{
    public static readonly string DataKey = "Data";
    
    public static void SaveBlockDataList(List<BlockData> blockDataList)
    {
        // Преобразуем список BlockData в массив JSON
        string jsonData = JsonHelper.ToJson(blockDataList.ToArray());

        // Сохраняем JSON в PlayerPrefs
        PlayerPrefs.SetString(DataKey, jsonData);
        PlayerPrefs.Save();
    }

    public static List<BlockData> LoadBlockDataList()
    {
        List<BlockData> loadedBlockDataList = new List<BlockData>();

        if (PlayerPrefs.HasKey(DataKey))
        {
            // Получаем JSON из PlayerPrefs
            string jsonData = PlayerPrefs.GetString(DataKey);

            // Преобразуем JSON в список BlockData
            BlockData[] dataArray = JsonHelper.FromJson<BlockData>(jsonData);
            loadedBlockDataList.AddRange(dataArray);
        }

        return loadedBlockDataList;
    }
    
    public static Material LoadMaterial(string materialName)
    {
        string name = MaterialHelper.GetBaseMaterialName(materialName);
        // Формируем путь к материалу в папке Resources
        string path = "Materials/" + name;

        // Загружаем материал из папки Resources
        Material material = Resources.Load<Material>(path);
        if (material == null)
        {
            Debug.LogError("Failed to load material: " + name);
        }
        return material;
    }
}

// Вспомогательный класс для сериализации и десериализации массивов JSON
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}

public static class MaterialHelper
{
    public static string GetBaseMaterialName(string fullMaterialName)
    {
        string name = fullMaterialName;
        
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Full material name is null or empty.");
            return ""; // Возвращаем пустую строку
        }
        
        // Проверяем, содержит ли полное имя строку "(Instance)"
        int index1 = name.IndexOf("(Instance)");

        // Если строка найдена, удаляем ее
        if (index1 != -1)
        {
            name = name.Substring(0, index1).Trim(); // Удаляем "(Instance)" и пробелы вокруг него
        }
        else
        {
            name = name.Trim(); // Если строка не найдена, просто удаляем пробелы вокруг имени
        }

        // Проверяем, содержит ли полное имя строку "(UnityEngine.Material)"
        int index2 = fullMaterialName.IndexOf("(UnityEngine.Material)");

        // Если строка найдена, удаляем ее
        if (index2 != -1)
        {
            name = name.Substring(0, index2).Trim(); // Удаляем "(UnityEngine.Material)" и пробелы вокруг него
        }
        else
        {
            name = name.Trim(); // Если строка не найдена, просто удаляем пробелы вокруг имени
        }

        Debug.Log("Material saved with name: " + name);
        return name;
    }
}
