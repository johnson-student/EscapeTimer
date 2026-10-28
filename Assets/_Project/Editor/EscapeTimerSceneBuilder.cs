using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;

public static class EscapeTimerSceneBuilder
{
    [MenuItem("Escape Timer/Build Starter Scene")]
    public static void BuildScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientLight = new Color(0.08f, 0.09f, 0.13f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.025f, 0.035f, 0.055f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.025f;

        var lightObj = new GameObject("Directional Light");
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        var light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 0.65f;
        light.color = new Color(0.55f, 0.68f, 1f);

        MakeCube("Floor", new Vector3(0, -0.5f, 0), new Vector3(12, 1, 12), new Color(0.10f, 0.12f, 0.16f), false);

        // Outer boundary
        MakeCube("Wall_North", new Vector3(0, 1, 6), new Vector3(12, 2, 0.5f), new Color(0.16f, 0.19f, 0.25f), false);
        MakeCube("Wall_South", new Vector3(0, 1, -6), new Vector3(12, 2, 0.5f), new Color(0.16f, 0.19f, 0.25f), false);
        MakeCube("Wall_East", new Vector3(6, 1, 0), new Vector3(0.5f, 2, 12), new Color(0.16f, 0.19f, 0.25f), false);
        MakeCube("Wall_West", new Vector3(-6, 1, 0), new Vector3(0.5f, 2, 12), new Color(0.16f, 0.19f, 0.25f), false);

        // Simple maze partitions, leaving openings to navigate around
        MakeCube("MazeWall_1", new Vector3(-2.5f, 1, -2), new Vector3(0.5f, 2, 6), new Color(0.20f, 0.23f, 0.30f), false);
        MakeCube("MazeWall_2", new Vector3(1.0f, 1, 1.5f), new Vector3(0.5f, 2, 7), new Color(0.20f, 0.23f, 0.30f), false);
        MakeCube("MazeWall_3", new Vector3(2.8f, 1, -3.5f), new Vector3(4.5f, 2, 0.5f), new Color(0.20f, 0.23f, 0.30f), false);
        MakeCube("MazeWall_4", new Vector3(-0.5f, 1, 4), new Vector3(4.5f, 2, 0.5f), new Color(0.20f, 0.23f, 0.30f), false);
        MakeCube("MazeWall_5", new Vector3(-4, 1, 1), new Vector3(3.5f, 2, 0.5f), new Color(0.20f, 0.23f, 0.30f), false);

        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(-4.5f, 1f, -4.5f);
        player.GetComponent<Renderer>().material.color = new Color(0.2f, 0.8f, 1f);
        var rb = player.AddComponent<Rigidbody>();
        player.AddComponent<EscapeTimerStarterPlayer>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        var plate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plate.name = "PressurePlate";
        plate.transform.position = new Vector3(3.8f, 0.05f, 3.5f);
        plate.transform.localScale = new Vector3(1.2f, 0.12f, 1.2f);
        plate.GetComponent<Renderer>().material.color = new Color(0.1f, 0.9f, 0.35f);
        plate.GetComponent<Collider>().isTrigger = true;
        plate.AddComponent<EscapeTimerStarterPlate>();

        var door = MakeCube("ExitDoor", new Vector3(4.7f, 1f, 5.4f), new Vector3(1.8f, 2f, 0.25f), new Color(0.8f, 0.2f, 0.15f), false);
        door.tag = "Exit";

        var exit = new GameObject("ExitZone");
        exit.transform.position = new Vector3(4.7f, 0.8f, 5.0f);
        var exitCollider = exit.AddComponent<BoxCollider>();
        exitCollider.isTrigger = true;
        exitCollider.size = new Vector3(1.8f, 1.8f, 1.0f);
        exit.AddComponent<EscapeTimerStarterExit>();

        var cameraObj = new GameObject("Main Camera");
        cameraObj.tag = "MainCamera";
        var cam = cameraObj.AddComponent<Camera>();
        cam.fieldOfView = 60;
        cameraObj.transform.position = new Vector3(-1, 8, -11);
        cameraObj.transform.LookAt(player.transform);

        var manager = new GameObject("GameManager");
        var builder = manager.AddComponent<EscapeTimerStarterManager>();
        builder.player = player.transform;
        builder.pressurePlate = plate;
        builder.exitDoor = door;
        builder.exitZone = exit;

        var follow = cameraObj.AddComponent<EscapeTimerStarterCamera>();
        follow.target = player.transform;

        string folder = "Assets/_Project/Scenes";
        if (!AssetDatabase.IsValidFolder("Assets/_Project")) AssetDatabase.CreateFolder("Assets", "_Project");
        if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets/_Project", "Scenes");
        EditorSceneManager.SaveScene(scene, folder + "/EscapeTimer.unity");
        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Escape Timer", "Starter scene created at Assets/_Project/Scenes/EscapeTimer.unity", "OK");
    }

    static GameObject MakeCube(string name, Vector3 pos, Vector3 scale, Color color, bool trigger)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        var renderer = obj.GetComponent<Renderer>();
        renderer.material.color = color;
        var collider = obj.GetComponent<Collider>();
        if (collider != null) collider.isTrigger = trigger;
        return obj;
    }
}
