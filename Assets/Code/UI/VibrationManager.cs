using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    // make it globally accessible like Handheld.Vibrate()
    public static VibrationManager Instance;

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject vibrator;
    private AndroidJavaClass unityPlayer;
#endif

    void Awake()
    {
        // singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
#if UNITY_ANDROID && !UNITY_EDITOR
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Vibrate with given strength (0-1) and duration in milliseconds.
    /// On iOS, only duration is used.
    /// </summary>
    public void Vibrate(float strength = 1f, int durationMs = 100)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator == null) return;

        long[] pattern = { 0, durationMs }; // delay 0, vibrate duration
        int amplitude = Mathf.Clamp((int)(strength * 255), 1, 255);

        // For API 26+ use amplitude pattern
        vibrator.Call("vibrate", pattern, -1);
#elif UNITY_IOS && !UNITY_EDITOR
        // iOS only supports fixed taptic feedback, no duration/strength
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
