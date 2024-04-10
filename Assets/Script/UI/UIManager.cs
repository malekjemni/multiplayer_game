using TGS;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance;

    public static Action OnCellHover;

    /// <summary>
    /// /Time
    /// </summary>
    public TextMeshProUGUI timeText;

    TerrainGridSystem tgs;


    public TextMeshProUGUI WoodStorage;
    public TextMeshProUGUI IronStorage;
    public TextMeshProUGUI MudStorage;
    public TextMeshProUGUI EnergieStorage;

    public TextMeshProUGUI WoodProduction;
    public TextMeshProUGUI IronProduction;
    public TextMeshProUGUI MudProduction;
    public TextMeshProUGUI EnergieProduction;


    public TextMeshProUGUI RegionText;
    public TextMeshProUGUI ProductivityText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI StateText;



    public TextMeshProUGUI cellDescription;
    public TextMeshProUGUI woodRequired;
    public TextMeshProUGUI IronRequired;
    public TextMeshProUGUI MudRequired;
    public TextMeshProUGUI EnergeyRequired;

    public TextMeshProUGUI Gem;

    public TextMeshProUGUI clokTime;

    public int timeElapsed = 0;


    public GameObject feedbackpos;
    public GameObject feedbackneg;


    public GameObject Clock;



    TerrainCellData lastClickedCell;

    public Button LevelUpButton;
    public Button SkipButton;
    public GameObject MaxReached;
    public GameObject MaxReachedText;


    private void Start()
    {
        Instance = this;
        if (loginmanager.LoadedPlayerData != null)
        {
            // Assuming ResourceManager has methods to add resources to storage
            ResourceManager.Instance.AddResourceStorage(ResourceType.Wood, loginmanager.LoadedPlayerData.storagewood);
            ResourceManager.Instance.AddResourceStorage(ResourceType.Iron, loginmanager.LoadedPlayerData.storageclay); // Assuming clay maps to iron in your model
            ResourceManager.Instance.AddResourceStorage(ResourceType.Mud, loginmanager.LoadedPlayerData.storagemud);
            //ResourceManager.Instance.AddResourceStorage(ResourceType.Energie, loginmanager.LoadedPlayerData.storageenergie); // You may need to adjust this for your energy resources

            // Update UI to reflect the combined total resources
            UpdateUiStorage();
        }


        tgs = TerrainGridSystem.instance;
        UpdateUiStorage();
        UpdateUiProduction();
        tgs.OnCellClick += (grid, cellIndex, buttonIndex) => lastClickedCell = updateCell(cellIndex);
        tgs.OnCellClick += (grid, cellIndex, buttonIndex) => UpdateCellDescription();
        LevelUpButton.onClick.AddListener(() => isUpgradeable(lastClickedCell));
        LevelUpButton.onClick.AddListener(() => upgrade(lastClickedCell));
        SkipButton.onClick.AddListener(() => SkipWaiting(lastClickedCell));

        Gem.text = ResourceManager.Instance.Gem.ToString();


    }


    private void Update()
    {
        //Debug.Log(lastClickedCell.state);
    }

    private void OnEnable()
    {
        TimeManager.OnMinuteChanged += UpdateTime;
        TimeManager.OnHourChanged += UpdateTime;
        ResourceManager.OnStorageChange += UpdateUiStorage;
        ResourceManager.OnProductionChange += UpdateUiProduction;


    }

    private void OnDisable()
    {

        TimeManager.OnMinuteChanged -= UpdateTime;
        TimeManager.OnHourChanged -= UpdateTime;
        ResourceManager.OnStorageChange -= UpdateUiStorage;
        ResourceManager.OnProductionChange -= UpdateUiProduction;

    }

    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";
    }

    private void UpdateUiStorage()
    {
      

        WoodStorage.text = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Wood).ToString();
        IronStorage.text = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Iron).ToString();
        MudStorage.text = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Mud).ToString();
        int energyValue = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeSolaire) + ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeWind) + ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeWater);
        EnergieStorage.text = energyValue.ToString();
        UpdateStorageAttributesOnServer();


    }

    private void UpdateUiProduction()
    {
        WoodProduction.text = ResourceManager.Instance.GetResourceAmountProduction(ResourceType.Wood).ToString();
        IronProduction.text = ResourceManager.Instance.GetResourceAmountProduction(ResourceType.Iron).ToString();
        MudProduction.text = ResourceManager.Instance.GetResourceAmountProduction(ResourceType.Mud).ToString();
        int energyValue = ResourceManager.Instance.GetResourceAmountProduction(ResourceType.ETypeSolaire) + ResourceManager.Instance.GetResourceAmountProduction(ResourceType.ETypeWind) + ResourceManager.Instance.GetResourceAmountProduction(ResourceType.ETypeWater);
        EnergieProduction.text = energyValue.ToString();
        // Debug.Log(TerrainGridManager.Instance.GetProductionSum(ResourceType.Wood));

        //WoodProduction.text = TerrainGridManager.Instance.GetProductionSum(ResourceType.Wood).ToString();
        //IronProduction.text = TerrainGridManager.Instance.GetProductionSum(ResourceType.Iron).ToString(); ;
        //MudProduction.text = TerrainGridManager.Instance.GetProductionSum(ResourceType.Mud).ToString();
        //int energyValue = TerrainGridManager.Instance.GetProductionSum(ResourceType.ETypeSolaire) + TerrainGridManager.Instance.GetProductionSum(ResourceType.ETypeWind) + TerrainGridManager.Instance.GetProductionSum(ResourceType.ETypeWater);
        //EnergieProduction.text = energyValue.ToString();

    }


    private TerrainCellData updateCell(int index)
    {

        TerrainCellData cell = TerrainGridManager.Instance.GetCellData(index);

        switch (cell.regionData.resourceType)
        {
            case ResourceType.ETypeSolaire:
                RegionText.text = " Solar region";
                break;
            case ResourceType.Iron:
                RegionText.text = "Iron region";
                break;
            case ResourceType.ETypeWater:
                RegionText.text = " Water region";
                break;
            case ResourceType.Mud:
                RegionText.text = "Mud region";
                break;
            case ResourceType.Wood:

                RegionText.text = "Wood region";
                break;
            case ResourceType.ETypeWind:
                RegionText.text = "Wind region";
                break;
            default:
                RegionText.text = "Unknown region type";
                break;
        }
        ProductivityText.text = (cell.regionData.productionRateBase * cell.level).ToString();

        LevelText.text = cell.level.ToString();


        if (cell.state) StateText.text = "Active";
        else StateText.text = "Inactive";

        return cell;
    }



    void UpdateCellDescription()
    {
        if (lastClickedCell.level == 5)
        {
            MaxReached.SetActive(false);
            MaxReachedText.SetActive(true);
        }
        else
        {
            MaxReached.SetActive(true);
            MaxReachedText.SetActive(false);
        }

        updateCell(lastClickedCell.id);
        cellDescription.text = lastClickedCell.regionData.RegionDescription.ToString();
        woodRequired.text = lastClickedCell.materialsNeeded[0].ToString();
        IronRequired.text = lastClickedCell.materialsNeeded[1].ToString();
        MudRequired.text = lastClickedCell.materialsNeeded[2].ToString();
        EnergeyRequired.text = lastClickedCell.materialsNeeded[3].ToString();



    }



    public bool isUpgradeable(TerrainCellData cell)
    {
        int woodReserve = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Wood);
        int ironReserve = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Iron);
        int mudReserve = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Mud);
        int energieReserve = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeSolaire);
        if (cell.materialsNeeded[0] < woodReserve && cell.materialsNeeded[1] < ironReserve && cell.materialsNeeded[2] < mudReserve && cell.materialsNeeded[3] < energieReserve)
        {
            Debug.Log("true");
            return true;
        }

        else
        {
            Debug.Log("false");
            return false;
        }

    }

    public void UpgradeRoutine(TerrainCellData cell)
    {
        ResourceManager.Instance.SubtractResourceStorage(ResourceType.Wood, cell.materialsNeeded[0]);
        ResourceManager.Instance.SubtractResourceStorage(ResourceType.Iron, cell.materialsNeeded[1]);
        ResourceManager.Instance.SubtractResourceStorage(ResourceType.Mud, cell.materialsNeeded[2]);
        ResourceManager.Instance.SubtractResourceStorage(ResourceType.ETypeSolaire, cell.materialsNeeded[3]);
        cell.CalculateMaterialsNeededForNextUpgrade();
        cell.level++;
        StartCoroutine(feedbackOn(feedbackpos));
        if (cell.regionData.resourceType == ResourceType.Wood || cell.regionData.resourceType == ResourceType.Mud || cell.regionData.resourceType == ResourceType.Iron)
        {
            ResourceManager.Instance.AddResourceRate(cell.regionData.resourceType, cell.regionData.productionRateBase);
        }
        else
        {
            

            ResourceManager.Instance.AddResourceRate(ResourceType.ETypeSolaire, cell.regionData.productionRateBase);
        }
        UpdateCellDescription();
        UpdateUiStorage();
        updateCell(cell.id);
    }

    public void upgrade(TerrainCellData cell)
    {
        if (cell.level == 4)
        {
            UpgradeRoutine(cell);
            TerrainGridManager.Instance.OpenNewCell(cell);
            TerrainGridManager.Instance.ActiveCellsInRegion(cell.regionData.index);
        }

        if (isUpgradeable(cell) && cell.state && cell.level <= 5 && cell.canUpgrade)
        {

            UpgradeRoutine(cell);
            cell.canUpgrade = false;
            StartCoroutine(TimerOn(cell));
        }
        else
        {
            StartCoroutine(feedbackOn(feedbackneg));
        }

    }

    IEnumerator feedbackOn(GameObject go)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(2f);
        go.SetActive(false);
    }




    IEnumerator TimerOn(TerrainCellData cell)
    {
        int timetowait = cell.level * 30;
        LevelUpButton.gameObject.SetActive(false);
        Clock.SetActive(true);

        while (timeElapsed < timetowait)
        {
            yield return new WaitForSeconds(1);
            timeElapsed++;

            int remainingSeconds = timetowait - timeElapsed;
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;

            if (remainingSeconds >= 60)
            {
                clokTime.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
            else
            {
                clokTime.text = seconds.ToString("00:00");
            }
        }

        timeElapsed = 0;
        cell.canUpgrade = true;
        Clock.SetActive(false);
        LevelUpButton.gameObject.SetActive(true);
    }

    public void SkipWaiting(TerrainCellData cell)
    {
        if (!cell.canUpgrade)
        {
            StopCoroutine("TimerOn"); // Stop the coroutine
            timeElapsed = cell.level * 30; // Set timeElapsed to its maximum value
            cell.canUpgrade = true; // Set canUpgrade to true to enable upgrading
            Clock.SetActive(false); // Hide the clock
            LevelUpButton.gameObject.SetActive(true); // Show the LevelUpButton
            ResourceManager.Instance.Gem -= 50;
            UpdateGoldOnServer(-50);// Assuming you deduct 50 gems for skipping
            Gem.text = ResourceManager.Instance.Gem.ToString();

            // Update gold on server
            // Assuming you want to deduct 50 gems
        }
    }

    public void UpdateStorageAttributesOnServer()
    {
        // Ensure you have a method to get the username correctly
        string username = loginmanager.LoadedPlayerData.username;

        StorageUpdateData data = new StorageUpdateData
        {
            storagewood = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Wood),
            storagemud = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Mud),
            storageclay = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.Iron), // or your actual mapping
            storageenergie = ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeSolaire)
                           + ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeWind)
                           + ResourceManager.Instance.GetResourceAmountStorage(ResourceType.ETypeWater)
        };

        StartCoroutine(UpdateStorageAttributesCoroutine(username, JsonUtility.ToJson(data)));
    }

    IEnumerator UpdateStorageAttributesCoroutine(string username, string jsonData)
    {
        string url = $"http://localhost:9090/players/{username}";
        UnityWebRequest request = UnityWebRequest.Put(url, jsonData);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Storage attributes updated successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to update storage attributes: " + request.error);
        }
    }
    public void UpdateGoldOnServer(int goldChange)
{
    string username = loginmanager.LoadedPlayerData.username; // Ensure this is correctly fetching the username
    StartCoroutine(UpdateGoldCoroutine(username, goldChange));
}

IEnumerator UpdateGoldCoroutine(string username, int goldChange)
{
    string url = $"http://localhost:9090/players/{username}";
    var data = new {
        gold = loginmanager.LoadedPlayerData.gold - goldChange
    };
    string jsonData = JsonUtility.ToJson(data);

    UnityWebRequest request = UnityWebRequest.Put(url, jsonData);
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Gold updated successfully on server.");
    }
    else
    {
        Debug.LogError($"Failed to update gold on server: {request.error}");
    }
}




}
[System.Serializable]
public class StorageUpdateData
{
    public int storagewood;
    public int storagemud;
    public int storageclay; // Adjust names as per your backend model.
    public int storageenergie;
}