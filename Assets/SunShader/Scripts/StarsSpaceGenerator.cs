//
// SunShader 1.3.0
//
// Panteleymonov Aleksandr 2017
//
// foxes@bk.ru
// mail@panteleymonov.ru
//

using UnityEngine;

namespace Panteleymonov
{
  [ExecuteInEditMode]
  [AddComponentMenu("Space/SkyBox/StarsSpaceGenerator")]
  public class StarsSpaceGenerator : MonoBehaviour
  {
    public enum EFaseSize
    {
      _16 = 16,
      _32 = 32,
      _64 = 64,
      _128 = 128,
      _256 = 256,
      _512 = 512,
      _1024 = 1024,
      _2048 = 2048
    }

    public enum EUpdateType
    {
      Manual,
      RealTime
    }

    [Header("Stars Colors")]
    [Tooltip("Color circle of stars")]
    public Color StarsColor1 = new Color(1, 0, 0, 1);
    [Tooltip("Color circle of stars")]
    public Color StarsColor2 = new Color(0, 0, 1, 1);
    [Tooltip("Main color of stars")]
    public Color StarsPrimary = new Color(1, 1, 1, 1);

    [Header("Stars")]
    [Tooltip("Intensity of primary stars layer")]
    public float StarsBrightPrimary = 1.0f;
    [Tooltip("Intensity of second stars layer")]
    public float StarsBrightSecond = 0.5f;
    [Tooltip("Stars seed for both layers")]
    [Range(0,1000)]
    public float StarsSeed = 1;

    [Header("Cloud Colors")]
    [Tooltip("Color of space clouds")]
    public Color CloudColor1 = new Color(1, 0, 0, 1);
    [Tooltip("Color of space clouds")]
    public Color CloudColor2 = new Color(0, 0, 1, 1);
    [Tooltip("Main color of space clouds, all colors will be mixed with this")]
    public Color CloudPrimary = new Color(0.9f, 1.0f, 0.1f, 1);

    [Header("Cloud")]
    [Tooltip("Main intensity of space clouds")]
    public float Bright = 1.5f;
    [Tooltip("Main scale of space clouds")]
    public float Zoom = 1.0f;
    [Tooltip("Interpolation between the two noise algorithms \"Voronoi\" and \"Perlin\"")]
    [Range(0.0f, 1.0f)]
    public float VoronoiPerlin = 0.5f;
    [Tooltip("Intensity of each of the following layer noise")]
    [Range(0.0001f, 1)]
    public float LightStage = 0.8f;
    [Tooltip("Scale of each of the following layer noise")]
    [Range(0.0001f, 1)]
    public float ScaleStage = 0.5f;
    [Tooltip("Space cloud seed")]
    [Range(0, 1000)]
    public float CloudSeed = 1;

    [Header("Material Properties")]
    [Tooltip("Type of visual update metod, if use \"Real Time\" - this will give poor performance")]
    public EUpdateType UpdateType = EUpdateType.Manual;
    [Tooltip("Size of texture for per side of cubemap")]
    public EFaseSize TextureSize = EFaseSize._512;
    [Tooltip("Exposure")]
    public float Exposure = 1.0f;

    private Material material = null;
    private Material preview = null;
    private Texture2D front = null;
    private Texture2D back = null;
    private Texture2D up = null;
    private Texture2D down = null;
    private Texture2D left = null;
    private Texture2D right = null;

    public static string[] MaterialTextureNames = { "_FrontTex", "_BackTex", "_LeftTex", "_RightTex", "_UpTex", "_DownTex" };

