//
// SunShader 1.3.0
//
// Panteleymonov Aleksandr 2017
//
// foxes@bk.ru
// mail@panteleymonov.ru
//

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Panteleymonov
{

  public class SaveToTextureSunWindow : EditorWindow
  {
    SunGeneratorEditor target;

    public enum FrameSizeType
    {
      Frame_16x16 = 16,
      Frame_32x32 = 32,
      Frame_64x64 = 64,
      Frame_128x128 = 128,
      Frame_256x256 = 256,
      Frame_512x512 = 512,
      Frame_1024x1024 = 1024,
    }

    public enum FramesCountType
    {
      Count_8 = 8,
      Count_32 = 32,
      Count_128 = 128,
      Count_512 = 512,
    }

    public static void ShowWindow(SunGeneratorEditor targ)
    {
      SaveToTextureSunWindow win = GetWindow<SaveToTextureSunWindow>(false, "Save animated", true);
      win.target = targ;
      win.target.TimeStep = 5.0f;
    }

    void OnGUI()
    {
      if (target == null)
      {
        target = SunGeneratorEditor.current;
        if (SunGeneratorEditor.current == null)
          return;
      }
      GUILayout.BeginVertical();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Frame size ",GUILayout.Width(100));
      target.FrameSize = (FrameSizeType)EditorGUILayout.EnumPopup(target.FrameSize);
      GUILayout.EndHorizontal();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Count of frames ", GUILayout.Width(100));
      target.FrameCount = (FramesCountType)EditorGUILayout.EnumPopup(target.FrameCount);
      GUILayout.EndHorizontal();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      GUILayout.Label("File type ", GUILayout.Width(100));
      target.Type = (SunGeneratorEditor.ImageType)EditorGUILayout.EnumPopup(target.Type);
      GUILayout.EndHorizontal();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Time scale ", GUILayout.Width(100));
      target.TimeStep = EditorGUILayout.FloatField(target.TimeStep);
      GUILayout.EndHorizontal();
      int Width = target.CalcWidth();
      GUILayout.Space(4);
      GUILayout.Label("Result texture size: " + Width.ToString() + " x " + Width.ToString());
      GUILayout.EndVertical();
      target.animatedTexture = (Texture2D)EditorGUILayout.ObjectField("", target.animatedTexture, typeof(Texture2D), false);
      GUILayout.EndHorizontal();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(new GUIContent("Save texture", "Save animated texture")))
      {
        string path = EditorUtility.SaveFilePanel("Save animated texture", Application.dataPath, target.target.name, target.Type.ToString().ToLower());
        if (path.Length != 0) {
          target.SaveToTexture(path);
          path = path.Replace(Application.dataPath, "Assets");
          target.animatedTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
        }
      }
      if (GUILayout.Button(new GUIContent("Save material", "Save material for animated texture")))
      {
        string path = EditorUtility.SaveFilePanel("", Application.dataPath, target.target.name, "mat");
        if (path.Length != 0)
        {
          SunGenerator tTarget = (SunGenerator)target.target;
          Material mat;
          if (tTarget.ShaderMode == SunGenerator.EModeSM.LIGHT_CPU ||
            tTarget.ShaderMode == SunGenerator.EModeSM.LIGHT_SM3 ||
            tTarget.ShaderMode == SunGenerator.EModeSM.LIGHT_SM4)
          {
            mat = new Material(Shader.Find("Space/Star/Sun_soft_animated"));
          }
          else
            mat = new Material(Shader.Find("Space/Star/Sun_animated"));
          mat.SetTexture("_AT", target.animatedTexture);
          mat.SetTextureScale("_AT", Vector2.one * 1.0f / Mathf.Sqrt(((int)target.FrameCount)*2));
          tTarget.fillShaderData(mat);
          path = path.Replace(Application.dataPath, "Assets");
          AssetDatabase.CreateAsset(mat, path);
          AssetDatabase.Refresh();
        }
      }
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }
  }

  [CustomEditor(typeof(SunGenerator))]
  public class SunGeneratorEditor : Editor
  {
    public enum ImageType
    {
      PNG
    }

    public SaveToTextureSunWindow.FrameSizeType FrameSize;
    public SaveToTextureSunWindow.FramesCountType FrameCount;
    public float TimeStep = 5.0f;
    public Texture2D animatedTexture;
    public ImageType Type = ImageType.PNG;
    public static SunGeneratorEditor current;

    [XmlRoot("SunGenerator")]
    public class Data
    {
      Color StringToColor(string val) { string[] temp = val.Split(','); return new Color(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])); }
      string ColorToString(Color val) { return val.r.ToString() + "," + val.g.ToString() + "," + val.b.ToString() + "," + val.a.ToString(); }
      Vector4 StringToVector4(string val) { string[] temp = val.Split(','); return new Vector4(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])); }
      string Vector4ToString(Vector4 val) { return val.x.ToString() + "," + val.y.ToString() + "," + val.z.ToString() + "," + val.w.ToString(); }

      [XmlAttribute("ShaderMode")]
      public SunGenerator.EModeSM ShaderMode;
      [XmlAttribute("MeshType")]
      public SunGenerator.EMeshType MeshType;
      [XmlAttribute("Radius")]
      public float Radius;
      [XmlAttribute("RayString")]
      public float RayString;
      [XmlAttribute("Zoom")]
      public float Zoom;
      [XmlAttribute("Detail")]
      public int Detail;
      [XmlAttribute("Seed")]
      public float Seed;
      [XmlAttribute("Glow")]
      public float Glow;
      [XmlAttribute("Rays")]
      public float Rays;
      [XmlAttribute("RayRing")]
      public float RayRing;
      [XmlAttribute("RayGlow")]
      public float RayGlow;
      [XmlAttribute("Light")]
      public string Light;
      [XmlAttribute("Color")]
      public string Color;
      [XmlAttribute("BaseColor")]
      public string BaseColor;
      [XmlAttribute("DarkColor")]
      public string DarkColor;
      [XmlAttribute("RayLight")]
      public string RayLight;
      [XmlAttribute("RayColor")]
      public string RayColor;
      [XmlAttribute("SpeedLow")]
      public float SpeedLow;
      [XmlAttribute("SpeedHi")]
      public float SpeedHi;
      [XmlAttribute("SpeedRay")]
      public float SpeedRay;
      [XmlAttribute("SpeedRing")]
      public float SpeedRing;
      [XmlAttribute("BodyNoiseLight")]
      public string BodyNoiseLight;
      [XmlAttribute("BodyNoiseScale")]
      public string BodyNoiseScale;
      [XmlAttribute("RayNoiseScale")]
      public string RayNoiseScale;

      public Data()
      {
      }
      public Data(SunGenerator targ)
      {
        Set(ref targ);
      }

      public void Set(ref SunGenerator targ)
      {
        ShaderMode = targ.ShaderMode;
        MeshType = targ.MeshType;
        Radius = targ.Radius;
        RayString = targ.RayString;
        Zoom = targ.Zoom;
        Detail = targ.Detail;
        Seed = targ.Seed;
        Glow = targ.Glow;
        Rays = targ.Rays;
        RayRing = targ.RayRing;
        RayGlow = targ.RayGlow;
        Light = ColorToString(targ.Light);
        Color = ColorToString(targ.Color);
        BaseColor = ColorToString(targ.BaseColor);
        DarkColor = ColorToString(targ.DarkColor);
        RayLight = ColorToString(targ.RayLight);
        RayColor = ColorToString(targ.RayColor);
        SpeedLow = targ.SpeedLow;
        SpeedHi = targ.SpeedHi;
        SpeedRay = targ.SpeedRay;
        SpeedRing = targ.SpeedRing;
        BodyNoiseLight = Vector4ToString(targ.BodyNoiseLight);
        BodyNoiseScale = Vector4ToString(targ.BodyNoiseScale);
        RayNoiseScale = Vector4ToString(targ.RayNoiseScale);
      }

      public void Get(ref SunGenerator targ)
      {
        targ.ShaderMode = ShaderMode;
        targ.MeshType = MeshType;
        targ.Radius = Radius;
        targ.RayString = RayString;
        targ.Zoom = Zoom;
        targ.Detail = Detail;
        targ.Seed = Seed;
        targ.Glow = Glow;
        targ.Rays = Rays;
        targ.RayRing = RayRing;
        targ.RayGlow = RayGlow;
        targ.Light = StringToColor(Light);
        targ.Color = StringToColor(Color);
        targ.BaseColor = StringToColor(BaseColor);
        targ.DarkColor = StringToColor(DarkColor);
        targ.RayLight = StringToColor(RayLight);
        targ.RayColor = StringToColor(RayColor);
        targ.SpeedLow = SpeedLow;
        targ.SpeedHi = SpeedHi;
        targ.SpeedRay = SpeedRay;
        targ.SpeedRing = SpeedRing;
        targ.BodyNoiseLight = StringToVector4(BodyNoiseLight);
        targ.BodyNoiseScale = StringToVector4(BodyNoiseScale);
        targ.RayNoiseScale = StringToVector4(RayNoiseScale);
      }
    }

    void OnEnable()
    {
      FrameSize = SaveToTextureSunWindow.FrameSizeType.Frame_256x256;
      FrameCount = SaveToTextureSunWindow.FramesCountType.Count_32;
    }

    private void OnSceneGUI()
    {
      SunGenerator tTarget = (SunGenerator)target;
      Transform handleTransform = tTarget.transform;
      Handles.color = Color.black;
      float radius = tTarget.Radius * tTarget.Zoom;
      Vector3 normal = SceneView.lastActiveSceneView.camera.transform.position - handleTransform.position;
      normal.Normalize();
      Handles.DrawWireDisc(handleTransform.position, normal, radius);
      Handles.color = Color.yellow;
      radius = (tTarget.Radius + tTarget.RayString) * tTarget.Zoom;
      Handles.DrawWireDisc(handleTransform.position, normal, radius);
    }

    public override void OnInspectorGUI()
    {
      current = this;
      SunGenerator tTarget = (SunGenerator)target;
      EditorGUI.BeginChangeCheck();
      DrawDefaultInspector();
      if (EditorGUI.EndChangeCheck())
        tTarget.Build();
      GUILayout.Space(6);
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(new GUIContent("Save animated", "Save animation to texture and create material")))
      {
        SaveToTextureSunWindow.ShowWindow(this);
        AssetDatabase.Refresh();
      }
      GUI.enabled = tTarget.ShaderMode != SunGenerator.EModeSM.CPU_2SM3
        && tTarget.ShaderMode != SunGenerator.EModeSM.CPU_SM3
        && tTarget.ShaderMode != SunGenerator.EModeSM.CPU_SM4
        && tTarget.ShaderMode != SunGenerator.EModeSM.LIGHT_CPU;
      if (GUILayout.Button(new GUIContent("Save material", "Save current material")))
      {
        string path = EditorUtility.SaveFilePanel("", Application.dataPath, target.name, "mat");
        MeshRenderer mr = tTarget.GetComponent<MeshRenderer>();
        if (path.Length != 0 && mr != null)
        {
          Material mat = mr.sharedMaterial;
          path = path.Replace(Application.dataPath, "Assets");
          AssetDatabase.CreateAsset(mat, path);
          AssetDatabase.Refresh();
        }
      }
      GUI.enabled = true;
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(new GUIContent("Load Themplate", "Load parameters to xml")))
      {
        string path = EditorUtility.OpenFilePanel(
          "Load template from XML",
          "/Assets/SunShader/Temlpates",
          "xml");
        if (path.Length != 0)
          LoadXml(path, ref tTarget);
      }
      if (GUILayout.Button(new GUIContent("Save Themplate", "Save parameters to xml")))
      {
        string path = EditorUtility.SaveFilePanel(
        "Save template as XML",
        "/Assets/SunShader/Temlpates",
        target.name + ".xml",
        "xml");
        if (path.Length != 0)
          SaveXml(path, ref tTarget);
      }
      GUILayout.EndHorizontal();
    }

    public int CalcWidth()
    {
      int iFrameSize = (int)FrameSize;
      int FramesInWidth = Mathf.FloorToInt(Mathf.Sqrt(((int)FrameCount)*2));
      int Width = iFrameSize * FramesInWidth;
      return Width;
    }

    public void SaveToTexture(string toPath)
    {
      SunGenerator tTarget = (SunGenerator)target;
      string[] founded = AssetDatabase.FindAssets("SunUtilities t:ComputeShader");
      if (founded.Length <= 0)
      {
        Debug.LogWarning("Can`t find SunUtilities.compute!");
        return;
      }
      string path =AssetDatabase.GUIDToAssetPath(founded[0]);
      ComputeShader SunUtilities = (ComputeShader)AssetDatabase.LoadAssetAtPath(path, typeof(ComputeShader));
      int indSaveToTexture = SunUtilities.FindKernel("SunGenerator");

      int iFrameSize = (int)FrameSize;
      int FramesInWidth = Mathf.FloorToInt(Mathf.Sqrt(((int)FrameCount) * 2 ));
      int Width = iFrameSize * FramesInWidth;
      int DispatchCount = Width / 16;

      RenderTexture RenderResult = null;
      RenderResult = new RenderTexture(Width, Width, 0);
      RenderResult.enableRandomWrite = true;
      RenderResult.Create();
      SunUtilities.SetFloat("Rays", tTarget.Rays);
      SunUtilities.SetFloat("RayRing", tTarget.RayRing);
      SunUtilities.SetVector("BodyNoiseS", tTarget.BodyNoiseScale);
      SunUtilities.SetVector("BodyNoiseL", tTarget.BodyNoiseLight);
      SunUtilities.SetVector("RayNoiseS", tTarget.RayNoiseScale);
      SunUtilities.SetFloat("SpeedRing", tTarget.SpeedRing);
      SunUtilities.SetFloat("SpeedRay", tTarget.SpeedRay);
      SunUtilities.SetFloat("Radius", tTarget.Radius);
      SunUtilities.SetFloat("Zoom", tTarget.Zoom);
      SunUtilities.SetFloat("Seed", tTarget.Seed);
      SunUtilities.SetFloat("TimeStep", TimeStep);
      SunUtilities.SetFloat("SpeedLow", tTarget.SpeedLow);
      SunUtilities.SetFloat("SpeedHi", tTarget.SpeedHi);
      SunUtilities.SetFloat("Detail", tTarget.Detail);
      SunUtilities.SetInt("FrameSize", iFrameSize);
      SunUtilities.SetInt("FramesInWidth", FramesInWidth);
      SunUtilities.SetTexture(indSaveToTexture, "Result", RenderResult);
      SunUtilities.Dispatch(indSaveToTexture, DispatchCount, DispatchCount, 1);
      RenderTexture.active = RenderResult;

      Texture2D ResultToSave = new Texture2D(RenderResult.width, RenderResult.height, TextureFormat.ARGB32, false, true);
      ResultToSave.ReadPixels(new Rect(0, 0, RenderResult.width, RenderResult.height), 0, 0);
      ResultToSave.Apply();

      byte[] bytes = new byte[0];
      if (Type == ImageType.PNG)
        bytes = ResultToSave.EncodeToPNG();
      File.WriteAllBytes(toPath, bytes);
    }

    public void LoadXml(string file, ref SunGenerator gen)
    {
      try
      {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        FileStream stream = new FileStream(file, FileMode.Open);
        Data data = serializer.Deserialize(stream) as Data;
        stream.Close();
        data.Get(ref gen);
        gen.Build();
      }
      catch (Exception ex)
      {
        Debug.Log("Wrong xml format: " + ex.Message);
      }
    }

    public void SaveXml(string file, ref SunGenerator gen)
    {
      Data data = new Data(gen);
      XmlSerializer serializer = new XmlSerializer(typeof(Data));
      FileStream stream = new FileStream(file, FileMode.Create);
      serializer.Serialize(stream, data);
      stream.Close();
    }
  }
}
