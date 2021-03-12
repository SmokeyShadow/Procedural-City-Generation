using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    private List<GameObject> intersections = new List<GameObject>();
    List<List<Vector3>> crossXintersections = new List<List<Vector3>>();
    private static MeshGenerator _instance;
    public static MeshGenerator Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MeshGenerator();
            return _instance;
        }
    }

    public void DestroyMap()
    {
        crossXintersections.Clear();
        intersections = new List<GameObject>();
        PropsGenerator.Instance.ClearObjs();
    }

    public MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        //set plain to center
        float leftTopX = (width - 1) / -2f;
        float leftTopZ = (height - 1) / 2f;
        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(leftTopX + x,
                  ((heightMap[x, y] < 0.3f) ? heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier : 0), leftTopZ - y);
                meshData.UVs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                meshData.normals[vertexIndex] = Vector3.up;
                vertexIndex++;
            }
        }
        return meshData;
    }

    public void GenerateCity(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //set plain to center
        float leftTopX = (width - 1) / -2f;
        float leftTopZ = (height - 1) / 2f;

        //first check for roads and assign negative values when they are road
        //iterates on depths
        int x = Random.Range(0, 2);
        for (int n = 0; n < 10; n++)
        {
            for (int i = 0; i < width; i++)
            {

                if (heightMap[i, x] <= 0.2f)
                    continue;
                heightMap[i, x] = -1;
            }
            x += Random.Range(2, 10);
            if (x >= height)
                break;
        }

        //first check for roads and assign negative values when they are road
        //iterates on depths
        int y = Random.Range(0, 2);
        for (int n = 0; n < 10; n++)
        {
            for (int i = 0; i < height; i++)
            {
                if (heightMap[y, i] <= 0.2f && heightMap[y, i] >= 0)
                    continue;
                if (heightMap[y, i] < 0)
                    heightMap[y, i] = -3;
                else
                    heightMap[y, i] = -2;
            }
            y += Random.Range(2, 10);
            if (y >= width)
                break;
        }
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < height; i++)
        {
            if (i >= 0)
            {
                if (list.Count > 0)
                {
                    crossXintersections.Add(new List<Vector3>(list));
                    list.Clear();
                }
            }
            for (int j = 0; j < width; j++)
            { 
                Vector2 pos = new Vector2(leftTopX + j, leftTopZ - i);//building footprint

                if (heightMap[j, i] == -3)
                {
                    GameObject go = PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.intersection,
                         new Vector3(pos.x, 0.2f, pos.y));
                    list.Add(new Vector3(pos.x, 0.2f, pos.y));
                    intersections.Add(go);
                }
                else if (heightMap[j, i] == -2)
                {
                    if (i + 1 < heightMap.GetLength(1) && heightMap[j, i + 1] > 0 || i == heightMap.GetLength(1) - 1)
                        PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.crossZEnd,
                    new Vector3(pos.x, 0.2f, pos.y), new Quaternion(0, 0.7071f, 0, 0.7071f));
                    else if (i - 1 >= 0 && heightMap[j, i - 1] > 0 || i == 0)
                        PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.crossZEnd,
                        new Vector3(pos.x, 0.2f, pos.y), new Quaternion(0, -0.7071f, 0, 0.7071f));
                    else
                        PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.crossZ,
                         new Vector3(pos.x, 0.2f, pos.y), new Quaternion(0, 0.7071f, 0, 0.7071f));
                }
                else if (heightMap[j, i] == -1)
                {
                    if (j + 1 < heightMap.GetLength(0) && heightMap[j + 1, i] > 0 || j == heightMap.GetLength(0) - 1)
                        PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.crossXEnd,
                    new Vector3(pos.x, 0.2f, pos.y));
                    else if (j - 1 >= 0 && heightMap[j - 1, i] > 0 || j == 0)
                        PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.crossXEnd,
                        new Vector3(pos.x, 0.2f, pos.y), new Quaternion(0, -180, 0, 0));
                    else
                        PropsGenerator.Instance.GenerateRoad(PropsGenerator.roadType.crossX,
                        new Vector3(pos.x, 0.2f, pos.y));
                }
                else if (heightMap[j, i] > 0.95f)
                {
                    PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building1,
                         new Vector3(pos.x, 0, pos.y), Quaternion.identity, (heightCurve.Evaluate(heightMap[j, i]) * heightMultiplier));

                }
                else if (heightMap[j, i] > 0.85f)
                {
                    GenerateGrass(pos);
                }
                else if (heightMap[j, i] > 0.77f)
                {
                    PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building3,
                         new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);

                }
                else if (heightMap[j, i] > 0.68f)
                {

                    float rand = Random.value;
                    if (rand < 0.2f)
                        PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building4,
                             new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);
                    else if (rand < 0.6f)
                        PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building5,
                      new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);
                    else
                        PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building2,
                             new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);


                }
                else if (heightMap[j, i] > 0.6f)
                {
                    GenerateGrass(pos);

                }
                else if (heightMap[j, i] > 0.55f)
                {
                    float rand = Random.value;
                    if (rand < 0.2f)
                        PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building6,
                             new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);
                    else if (rand < 0.6f)
                        PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building7,
                      new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);
                    else
                        PropsGenerator.Instance.GenerateBuilding(PropsGenerator.buildingType.building8,
                      new Vector3(pos.x, 0, pos.y), Quaternion.identity, 3);

                }
                else if (heightMap[j, i] > 0.46f)
                {
                    GenerateGrass(pos);
                }
                else if (heightMap[j, i] > 0.3f)
                {
                    GenerateGround(pos);
                }
            }
        }
        GenerateCars();
    }

    void GenerateCars()
    {
        int count = 150;
        int randCount = Random.Range(crossXintersections.Count/2, crossXintersections.Count);
        for (int c = 0; c < randCount; c++)
        {
            int randLine = Random.Range(0, crossXintersections.Count);
            while (crossXintersections[randLine].Count == 0 && count > 0)
            {
                randLine = Random.Range(0, crossXintersections.Count);
                count--;
            }
            List<Vector3> positions = new List<Vector3>();
            foreach (var i in crossXintersections[randLine])
            {
                positions.Add(i);
            }
            int rand1 = Random.Range(0, positions.Count);
            Vector3 start = positions[rand1];
            positions.RemoveAt(rand1);
            int rand2 = Random.Range(0, positions.Count);
            Vector3 end = positions[rand2];
            positions.RemoveAt(rand2);
            Debug.DrawLine(start, end, Color.red , 50);
            int randCar = Random.Range(0, 10);

            PropsGenerator.Instance.GenerateCar((PropsGenerator.carType)randCar,
                       start, start, end, new Quaternion(0, 0.7071f, 0, 0.7071f));
        }
    }

    void GenerateGrass(Vector3 pos)
    {
        float randomVal = Random.value;
        if (randomVal < 0.3f)
            PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.river,
                new Vector3(pos.x, 0, pos.y), new Quaternion(0, 0, 0, 0));
        else
        {
            PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.grass,
                           new Vector3(pos.x, 0, pos.y), Quaternion.identity);
            float val = Random.value;
            if (val < 0.1f)
            {
                PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.bench,
               new Vector3(pos.x, 0, pos.y), new Quaternion(0, Random.Range(-180, 180), 0, 0));
            }
            else if (val < 0.2)
            {
                int rand = Random.Range(0, 10);
                PropsGenerator.Instance.GenerateProp((PropsGenerator.PropType)rand,
                     new Vector3(pos.x, 0, pos.y), Quaternion.identity);

            }
            else if(val < 0.4)
                PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.grassTall,
            new Vector3(pos.x, 0, pos.y), new Quaternion(0, Random.Range(-180, 180), 0, 0));
        }
    }

    void GenerateGround(Vector3 pos)
    {

        float randomVal = Random.value;

        PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.ground,
                       new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        if (randomVal < 0.2f)
        {
            PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.cliff,
           new Vector3(pos.x, 0, pos.y), new Quaternion(0, Random.Range(-180, 180), 0, 0));
        }
        else if (randomVal < 0.3)
        {
            PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.cactus,
    new Vector3(pos.x, 0, pos.y), new Quaternion(0, Random.Range(-180, 180), 0, 0));

        }
        else if (randomVal < 0.4)
        {
            PropsGenerator.Instance.GenerateProp(PropsGenerator.PropType.tent,
    new Vector3(pos.x, 0, pos.y), new Quaternion(0, Random.Range(-180, 180), 0, 0));

        }
    }
    public Vector3 GetPlayerPosition()
    {
        int index = Random.Range(0, intersections.Count);
        return intersections[index].transform.position;
    }
}

public class MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] UVs;
        public Vector3[] normals;
        int currentTriangle;

        public MeshData(int meshWidth, int meshHeight)
        {
            vertices = new Vector3[meshWidth * meshHeight];
            normals = new Vector3[meshWidth * meshHeight];
            UVs = new Vector2[meshWidth * meshHeight];
            triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[currentTriangle] = a;
            triangles[currentTriangle + 1] = b;
            triangles[currentTriangle + 2] = c;
            currentTriangle += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = UVs;
            mesh.normals = normals;
            mesh.RecalculateNormals();
            return mesh;
        }

    }
