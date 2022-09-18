using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ScreenshotShare : MonoBehaviour
{
    string filePath;
    [SerializeField] TMP_InputField NameofPlanet;
    private string shareMesage;
    [SerializeField] private string planetName;
    [SerializeField] private TextMeshProUGUI PlanetName3D;
    [SerializeField] GameObject ShareComplete;
    [SerializeField] GameObject Gohome;

    //스크린 샷 셰어 스크립트

    private void Update()
    {
        planetName = NameofPlanet.text.ToString();
        PlanetName3D.text = planetName;
    }
    public void ScreenShot()
    {
        shareMesage = "이 행성의 이름은? \n" + NameofPlanet.text;
        StartCoroutine(TakeScreenshot());
    }
    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(5);
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        filePath = Path.Combine(Application.temporaryCachePath, "Your Planet.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Artronaut").SetText(shareMesage).Share();
        yield return new WaitForSeconds(2);
        ShareComplete.SetActive(false);
        Gohome.SetActive(true);
    }
}
