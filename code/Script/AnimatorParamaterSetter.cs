using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParamaterSetter : MonoBehaviour
{
    [System.Serializable]
    public class Param { public string paramName; public float delay; [System.NonSerialized] public bool hasBeenSet = false; }
    [System.Serializable]
    public class FloatParam : Param { public float param; }
    [System.Serializable]
    public class IntParam : Param { public int param; }
    [System.Serializable]
    public class BoolParam : Param { public bool param; }
    [System.Serializable]
    public class TriggerParam : Param { }

    public List<FloatParam> floatParams = new List<FloatParam>();
    public List<IntParam> intParams = new List<IntParam>();
    public List<BoolParam> boolparams = new List<BoolParam>();
    public List<TriggerParam> triggerParams = new List<TriggerParam>();

    [Header("debug")]
    public float startTime;
    public Animator anim;

    private void Start()
    {
        startTime = Time.unscaledTime;

        anim = GetComponent<Animator>();
        if (!anim) return;

        StartCoroutine(coroutine());

        foreach (var fp in floatParams)
            anim.SetFloat(fp.paramName, fp.param);
    }

    IEnumerator coroutine()
    {
        do
        {
            yield return null;
            foreach (var p in floatParams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting float param {p.paramName} to val {p.param}");
                    anim.SetFloat(p.paramName, p.param);
                    p.hasBeenSet = true;
                }
            foreach (var p in intParams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting int param {p.paramName} to val {p.param}");
                    anim.SetInteger(p.paramName, p.param);
                    p.hasBeenSet = true;
                }
            foreach (var p in boolparams)
                if (Time.time - startTime >= p.delay && !p.hasBeenSet)
                {
                    Debug.Log($"setting bool param {p.paramName} to val {p.param}");
                    anim.SetBool(p.paramName, p.param);
                    p.hasBeenSet = true;
                }
            foreach (var p in triggerParams)
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
        bool checkList(IEnumerable<Param> list)
        {
            foreach (var p in list)
                if (Time.time - startTime < p.delay)
                    return false;
            return true;
        }

        return checkList(floatParams) && checkList(intParams) && checkList(boolparams) && checkList(triggerParams);
    }
}
