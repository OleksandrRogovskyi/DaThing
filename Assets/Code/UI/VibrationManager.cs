using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject vibrator;
    private AndroidJavaClass vibrationEffectClass;
    private int sdkVersion;
#endif

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION");
            sdkVersion = version.GetStatic<int>("SDK_INT");

            vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Vibrate(float strength, int durationMs)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator == null) return;

        int amplitude = Mathf.Clamp((int)(strength * 255), 1, 255);

        if (sdkVersion >= 26)
        {
            // Use VibrationEffect for modern Android
            AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                "createOneShot", (long)durationMs, amplitude
            );
            vibrator.Call("vibrate", vibrationEffect);
        }
        else
        {
            // Legacy vibration (no amplitude support)
            vibrator.Call("vibrate", (long)durationMs);
        }

#elif UNITY_IOS && !UNITY_EDITOR
        // iOS has fixed vibration intensity
        Handheld.Vibrate();
#else
        Debug.Log($"Vibrate({strength}, {durationMs})");
#endif
    }

    public void StopVibration()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        vibrator?.Call("cancel");
#endif
    }
}
