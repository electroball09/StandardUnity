using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderUtil
{
    static bool update = true;

    static List<(Object, System.Action<ScriptableRenderContext, Camera>)> objectsToUpdate = new List<(Object, System.Action<ScriptableRenderContext, Camera>)>();

    [RuntimeInitializeOnLoadMethod]
    static void init()
    {
        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;

        ComReg.AddCom("render.utilupdate", (bool val) => update = val, "sets begincamerarendering events to fire");
    }

    private static void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (!update)
            return;

        objectsToUpdate.RemoveAll((tuple) => tuple.Item1 == null);

        foreach (var obj in objectsToUpdate)
        {
            obj.Item2(arg1, arg2);
        }
    }

    public static void RegisterBeginRender(Object go, System.Action<ScriptableRenderContext, Camera> action)
    {
        objectsToUpdate.Add((go, action));
    }
}