    // Use this for initialization
    void Start()
    {
      if (UpdateType == EUpdateType.RealTime)
        InitPreview();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnValidate()
    {
      if (material != null && UpdateType != EUpdateType.RealTime)
      {
        RenderSettings.skybox = material;
        material.SetFloat("_Exposure", Exposure);
      }
      if (UpdateType == EUpdateType.RealTime)
      {
        if (preview == null)
          InitPreview();
        UpdatePreview();
      }
    }

    void CheckTexture(ref Texture2D texture, int Size)
    {
      if (texture == null)
      {
        texture = new Texture2D(Size, Size, TextureFormat.ARGB32, true);
        texture.wrapMode = TextureWrapMode.Clamp;
      }
      if (texture.width != Size)
        texture.Resize(Size, Size);
    }

    void InitMaterial()
    {
#if UNITY_5
      if (material == null)
        material = new Material(Shader.Find("Skybox/6 Sided"));
#else
      if (material == null)
        material = new Material(Shader.Find("RenderFX/Skybox"));
#endif
      CheckTexture(ref front,(int)TextureSize);
      CheckTexture(ref back, (int)TextureSize);
      CheckTexture(ref up, (int)TextureSize);
      CheckTexture(ref down, (int)TextureSize);
      CheckTexture(ref left, (int)TextureSize);
      CheckTexture(ref right, (int)TextureSize);

      RenderSettings.skybox = material;
    }

    void InitPreview()
    {
      if (preview == null)
        preview = new Material(Shader.Find("Space/Sky Box/Stars Space"));
      UpdatePreview();

      RenderSettings.skybox = preview;
    }

    public void UpdateTexturesCubeMap(ComputeShader shader)
    {
      InitMaterial();
      int size = (int)TextureSize;
      RenderTexture OutTexture = new RenderTexture(size, size, 0);
      OutTexture.enableRandomWrite = true;
      OutTexture.Create();

      int indStarSpace = shader.FindKernel("StarSpaceGenerator");
      //shader.SetFloat("Exposure", Exposure);
      shader.SetFloat("Zoom",Zoom);
      shader.SetFloat("VPMix", VoronoiPerlin);
      shader.SetFloat("LightStage", LightStage);
      shader.SetFloat("ScaleStage", ScaleStage);
      shader.SetFloat("Bright", Bright);
      shader.SetFloat("CloudSeed", CloudSeed);
      shader.SetFloat("StarsSeed", StarsSeed);
      shader.SetVector("CloudColor1", CloudColor1);
      shader.SetVector("CloudColor2", CloudColor2);
      shader.SetVector("CloudColor3", CloudPrimary);
      shader.SetVector("StarsColor1", StarsColor1);
      shader.SetVector("StarsColor2", StarsColor2);
      shader.SetVector("StarsColor3", StarsPrimary);
      shader.SetFloat("FirstStarsBright", StarsBrightPrimary);
      shader.SetFloat("SecondStarsBright", StarsBrightSecond);
      shader.SetFloat("TextureSize",1.0f/size);

      shader.SetTexture(indStarSpace, "Result", OutTexture);
      int dipc = size / 16;

      Texture2D[] textures = GetTextures();
      for (int i = 0; i < 6; i++)
      {
        shader.SetInt("Side", i);
        shader.Dispatch(indStarSpace, dipc, dipc, 1);
        RenderTexture.active = OutTexture;
        textures[i].ReadPixels(new Rect(0, 0, OutTexture.width, OutTexture.height), 0, 0);
        textures[i].Apply();
        material.SetTexture(MaterialTextureNames[i], textures[i]);
      }
    }

    void UpdatePreview()
    {
      preview.SetFloat("_Exposure", Exposure);
      preview.SetFloat("_Zoom", Zoom);
      preview.SetFloat("_VPMix", VoronoiPerlin);
      preview.SetFloat("_LightStage", LightStage);
      preview.SetFloat("_ScaleStage", ScaleStage);
      preview.SetFloat("_Bright", Bright);
      preview.SetFloat("_CloudSeed", CloudSeed);
      preview.SetFloat("_StarsSeed", StarsSeed);
      preview.SetVector("_CloudColor1", CloudColor1);
      preview.SetVector("_CloudColor2", CloudColor2);
      preview.SetVector("_CloudColor3", CloudPrimary);
      preview.SetVector("_StarsColor1", StarsColor1);
      preview.SetVector("_StarsColor2", StarsColor2);
      preview.SetVector("_StarsColor3", StarsPrimary);
      preview.SetFloat("_FirstStarsBright", StarsBrightPrimary);
      preview.SetFloat("_SecondStarsBright", StarsBrightSecond);

      RenderSettings.skybox = preview;
    }

    public Texture2D[] GetTextures()
    {
      Texture2D[] textures = { front, back, left, right, up, down };
      return textures;
    }

    public Material GetMaterial()
    {
      return material;
    }
  }
}
