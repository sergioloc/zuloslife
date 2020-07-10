using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class GlobalLight2D : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Experimental.Rendering.Universal.Light2D globalLight2D;

    void Start()
    {
        StartCoroutine(GetRemoteConfigValues());
    }

    private IEnumerator GetRemoteConfigValues(){
        yield return new WaitForSeconds(0.5f);
        globalLight2D.intensity = ConfigManager.appConfig.GetInt("globalLight") / 100;
        if (globalLight2D.intensity == 0) globalLight2D.intensity = 0.5f;
    }

}
