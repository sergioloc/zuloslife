using UnityEngine;

public class GlobalLight2D : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Rendering.Universal.Light2D globalLight2D;

    void Start()
    {
        globalLight2D.intensity = GameBalance.GlobalLightPercent / 100f;
    }

}
