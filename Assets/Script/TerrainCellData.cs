using System;
using System.Diagnostics;
using UnityEngine;
[System.Serializable]
public class TerrainCellData
{
    public RegionData regionData;
    public bool state;
    public int level;
  
    public int id;
    public int[] materialsNeeded;
    public bool canUpgrade=true;
  


    public TerrainCellData()
    {
      
        materialsNeeded = new int[4];
       
    }

    // Method to calculate the materials needed for the next upgrade
    public int[] CalculateMaterialsNeededForNextUpgrade()
    {
        // Example: Define materials needed for each level
      if(level==0)
        {
           
            switch (regionData.resourceType)
            {
                case ResourceType.ETypeSolaire:
                    materialsNeeded[0] = 50; // Wood
                    materialsNeeded[1] = 30; // Iron
                    materialsNeeded[2] = 200; // Mud
                    materialsNeeded[3] = 250; // Energy

                    break;
                case ResourceType.Iron:
                    materialsNeeded[0] = 50; // Wood
                    materialsNeeded[1] = 70; // Iron
                    materialsNeeded[2] = 200; // Mud
                    materialsNeeded[3] = 100; // Energy

                    break;
                case ResourceType.ETypeWater:
                    materialsNeeded[0] = 50; // Wood
                    materialsNeeded[1] = 30; // Iron
                    materialsNeeded[2] = 200; // Mud
                    materialsNeeded[3] = 250; // Energy

                    break;
                case ResourceType.Mud:
                    materialsNeeded[0] = 50; // Wood
                    materialsNeeded[1] = 30; // Iron
                    materialsNeeded[2] = 800; // Mud
                    materialsNeeded[3] = 100; // Energy


                    break;
                case ResourceType.Wood:
                    materialsNeeded[0] = 200; // Wood
                    materialsNeeded[1] = 30; // Iron
                    materialsNeeded[2] = 200; // Mud
                    materialsNeeded[3] = 100; // Energy

                    break;
                case ResourceType.ETypeWind:
                    materialsNeeded[0] = 50; // Wood
                    materialsNeeded[1] = 30; // Iron
                    materialsNeeded[2] = 200; // Mud
                    materialsNeeded[3] = 250; // Energy


                    break;
              
            }
        }

    
        else
        {
            for (int i = 0; i < materialsNeeded.Length; i++)
            {
               

                switch (this.regionData.resourceType)
                {
                    case ResourceType.ETypeSolaire:
                        materialsNeeded[i] *= level + 1;
                        materialsNeeded[3] += 10;

                        break;
                    case ResourceType.Iron:
                        materialsNeeded[i] *= level + 1;
                        materialsNeeded[1] += 10;

                        break;
                    case ResourceType.ETypeWater:
                        materialsNeeded[i] *= level + 1;
                        materialsNeeded[3] += 10;

                        break;
                    case ResourceType.Mud:
                        materialsNeeded[i] *= level + 1;
                        materialsNeeded[2] += 10;


                        break;
                    case ResourceType.Wood:
                        materialsNeeded[i] *= level + 1;
                        materialsNeeded[0] += 10;

                        break;
                    case ResourceType.ETypeWind:
                        materialsNeeded[i] *= level + 1;
                        materialsNeeded[3] += 10;


                        break;
        
                }

            }
        }
        return materialsNeeded;
    }





}
