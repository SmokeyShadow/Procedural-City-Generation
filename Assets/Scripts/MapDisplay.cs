using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapDisplay : MonoBehaviour {
    [SerializeField]
    private Renderer textureRenderer;
    [SerializeField]
    private Renderer meshRenderer;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private RawImage textureDisplay;
    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    internal void DrawMesh(MeshData meshData, Texture2D texture2D)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture2D;
        textureRenderer.transform.localScale = new Vector3(3, 1, 3);
    }

    public void DrawRawImage(Texture2D texture2D)
    {
        textureDisplay.texture = texture2D;
    }
}
