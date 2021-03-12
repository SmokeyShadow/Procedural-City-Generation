using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PropsGenerator : MonoBehaviour
{
    private static PropsGenerator instance;

    public static PropsGenerator Instance
    {
        get => !instance ? FindObjectOfType<PropsGenerator>() : instance;
        set => instance = value;
    }

    public enum PropType : int
    {
        tree1=0,
        tree2,
        tree3,
        tree4,
        tree5,
        tree6,
        tree7,
        tree8,
        tree9,
        tree10,
        grass,
        bench,
        ground,
        cliff,
        cactus,
        tent,
        river,
        grassTall
    }

    public enum buildingType
    {
        building1=0,
        building2,
        building3,
        building4,
        building5,
        building6,
        building7,
        building8
    }

    public enum roadType
    {
        crossX=0,
        crossZ,
        intersection,
        crossXEnd,
        crossZEnd
    }

    public enum carType
    {
        car1 = 0,
        car2,
        car3,
        car4,
        car5,
        car6,
        car7,
        car8,
        car9,
        car10,
    }
    [SerializeField]
    public List<PropStruct> props;

    [SerializeField]
    public List<BuildingStruct> buildings;

    [SerializeField]
    public List<RoadStruct> roads;

    [SerializeField]
    public List<carStruct> cars;

    List<GameObject> instanceList = new List<GameObject>();

    public void GenerateProp(PropType type, Vector3 position, Quaternion rotation = default)
    {
        GameObject go = Instantiate(props[(int)type].propPrefab);
        go.transform.position = Vector3.Scale(position, props[(int)type].footPrint);
        go.transform.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
        instanceList.Add(go);
    }

    public void GenerateBuilding(buildingType type, Vector3 position, Quaternion rotation = default, float scaleY = default)
    {
        GameObject go = Instantiate(buildings[(int)type].buildingPrefab, Vector3.Scale(position, buildings[(int)type].footPrint), rotation);
        go.transform.localScale = new Vector3(go.transform.localScale.x, scaleY, go.transform.localScale.z);
        instanceList.Add(go);
    }

    public GameObject GenerateRoad(roadType type, Vector3 position, Quaternion rotation = default)
    {
        GameObject go = Instantiate(roads[(int)type].roadPrefab, Vector3.Scale(position, roads[(int)type].footPrint), rotation);
        go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
        instanceList.Add(go);
        return go;
    }

    public void GenerateCar(carType type, Vector3 position, Vector3 startPos, Vector3 endPos, Quaternion rotation = default)
    {
        GameObject go = Instantiate(cars[(int)type].carPrefab,position, rotation);
        go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
        go.GetComponent<Car>().Init(startPos, endPos);
        instanceList.Add(go);
    }

    public void ClearObjs()
    {
        foreach (var instance in instanceList)
        {         
            Destroy(instance);
        }
    }
    
    [System.Serializable]
    public struct RoadStruct
    {
        public roadType type;
        public GameObject roadPrefab;
        public Vector3 footPrint;
    }

    [System.Serializable]
    public struct BuildingStruct
    {
        public buildingType type;
        public GameObject buildingPrefab;
        public Vector3 footPrint;
    }

    [System.Serializable]
    public struct PropStruct
    {
        public PropType type;
        public GameObject propPrefab;
        public Vector3 footPrint;
    }

    [System.Serializable]
    public struct carStruct
    {
        public carType type;
        public GameObject carPrefab;
        public Vector3 footPrint;
    }

}
