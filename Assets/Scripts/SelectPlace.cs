using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectPlace : MonoBehaviour
{
    [SerializeField] private bool canWork = false;

    private bool isSomethingBuilding = false;

    private string structureType = "";

    [SerializeField] private GameObject gasMinesParent;
    [SerializeField] private GameObject mineralMinesParent;
    [SerializeField] private GameObject gasStoragesParent;
    [SerializeField] private GameObject mineralStoragesParent;
    [SerializeField] private GameObject cancelBuilding;

    private GameObject[] gasMines = new GameObject[160];
    private GameObject[] mineralMines = new GameObject[160];
    private GameObject[] gasStorages = new GameObject[160];
    private GameObject[] mineralStorages = new GameObject[160];

    private int gasMinePoolNumber = 0;
    private int mineralMinePoolNumber = 0;
    private int gasStoragePoolNumber = 0;
    private int mineralStoragePoolNumber = 0;
    private int structureCost = 0;

    [SerializeField] private Text gasValue;
    [SerializeField] private Text mineralValue;

    [SerializeField] private float gasMineBuildTime;
    [SerializeField] private float mineralMineBuildTime;
    [SerializeField] private float gasStorageBuildTime;
    [SerializeField] private float mineralStorageBuildTime;


    private void OnEnable()
    {
        StartCoroutine(Wait(0.3f));
    }
    // Update is called once per frame
    private void Start()// заполняем массив для пула объектов | filling in the array for the object pool
    {
        for (int i = 0; i < gasMinesParent.transform.childCount; i++)
        {
            gasMines[i] = gasMinesParent.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < mineralMinesParent.transform.childCount; i++)
        {
            mineralMines[i] = mineralMinesParent.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < gasStoragesParent.transform.childCount; i++)
        {
            gasStorages[i] = gasStoragesParent.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < mineralStoragesParent.transform.childCount; i++)
        {
            mineralStorages[i] = mineralStoragesParent.transform.GetChild(i).gameObject;
        }
        
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && canWork)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Placement"))
                {
                    if (hit.collider.gameObject.GetComponent<PlacementControl>().GetStatus())// место строительства свободно | building placement is free
                    {
                        MoveToPlace(structureType, hit.collider.gameObject);
                    }
                }
            }
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canWork = true;
    }

    void MoveToPlace(string buildingType, GameObject buildPlace)
    {
        isSomethingBuilding = true;
        switch (buildingType)
        {
            case "gasMine":
                gasMines[gasMinePoolNumber].transform.position = buildPlace.transform.position;// перемещаем объект на место строительства | move object to building place
                StartCoroutine(Building(gasMineBuildTime, gasMines[gasMinePoolNumber]));// строим | building
                gasMinePoolNumber++;// берем следующий индекс в пуле объектов | take next index in object pool
                gasValue.text = (Convert.ToInt32(gasValue.text) - structureCost).ToString();// вычитаем стоимость | subtract the cost
                break;
            case "mineralMine":
                mineralMines[mineralMinePoolNumber].transform.position = buildPlace.transform.position;
                StartCoroutine(Building(mineralMineBuildTime, mineralMines[mineralMinePoolNumber]));
                mineralMinePoolNumber++;
                mineralValue.text = (Convert.ToInt32(mineralValue.text) - structureCost).ToString();
                break;
            case "gasStorage":
                gasStorages[gasStoragePoolNumber].transform.position = buildPlace.transform.position;
                StartCoroutine(Building(gasStorageBuildTime, gasStorages[gasStoragePoolNumber]));
                gasStoragePoolNumber++;
                gasValue.text = (Convert.ToInt32(gasValue.text) - structureCost).ToString();
                break;
            case "mineralStorage":
                mineralStorages[mineralStoragePoolNumber].transform.position = buildPlace.transform.position;
                StartCoroutine(Building(mineralStorageBuildTime, mineralStorages[mineralStoragePoolNumber]));
                mineralStoragePoolNumber++;
                mineralValue.text = (Convert.ToInt32(mineralValue.text) - structureCost).ToString();
                break;
            default:
                Debug.Log("building type is empty");
                break;
        }
        buildPlace.GetComponent<PlacementControl>().Occupied();// плитка занята | place is occupied
        cancelBuilding.SetActive(false);
        canWork = false;
        
    }

    public void SetStructureTypeAndCost(string type, int cost)
    {
        structureType = type;
        structureCost = cost;
    }

    public void StopWork() => canWork = false;

    private IEnumerator Building(float seconds, GameObject structure)
    {
        yield return new WaitForSeconds(seconds);
        structure.SetActive(true);
        isSomethingBuilding = false;
        gameObject.SetActive(false);
    }

    public bool GetBuildingStatus() => isSomethingBuilding;
}
