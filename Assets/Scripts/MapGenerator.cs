using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour {

    public bool autoUpdate;

    [SerializeField, Range(20,200)]
    private int mapWidth;

    [SerializeField, Range(20, 200)]
    private int mapHeight;

    [SerializeField]
    private float scale;

    [SerializeField]
    private int seed;

    [SerializeField]
    private  float[] offsets = new float[2];

    [SerializeField]
    private regionType[] regions;

    [SerializeField]
    private DrawMode drawMode;

    [SerializeField]
    private AnimationCurve heightCurve;

    [SerializeField]
    private float meshHeightMultiplier;

    [SerializeField]
    private Slider seedSlider;

    [SerializeField]
    private Slider scaleSlider;

    [SerializeField]
    private Slider offsetXSlider;

    [SerializeField]
    private Slider offsetYSlider;

    [SerializeField]
    private Slider widthSlider;

    [SerializeField]
    private Slider heightSlider;

    [SerializeField]
    private Button toggleViewButton;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private Transform cameraPosition;

    [SerializeField]
    private Transform topCameraPosition;

    private MapDisplay display;

    float[,] noiseMap;

    Color[] colorMap;

    bool characterView = false;
    public enum DrawMode
    {
        NoiseMap, ColorMap, Mesh, City
    }

    private void Start()
    {
        display = GetComponent<MapDisplay>();
        GenerateTexture();
    }


    public void GenerateMap()
    {
        MeshGenerator.Instance.DestroyMap();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.Instance.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.Instance.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.Instance.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, heightCurve), TextureGenerator.Instance.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.City)
        {
            display.DrawMesh(MeshGenerator.Instance.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, heightCurve), TextureGenerator.Instance.TextureFromColorMap(colorMap, mapWidth, mapHeight));
            MeshGenerator.Instance.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, heightCurve);
            MeshGenerator.Instance.GenerateCity(noiseMap, meshHeightMultiplier, heightCurve);
        }
    }

    void GenerateTexture()
    {
        noiseMap = Noise.Instance.GenerateNoiseMap(mapWidth, mapHeight, scale, seed, offsets[0], offsets[1]);
        colorMap = new Color[mapHeight * mapWidth];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (noiseMap[x, y] <= regions[i].height)
                    {
                        colorMap[mapWidth * y + x] = regions[i].color;
                        break;
                    }
                }

            }
        }
        display.DrawRawImage(TextureGenerator.Instance.TextureFromColorMap(colorMap, mapWidth, mapHeight));

    }
    private void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
    }

    public void GenerateClick()
    {
        GenerateMap();
        if(toggleViewButton.interactable == false)
            toggleViewButton.interactable = true;
    }

    public void ToggleViewClick()
    {
        if (!characterView)
        {
            player.transform.position = MeshGenerator.Instance.GetPlayerPosition() + 20 * Vector3.up;
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = cameraPosition.position;
            camera.transform.localRotation = cameraPosition.rotation;
            player.SetActive(true);
            characterView = true;
        }
        else
        {
            player.transform.position = MeshGenerator.Instance.GetPlayerPosition() + 20 * Vector3.up;
            camera.transform.SetParent(null);
            camera.transform.localPosition = topCameraPosition.position;
            camera.transform.localRotation = topCameraPosition.rotation;
            player.SetActive(false);
            characterView = false;
        }
    }

    private void Update()
    {
        if (camera.transform.parent != null)
        {
            camera.transform.localPosition = cameraPosition.position;
            camera.transform.localRotation = cameraPosition.rotation;
        }
        if (seed != (int)seedSlider.value || scale != scaleSlider.value || offsets[0] != offsetXSlider.value || offsets[1] != offsetYSlider.value
            || mapWidth != widthSlider.value || mapHeight !=heightSlider.value)
        {
            mapWidth = (int)widthSlider.value;
            mapHeight = (int)heightSlider.value;
            seed = (int)seedSlider.value;
            scale = scaleSlider.value;
            offsets[0] = offsetXSlider.value;
            offsets[1] = offsetYSlider.value;
            GenerateTexture();
        }
    }
}

[System.Serializable]
public struct regionType
{
    public string name;
    public Color color;
    public float height;
}

