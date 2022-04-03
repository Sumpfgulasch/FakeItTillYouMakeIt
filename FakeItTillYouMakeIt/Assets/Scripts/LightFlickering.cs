using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



public class LightFlickering : SerializedMonoBehaviour
{
    const string A = "Attributes";
    const string R = "References";
    const string D = "Debug";

    [TitleGroup(A)] public float speed = 0.4f;
    [TitleGroup(A)] public Vector2 lightIntensityRange = new Vector2(0.5f, 1.75f);

    [TitleGroup(R)] [Required] public Material backgroundMaterial = null;
    [TitleGroup(R)] [Required] public Camera mainCamera = null;

    [TitleGroup(D)][ShowInInspector][ReadOnly] private bool run = false;


    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (mainCamera == null)
        {
            Debug.LogWarning("hier muss ganz dringend eine Kamera assigned werden - vielleicht gibts gerade garkeine in der Szene?");
        }

        if (backgroundMaterial != null && mainCamera != null)
        {
            run = true;
        }
    }

    void Update()
    {
        if (run)
        {
            float t = Time.time;
            float scaledT = t * speed;
            float noise = Mathf.PerlinNoise(scaledT, scaledT + 54.223f); // sample den Perlin noise mit 2 Koordinaten, einmal mit zeit und einmal mit Zeit + random offset.
            float intensity = Remap(noise, 0f, 1f, lightIntensityRange.x, lightIntensityRange.y);
            backgroundMaterial.SetFloat("_MainColorIntensity", intensity);



        }


        //UnityEngine.Rendering.
        //UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        //if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        //// You can leave this variable out of your function, so you can reuse it throughout your class.
        //UnityEngine.Rendering.Universal.Vignette vignette;

        //if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        //vignette.intensity.Override(0.5f);

    }

    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
