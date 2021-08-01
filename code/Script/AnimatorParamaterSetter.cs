using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorParamGroup
{
    public List<AnimatorFloatParam> FloatParams = new List<AnimatorFloatParam>();
    public List<AnimatorIntParam> IntParams = new List<AnimatorIntParam>();
    public List<AnimatorBoolParam> BoolParams = new List<AnimatorBoolParam>();
    public List<AnimatorTriggerParam> TriggerParams = new List<AnimatorTriggerParam>();

    public void SetAllParams(Animator anim)
    {
        foreach (var p in FloatParams)
            anim.SetFloat(p.paramName, p.param);
        foreach (var p in IntParams)
            anim.SetInteger(p.paramName, p.param);
        foreach (var p in BoolParams)
            anim.SetBool(p.paramName, p.param);
        foreach (var p in TriggerParams)
            anim.SetTrigger(p.paramName);
    }
}
[System.Serializable]
public class AnimatorParam { public string paramName; public float delay;[System.NonSerialized] public bool hasBeenSet = false; }
[System.Serializable]
public class AnimatorFloatParam : AnimatorParam { public float param; }
[System.Serializable]
public class AnimatorIntParam : AnimatorParam { public int param; }
[System.Serializable]
public class AnimatorBoolParam : AnimatorParam { public bool param; }
[System.Serializable]
public class AnimatorTriggerParam : AnimatorParam { }

public class AnimatorParamaterSetter : MonoBehaviour
{
    public AnimatorParamGroup paramsToSet = new AnimatorParamGroup();

    [Header("debug")]
    public float startTime;
    public Animator anim;

    private void Start()
    {
        startTime = Time.unscaledTime;

        anim = GetComponent<Animator>();
        if (!anim) return;

        StartCoroutine(coroutine());
    }

    IEnumerator coroutine()
    {
        do
        {
            yield return null;
            foreach (var p in paramsToSet.FloatParams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting float param {p.paramName} to val {p.param}");
                    anim.SetFloat(p.paramName, p.param);
                    p.hasBeenSet = true;
                }
            foreach (var p in paramsToSet.IntParams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting int param {p.paramName} to val {p.param}");
                    anim.SetInteger(p.paramName, p.param);
                    p.hasBeenSet = true;
                }
            foreach (var p in paramsToSet.BoolParams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting bool param {p.paramName} to val {p.param}");
                    anim.SetBool(p.paramName, p.param);
                    p.hasBeenSet = true;
                }
            foreach (var p in paramsToSet.TriggerParams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting trigger {p.paramName}");
                    anim.SetTrigger(p.paramName);
                    p.hasBeenSet = true;
                }
            if (!AreAllParamsSet())
                yield return null;
            else
                break;
        }
        while (true);

        Debug.Log("finished setting all animator params");
    }

    private bool AreAllParamsSet()
    {
        bool checkList(IEnumerable<AnimatorParam> list)
        {
            foreach (var p in list)
                if (Time.time - startTime < p.delay)
                    return false;
            return true;
        }

        return checkList(paramsToSet.FloatParams) && checkList(paramsToSet.IntParams) && checkList(paramsToSet.BoolParams) && checkList(paramsToSet.TriggerParams);
    }
}
