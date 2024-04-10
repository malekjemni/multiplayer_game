using System.Collections;
using TGS;
using UnityEngine;



public class TerrainGridManager : MonoBehaviour
{
    public Material LockMaterial;
    public TerrainGridSystem tgs;
    public RegionData[] regionDataList; // List of RegionData scriptable objects
    public  TerrainCellData[,] cellDataGrid;//matrice mtaa les cellules data
    public static TerrainGridManager Instance;//



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        InitializeGrid();
        StartCoroutine("InitiateStartingGrid");
     




    }

 

    public void InitializeGrid()
    {
        
        cellDataGrid = new TerrainCellData[tgs.columnCount, tgs.rowCount];

        for (int x = 0; x < tgs.columnCount; x++)
        {
            for (int y = 0; y < tgs.rowCount; y++)
            {
               
                cellDataGrid[x, y] = GenerateCellData(x,y);
              //  tgs.CellSetColor(cellDataGrid[x, y].id,UnityEngine.Color.gray);
              //  tgs.CellFadeOut(cellDataGrid[x, y].id, UnityEngine.Color.gray, 10, 10);
                tgs.CellSetMaterial(cellDataGrid[x,y].id, LockMaterial);
              // tgs.CellSetVisible(cellDataGrid[x, y].id, true);  


            }
        }
    }


    public  TerrainCellData GetCellData(int index)
    {
        for (int x = 0; x < tgs.columnCount; x++)
        {
            for (int y = 0; y < tgs.rowCount; y++)
            {

                if (cellDataGrid[x, y].id == index)
                {
                  
                    return cellDataGrid[x, y];
                }


            }
        }
return null;
    }




    private IEnumerator InitiateStartingGrid()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 6; i++)
        {
            tgs.CellSetColor(tgs.TerritoryGetCells(i)[0], UnityEngine.Color.clear);
            tgs.CellFlash(tgs.TerritoryGetCells(i)[0], UnityEngine.Color.yellow, 2, 3);

            TerrainCellData cell= GetCellData(tgs.TerritoryGetCells(i)[0].index);
            cell.state = true;
            cell.level = 1;
           // cell.regionData.activeCells++;
           // tgs.CellFadeOut(tgs.TerritoryGetCells(i)[0].index, UnityEngine.Color.yellow, 10, 10);

        }
    }

    public int ActiveCellsInRegion(int index)
    {
        int counter = 0;
        for (int i = 0; i < tgs.TerritoryGetCells(index).Count; i++)
        {
            if (GetCellData(tgs.TerritoryGetCells(index)[i].index).state == true) 
            {
                counter++;
            };
        }
        return counter;
    }



    public void OpenNewCell(TerrainCellData cellData)
    {
        int regionIndex = cellData.regionData.index;



        tgs.CellSetColor(tgs.TerritoryGetCells(regionIndex)[ActiveCellsInRegion(regionIndex)], Color.clear);
        tgs.CellFlash(tgs.TerritoryGetCells(regionIndex)[ActiveCellsInRegion(regionIndex)], Color.yellow, 2, 3);
       
        GetCellData(tgs.TerritoryGetCells(regionIndex)[ActiveCellsInRegion(regionIndex)].index).level = 1;

        GetCellData(tgs.TerritoryGetCells(regionIndex)[ActiveCellsInRegion(regionIndex)].index).state = true;

        //Debug.Log("activecells" + cellData.regionData.activeCells + "region index " + cellData.regionData.index);

        Debug.Log(GetCellData(tgs.TerritoryGetCells(regionIndex)[ActiveCellsInRegion(regionIndex)].index).level);   
    }


    private TerrainCellData GenerateCellData(int x, int y)
    {
    TerrainCellData data = new TerrainCellData();


        data.id = tgs.CellGetIndex(x, y, true);
       
        data.state = false;
        int territoryIndex = tgs.CellGetTerritoryIndex(data.id);


        if (territoryIndex >= 0 && territoryIndex < regionDataList.Length)
        {
            data.regionData = regionDataList[territoryIndex];
           
        }
        else
        {
            Debug.LogError("Territory index out of bounds or regionDataList is not properly initialized.");
            data.regionData = null; // Or handle the error in your desired way
        }

        // Initialize level
        data.level = 0;
        data.CalculateMaterialsNeededForNextUpgrade();
        return data;
    }





    public int GetProductionSum(ResourceType type)
    {
        int sum=0;
        for (int x = 0; x < tgs.columnCount; x++)
        {
            for (int y = 0; y < tgs.rowCount; y++)
            {

                if (cellDataGrid[x, y].state == true&& cellDataGrid[x, y].regionData.resourceType==type)
                {
                    sum += cellDataGrid[x, y].regionData.productionRateBase * cellDataGrid[x, y].regionData.levelUpMultiplier;
                    //Debug.Log("type "+ type+" "+sum);

                }
            }
        }

        return sum; 
    }

   



}
