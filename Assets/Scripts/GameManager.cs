using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab; // Префаб куба
    public int gridWidth = 20; // Ширина сетки
    public int gridHeight = 20; // Высота сетки
    public int gridDepth = 20; // Глубина сетки
    public int moveSpeed = 1; // Скорость перемещения кубов
    public float fallSpeed = 3f; // Скорость падения кубов

    public Color selectedColor = Color.white;
    public Color defaultColor = Color.white;

    private Block[,,] grid; // Трехмерная сетка игры
    private Block lastPlacedBlock;
    private Block notPlacedBlock;
    private Block currentBlock; // Текущий куб
    private float fallTimer; // Таймер падения
    [NonSerialized] public Material currentMaterial;
    private List<BlockData> blockDatas = new List<BlockData>();
    private bool isDipDownPressed = false;

    private void Awake()
    {
        Time.timeScale = 1;
        grid = new Block[gridWidth + 1, gridHeight + 1, gridDepth + 1];
        blockDatas = Data.LoadBlockDataList();
        PlaceSavedBlocks();
    }

    private void Start()
    {
        SpawnCube();
    }

    private void Update()
    {
        fallTimer += Time.deltaTime;
        if (isDipDownPressed && fallTimer >= fallSpeed)
        {
            FallCube();
            fallTimer = 0f;
        }
    }

    // Метод для создания нового куба
    private void SpawnCube()
    {
        GameObject blockObject = Instantiate(cubePrefab, new Vector3(gridWidth / 2f, gridHeight - 1, gridDepth / 2f), Quaternion.identity);
        Block blockComponent = blockObject.GetComponent<Block>();
        notPlacedBlock = blockComponent;
        SetCurrentBlock(blockComponent);
        SetMaterial(currentMaterial);
    }

    public void SetMaterial(Material material)
    {
        currentMaterial = material;
        if (currentBlock.IsPlaced)
        {
            foreach (BlockData data in blockDatas)
            {
                if (data.position == currentBlock.GetData().position)
                {
                    data.materialName = MaterialHelper.GetBaseMaterialName(currentMaterial.name);
                }
            }
            Data.SaveBlockDataList(blockDatas);
        }
        currentBlock.SetMaterial(currentMaterial);
        currentBlock.SetColor(selectedColor);
    }

    public void SetCurrentBlock(Block block)
    {
        if(currentBlock != null)
            currentBlock.SetColor(defaultColor);
        currentBlock = block;
        currentBlock.SetColor(selectedColor);
    }

    // Метод для падения куба
    private void FallCube()
    {
        // Проверяем, может ли куб двигаться вниз
        if (CanMove(Vector3.down))
        {
            currentBlock.transform.position += Vector3.down;
        }
        else
        {
            if (currentBlock.transform.position.y < gridHeight - 3)
            {
                // Если куб не может двигаться вниз, то добавляем его в сетку и создаем новый куб
                AddCubeToGrid(currentBlock, true);
                SpawnCube();
            }
        }
    }

    // Метод для проверки возможности движения куба в заданном направлении
    private bool CanMove(Vector3 direction)
    {
        // Получаем позицию текущего куба
        Vector3 currentPosition = currentBlock.transform.position;

        // Вычисляем следующую позицию с учетом направления движения
        Vector3 nextPosition = currentPosition + direction;

        // Проверяем, находится ли следующая позиция в пределах сетки и свободна ли она
        int x = Mathf.FloorToInt(nextPosition.x);
        int y = Mathf.FloorToInt(nextPosition.y);
        int z = Mathf.FloorToInt(nextPosition.z);

        // Проверяем, свободно ли место для перемещения
        for (int i = x; i < x + 2; i++) // Учитываем размер куба по каждой оси
        {
            for (int j = y; j < y + 2; j++)
            {
                for (int k = z; k < z + 2; k++)
                {
                    if (i < 0 || i > gridWidth ||
                        j < 0 || j > gridHeight ||
                        k < 0 || k > gridDepth ||
                        grid[i, j, k] != null)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    // Метод для добавления куба в сетку
    private void AddCubeToGrid(Block block, bool saveBlock)
    {
        // Проверяем, чтобы все ячейки, которые занимает куб, находились в допустимом диапазоне
        for (int i = 0; i < 2; i++) // Учитываем размер куба по оси X
        {
            for (int j = 0; j < 2; j++) // Учитываем размер куба по оси Y
            {
                for (int k = 0; k < 2; k++) // Учитываем размер куба по оси Z
                {
                    int x = Mathf.FloorToInt(block.transform.position.x) + i;
                    int y = Mathf.FloorToInt(block.transform.position.y) + j;
                    int z = Mathf.FloorToInt(block.transform.position.z) + k;

                    if (x < 0 || x > gridWidth ||
                        y < 0 || y > gridHeight ||
                        z < 0 || z > gridDepth)
                    {
                        Debug.LogError($"Attempted to add cube outside grid boundaries. X: {x} Y:{y} Z:{z}");
                        return;
                    }
                }
            }
        }

        // Добавляем куб в сетку
        for (int i = 0; i < 2; i++) // Учитываем размер куба по оси X
        {
            for (int j = 0; j < 2; j++) // Учитываем размер куба по оси Y
            {
                for (int k = 0; k < 2; k++) // Учитываем размер куба по оси Z
                {
                    int x = Mathf.FloorToInt(block.transform.position.x) + i;
                    int y = Mathf.FloorToInt(block.transform.position.y) + j;
                    int z = Mathf.FloorToInt(block.transform.position.z) + k;

                    grid[x, y, z] = block;
                }
            }
        }
        
        block.IsPlaced = true;
        block.SetData();

        if (saveBlock)
        {
            blockDatas.Add(block.GetData());
            Data.SaveBlockDataList(blockDatas);
        }
        
        SetLastPlacedBlock();
    }

    private void PlaceSavedBlocks()
    {
        foreach (BlockData blockData in blockDatas)
        {
            GameObject blockObject = Instantiate(cubePrefab, blockData.position, Quaternion.identity);
            Block block = blockObject.GetComponent<Block>();
            block.SetProperties(blockData);
            block.IsPlaced = true;
            AddCubeToGrid(block, false);
        }
    }

    // Методы для управления кубом с помощью кнопок на Canvas
    public void MoveRight()
    {
        if (CanMove(Vector3.left) && !currentBlock.IsPlaced)
        {
            currentBlock.transform.position += Vector3.left * moveSpeed;
        }
    }

    public void MoveLeft()
    {
        if (CanMove(Vector3.right) && !currentBlock.IsPlaced)
        {
            currentBlock.transform.position += Vector3.right * moveSpeed;
        }
    }

    public void MoveForward()
    {
        if (CanMove(Vector3.forward) && !currentBlock.IsPlaced)
        {
            currentBlock.transform.position += Vector3.forward * moveSpeed;
        }
    }

    public void MoveBackward()
    {
        if (CanMove(Vector3.back) && !currentBlock.IsPlaced)
        {
            currentBlock.transform.position += Vector3.back * moveSpeed;
        }
    }

    public void DipDownPressed()
    {
        if(!currentBlock.IsPlaced)
            isDipDownPressed = true;
    }
    
    public void DipDownReleased()
    {
        if(!currentBlock.IsPlaced)
            isDipDownPressed = false;
    }

    public void PauseOn()
    {
        CameraController.CanInput = false;
        Time.timeScale = 0;
    }

    public void PauseOff()
    {
        CameraController.CanInput = true;
        Time.timeScale = 1;
    }

    public void DeleteCurrentBlock()
    {
        if (currentBlock.IsPlaced)
        {
            for (int i = blockDatas.Count - 1; i >= 0; i--)
            {
                if (blockDatas[i].position == currentBlock.GetData().position)
                {
                    blockDatas.RemoveAt(i);
                }
            }
            Data.SaveBlockDataList(blockDatas);
        }
        
        if (currentBlock == notPlacedBlock)
        {
            DestroyImmediate(currentBlock.gameObject);
            SpawnCube();
        }
        else
        {
            DestroyImmediate(currentBlock.gameObject);
            SetCurrentBlock(notPlacedBlock);
        }
        
        SetLastPlacedBlock();
    }

    public void RemoveLastPlacedBlock()
    {
        BlockData data = null;
        
        for (int i = 0; i < blockDatas.Count; i++)
        {
            data = blockDatas[i];
        }

        if (lastPlacedBlock != null)
        {
            blockDatas.Remove(data);
            if (lastPlacedBlock.gameObject == currentBlock.gameObject)
            {
                SetCurrentBlock(notPlacedBlock);
            }
            DestroyImmediate(lastPlacedBlock.gameObject);
        }
        
        Data.SaveBlockDataList(blockDatas);
        
        SetLastPlacedBlock();
    }

    private Block FindLastPlacedBlock(BlockData data)
    {
        Block blockComponent = null;
        
        foreach (Block block in grid)
        {
            if (block != null && block.IsPlaced)
            {
                if (data != null && block.GetData().position == data.position)
                {
                    blockComponent = block;
                }
            }
        }

        return blockComponent;
    }

    private void SetLastPlacedBlock()
    {
        if (blockDatas.Count > 0)
            lastPlacedBlock = FindLastPlacedBlock(blockDatas[blockDatas.Count - 1]);
        else
            lastPlacedBlock = null;
    }
}
