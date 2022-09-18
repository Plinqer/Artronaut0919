//
// SunShader 1.3.0
//
// Panteleymonov Aleksandr 2017
//
// foxes@bk.ru
// mail@panteleymonov.ru
//

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Panteleymonov
{
#if UNITY_EDITOR
  [CustomEditor(typeof(SunShape))]
  public class SunShapeEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      SunShape tTarget = (SunShape)target;
      EditorGUI.BeginChangeCheck();
      DrawDefaultInspector();
      if (EditorGUI.EndChangeCheck())
        tTarget.Build();
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(new GUIContent("Update shape", "")))
      {
        tTarget.Build();
      }
      if (GUILayout.Button(new GUIContent("Save shape", "")))
      {
        string path = EditorUtility.SaveFilePanelInProject("Save Asset", "as.asset", "asset", "");
        if (path.Length != 0)
        {
          MeshFilter mf = tTarget.GetComponent<MeshFilter>();
          Mesh mesh = Instantiate(mf.sharedMesh);
          mesh.name = Path.GetFileNameWithoutExtension(path);
          AssetDatabase.CreateAsset(mesh, path);
          AssetDatabase.SaveAssets();
        }
      }
      GUILayout.EndHorizontal();
    }
  }
#endif

  [ExecuteInEditMode]
  [AddComponentMenu("Space/Star/SunShape")]
  public class SunShape : MonoBehaviour
  {
    public enum EMeshType { Billboard, Prisma } //Box,Sphere

    [Tooltip("Model of mesh for view body, Billboard, Prisma")]
    public EMeshType MeshType = EMeshType.Prisma;

    // Use this for initialization
    void Start()
    {
      Build();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Build()
    {
      if (MeshType == EMeshType.Billboard) MeshBillboard();
      if (MeshType == EMeshType.Prisma) MeshPrisma();

      MeshRenderer mr = GetComponent<MeshRenderer>();
      if (mr == null) mr = (MeshRenderer)gameObject.AddComponent<MeshRenderer>();
      Material[] materials = mr.sharedMaterials;
      if (materials[0] != null)
      {
        float r = (materials[0].GetFloat("_Radius") + materials[0].GetFloat("_RayString")) / materials[0].GetFloat("_Zoom");
        if (float.IsNaN(r)) r = 1.0f;
        transform.localScale = new Vector3(r, r, r);
      }
    }

    MeshFilter PrepeareMesh()
    {
      MeshFilter mf = GetComponent<MeshFilter>();
      if (mf == null)
      {
        mf = (MeshFilter)gameObject.AddComponent<MeshFilter>();
      }
      mf.sharedMesh = new Mesh();
      mf.sharedMesh.Clear();
      return mf;
    }

    void MeshBillboard()
    {
      MeshFilter mf = PrepeareMesh();
      mf.sharedMesh = SunGenerator.GetBilboard();
    }

    void MeshPrisma()
    {
      MeshFilter mf = PrepeareMesh();
      mf.sharedMesh = SunGenerator.GetPrisma();
    }
  }
}
