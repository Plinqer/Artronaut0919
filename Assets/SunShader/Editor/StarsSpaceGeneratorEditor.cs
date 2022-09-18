//
// SunShader 1.3.0
//
// Panteleymonov Aleksandr 2017
//
// foxes@bk.ru
// mail@panteleymonov.ru
//

using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

namespace Panteleymonov
{
  public class SaveToTextureStarsSpaceWindow : EditorWindow
  {
    public enum PrefixName
    {
      Num,
      Side,
    }

    public enum ImageType
    {
      PNG,
      JPG
    }

    public enum SpaserType
    {
      None,
      Space,
      Derscore
    }

    public enum SaveType
    {
      All,
      Textures,
      Material
    }

    StarsSpaceGeneratorEditor target;
    string basefilename;
    string basepath;
    PrefixName NamePrefix = PrefixName.Side;
    ImageType Type = ImageType.PNG;
    SpaserType Separator = SpaserType.Derscore;
    int quality = 90;

    static string[] prefix = new string[6] { "front", "back", "left", "right", "up", "down" };

    public static void ShowWindow(StarsSpaceGeneratorEditor targ)
    {
      SaveToTextureStarsSpaceWindow win = GetWindow<SaveToTextureStarsSpaceWindow>(false, "Save To Texture", true);
      win.target = targ;
      win.basefilename = targ.target.name;
      win.basepath = Application.dataPath;
    }

    void OnGUI()
    {
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.Label("Spacer ", GUILayout.Width(100));
      Separator = (SpaserType)EditorGUILayout.EnumPopup(Separator);
      GUILayout.Label("Name prefix ", GUILayout.Width(100));
      NamePrefix = (PrefixName)EditorGUILayout.EnumPopup(NamePrefix);
      GUILayout.Label("File type ", GUILayout.Width(100));
      Type = (ImageType)EditorGUILayout.EnumPopup(Type);
      GUILayout.EndHorizontal();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      GUILayout.Label("Path ", GUILayout.Width(100));
      basepath = EditorGUILayout.TextField(basepath);
      GUILayout.EndHorizontal();
      GUILayout.Space(4);
      GUILayout.BeginHorizontal();
      GUILayout.Label("File name ", GUILayout.Width(100));
      basefilename = EditorGUILayout.TextField(basefilename);
      if (GUILayout.Button(new GUIContent("select", "change file name"), GUILayout.Width(100)))
      {
        string path = EditorUtility.SaveFilePanel("", Application.dataPath, basefilename, "mat");
        if (path.Length != 0)
        {
          basefilename = Path.GetFileNameWithoutExtension(path);
          basepath = Path.GetDirectoryName(path);
        }
      }
      GUILayout.EndHorizontal();
      if (Type == ImageType.JPG) {
        GUILayout.BeginHorizontal();
        GUILayout.Label("JPG quality ", GUILayout.Width(100));
        quality = EditorGUILayout.IntSlider(quality,0,100);
        GUILayout.EndHorizontal();
      }
      GUILayout.Label("The files will be created:");
      for (int i = 0; i < 6; i++)
      {
        string pref = prefix[i];
        if (NamePrefix == PrefixName.Num)
          pref = i.ToString();
        GUILayout.Label(" - " + basefilename + AddSpacer(pref) + "." + Type.ToString().ToLower());
      }
      GUILayout.Label(" - " + basefilename + ".mat");
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(new GUIContent("Save textures", "Save textures"), GUILayout.Width(100)))
      {
        SaveFiles(SaveType.Textures);
      }
      if (GUILayout.Button(new GUIContent("Save material", "Save material"), GUILayout.Width(100)))
      {
        SaveFiles(SaveType.Material);
      }
      if (GUILayout.Button(new GUIContent("Save all", "Save textures and material"), GUILayout.Width(100)))
      {
        SaveFiles();
      }
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }

    string AddSpacer(string pref)
    {
      if (Separator == SpaserType.Derscore)
        return "_" + pref;
      if (Separator == SpaserType.Space)
        return " " + pref;
      return pref;
    }

