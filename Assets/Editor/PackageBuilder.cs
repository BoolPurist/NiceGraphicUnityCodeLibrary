using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor; 

namespace NiceGraphicLibrary.Editor
{
  public class PackageBuilder : EditorWindow
  {
    private string PACKAGE_PATH;
    private string PACKAGE_OUTPUT_PATH;
    private ExportPackageOptions EXPORT_OPTIONS;

    private const float MAX_WINDOW_WIDTH = 480f;
    private const float MAX_WINDOW_HEIGHT = 640f;


    private bool includeDependencies;
    private string packageName;
    private bool lastPackageWasDuplicate;
    private string previousPackageName;

    private int majorVersion;
    private int minorVersion;
    private int pathVersion;

    [MenuItem("Integration/Package Builder")]
    private static void CreateWindow()
    {
      var window = GetWindow<PackageBuilder>();
      window.titleContent = new GUIContent("Package Builder");
      window.maxSize = new Vector2(MAX_WINDOW_WIDTH, MAX_WINDOW_HEIGHT);
      window.Show();
    }

    private void OnEnable()
    {
      PACKAGE_PATH = $"Assets/PackageNicegraphicLibrary";
      PACKAGE_OUTPUT_PATH = $"{Application.dataPath}/Built Packages";
      EXPORT_OPTIONS = ExportPackageOptions.Recurse;

      packageName = "NiceGraphicLibrary";
      lastPackageWasDuplicate = false;
      majorVersion = 0;
      minorVersion = 0;
      pathVersion = 0;
  }



    private void OnGUI()
    {
      EditorGUILayout.LabelField(PACKAGE_PATH);

      packageName = EditorGUILayout.TextField("Package Name", packageName);
      includeDependencies = EditorGUILayout.Toggle("Include Dependencies", includeDependencies);

      RenderVersioningInput();

      if (GUILayout.Button("Create Package"))
      {
        
        string finalPath = CreateFinalPath();
        previousPackageName = CreateFinalName();
        EnsureOutputPathCorrectOutputPath();

        if (IsDuplicatePackageInTarget(finalPath))
        {
          lastPackageWasDuplicate = true;
          
        }
        else
        {
          AdjustExportOptions();
          lastPackageWasDuplicate = false;
          
          Export(finalPath);
        }        
      }

      if (lastPackageWasDuplicate)
      {
        EditorGUILayout.HelpBox($"A package with name [{previousPackageName}] already exits", MessageType.Error);
      }
    }

    private void RenderVersioningInput()
    {
      majorVersion = EditorGUILayout.IntField("Major Version", majorVersion);
      minorVersion = EditorGUILayout.IntField("Minor Version", minorVersion);
      pathVersion = EditorGUILayout.IntField("Path Version", pathVersion);

      majorVersion = Mathf.Abs(majorVersion);
      minorVersion = Mathf.Abs(minorVersion);
      pathVersion = Mathf.Abs(pathVersion);
    }

    private string CreateFinalPath() => $"{PACKAGE_OUTPUT_PATH}/{packageName}_v{majorVersion}.{minorVersion}.{pathVersion}.unitypackage";

    private string CreateFinalName() => $"{packageName}_v{majorVersion}.{minorVersion}.{pathVersion}.unitypackage";

    private void AdjustExportOptions()
    {
      EXPORT_OPTIONS = ExportPackageOptions.Recurse;
      if (includeDependencies)
      {
        EXPORT_OPTIONS |= ExportPackageOptions.IncludeDependencies;        
      }
    }

    private bool IsDuplicatePackageInTarget(in string finalPath) => File.Exists(finalPath);



    private void EnsureOutputPathCorrectOutputPath()
    {
      Directory.CreateDirectory(PACKAGE_OUTPUT_PATH);
      AssetDatabase.Refresh();
    }

    private void Export(in string finalOutPath)
    {
      Debug.Log($"PACKAGE_PATH: {PACKAGE_PATH}");
      AssetDatabase.ExportPackage(PACKAGE_PATH, finalOutPath, EXPORT_OPTIONS);
      AssetDatabase.Refresh();
    }
  } 
}