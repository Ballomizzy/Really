using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Slider gameVolume, gameSensitivity;

    public float GetGameVolumeValue() { return gameVolume.value; }
    public float GetGameSensititvityValue() { return gameSensitivity.value; }
}