    void SaveFiles(SaveType selected = SaveType.All)
    {
      StarsSpaceGenerator tTarget = ((StarsSpaceGenerator)target.target);
      StarsSpaceGeneratorEditor.UpdateTexturesCubeMap(tTarget);
      Texture2D[] textures = tTarget.GetTextures();
      for (int i = 0; i < 6; i++)
      {
        if (textures[i] == null)
          break;
        string pref = prefix[i];
        if (NamePrefix == PrefixName.Num)
          pref = i.ToString();
        string toPath = Path.Combine(basepath, basefilename + AddSpacer(pref) + "." + Type.ToString().ToLower());
        if (selected != SaveType.Material)
        {
          byte[] bytes = new byte[0];
          if (Type == ImageType.PNG)
            bytes = textures[i].EncodeToPNG();
          if (Type == ImageType.JPG)
            bytes = textures[i].EncodeToJPG(quality);
          File.WriteAllBytes(toPath, bytes);
          AssetDatabase.Refresh();
        }
        toPath = toPath.Replace(Application.dataPath, "Assets");
        textures[i] = (Texture2D)AssetDatabase.LoadAssetAtPath(toPath, typeof(Texture2D));
        textures[i].wrapMode = TextureWrapMode.Clamp;
      }
      if (selected != SaveType.Textures) {
        Material mat = new Material(tTarget.GetMaterial());
        for (int i = 0; i < 6; i++)
        {
          mat.SetTexture(StarsSpaceGenerator.MaterialTextureNames[i], textures[i]);
        }
        string toPathMaterial = Path.Combine(basepath, basefilename + ".mat");
        toPathMaterial = toPathMaterial.Replace(Application.dataPath, "Assets");
        AssetDatabase.CreateAsset(mat, toPathMaterial);
      }
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
    }
  }

  [CustomEditor(typeof(StarsSpaceGenerator))]
  public class StarsSpaceGeneratorEditor : Editor
  {
    [XmlRoot("StarsSpaceGenerator")]
    public class Data
    {
      Color StringToColor(string val) { string[] temp = val.Split(','); return new Color(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])); }
      string ColorToString(Color val) { return val.r.ToString() + "," + val.g.ToString() + "," + val.b.ToString() + "," + val.a.ToString(); }

      [XmlAttribute("StarsColor1")]
      public string StarsColor1;
      [XmlAttribute("StarsColor2")]
      public string StarsColor2;
      [XmlAttribute("StarsPrimary")]
      public string StarsPrimary;
      [XmlAttribute("StarsBrightPrimary")]
      public float StarsBrightPrimary;
      [XmlAttribute("StarsBrightSecond")]
      public float StarsBrightSecond;
      [XmlAttribute("StarsSeed")]
      public float StarsSeed;
      [XmlAttribute("CloudColor1")]
      public string CloudColor1;
      [XmlAttribute("CloudColor2")]
      public string CloudColor2;
      [XmlAttribute("CloudPrimary")]
      public string CloudPrimary;
      [XmlAttribute("Zoom")]
      public float Zoom;
      [XmlAttribute("VPMix")]
      public float VPMix;
      [XmlAttribute("LightStage")]
      public float LightStage;
      [XmlAttribute("ScaleStage")]
      public float ScaleStage;
      [XmlAttribute("Bright")]
      public float Bright;
      [XmlAttribute("CloudSeed")]
      public float CloudSeed;
      [XmlAttribute("TextureSize")]
      public StarsSpaceGenerator.EFaseSize TextureSize;
      [XmlAttribute("Exposure")]
      public float Exposure;

      public Data()
      {

      }

      public Data(StarsSpaceGenerator targ)
      {
        Set(ref targ);
      }

      public void Set(ref StarsSpaceGenerator targ)
      {
        StarsColor1 = ColorToString(targ.StarsColor1);
        StarsColor2 = ColorToString(targ.StarsColor2);
        StarsPrimary = ColorToString(targ.StarsPrimary);
        StarsBrightPrimary = targ.StarsBrightPrimary;
        StarsBrightSecond = targ.StarsBrightSecond;
        StarsSeed = targ.StarsSeed;
        CloudColor1 = ColorToString(targ.CloudColor1);
        CloudColor2 = ColorToString(targ.CloudColor2);
        CloudPrimary = ColorToString(targ.CloudPrimary);
        Zoom = targ.Zoom;
        VPMix = targ.VoronoiPerlin;
        LightStage = targ.LightStage;
        ScaleStage = targ.ScaleStage;
        Bright = targ.Bright;
        CloudSeed = targ.CloudSeed;
        TextureSize = targ.TextureSize;
        Exposure = targ.Exposure;
      }

      public void Get(ref StarsSpaceGenerator targ)
      {
        targ.StarsColor1 = StringToColor(StarsColor1);
        targ.StarsColor2 = StringToColor(StarsColor2);
        targ.StarsPrimary = StringToColor(StarsPrimary);
        targ.StarsBrightPrimary = StarsBrightPrimary;
        targ.StarsBrightSecond = StarsBrightSecond;
        targ.StarsSeed = StarsSeed;
        targ.CloudColor1 = StringToColor(CloudColor1);
        targ.CloudColor2 = StringToColor(CloudColor2);
        targ.CloudPrimary = StringToColor(CloudPrimary);
        targ.Zoom = Zoom;
        targ.VoronoiPerlin = VPMix;
        targ.LightStage = LightStage;
        targ.ScaleStage = ScaleStage;
        targ.Bright = Bright;
        targ.CloudSeed = CloudSeed;
        targ.TextureSize = TextureSize;
        targ.Exposure = Exposure;
      }
    }

    public override void OnInspectorGUI()
    {
      StarsSpaceGenerator tTarget = (StarsSpaceGenerator)target;
      DrawDefaultInspector();
      GUILayout.BeginHorizontal();
      GUI.enabled = tTarget.UpdateType == StarsSpaceGenerator.EUpdateType.Manual;
      if (GUILayout.Button(new GUIContent("Update", "Rebuild Sky Box Textures"), GUILayout.Width(100)))
      {
        UpdateTexturesCubeMap((StarsSpaceGenerator)target);
      }
      GUI.enabled = true;
      if (GUILayout.Button(new GUIContent("Load Themplate", "Load parameters from XML")))
      {
        string path = EditorUtility.OpenFilePanel(
          "Load template from XML",
          "/Assets/SunShader/Temlpates",
          "xml");
        if (path.Length != 0)
          LoadXml(path, ref tTarget);
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(new GUIContent("Save Sky Box", "Save all textures and material."
        + "When you are finished choose the settings, you can create a static skybox for high performance"), GUILayout.Width(100)))
      {
        SaveToTextureStarsSpaceWindow.ShowWindow(this);
      }
      if (GUILayout.Button(new GUIContent("Save Themplate", "Save parameters to XML")))
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

    static public void UpdateTexturesCubeMap(StarsSpaceGenerator target)
    {
      string[] founded = AssetDatabase.FindAssets("SunUtilities t:ComputeShader");
      if (founded.Length <= 0)
      {
        Debug.LogWarning("Can`t find SunUtilities.compute!");
        return;
      }
      string path = AssetDatabase.GUIDToAssetPath(founded[0]);
      ComputeShader SunUtilities = (ComputeShader)AssetDatabase.LoadAssetAtPath(path, typeof(ComputeShader));
      target.UpdateTexturesCubeMap(SunUtilities);
    }

    public void LoadXml(string file, ref StarsSpaceGenerator gen)
    {
      try
      {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        FileStream stream = new FileStream(file, FileMode.Open);
        Data data = serializer.Deserialize(stream) as Data;
        stream.Close();
        data.Get(ref gen);
        UpdateTexturesCubeMap((StarsSpaceGenerator)gen);
      }
      catch (Exception ex)
      {
        Debug.Log("Wrong xml format: "+ ex.Message);
      }
    }

    public void SaveXml(string file, ref StarsSpaceGenerator gen)
    {
      Data data = new Data(gen);
      XmlSerializer serializer = new XmlSerializer(typeof(Data));
      FileStream stream = new FileStream(file, FileMode.Create);
      serializer.Serialize(stream, data);
      stream.Close();
    }
  }

}
