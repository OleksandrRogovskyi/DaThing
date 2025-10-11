using System.Text;
using TMPro; // Important: Make sure TextMeshPro is imported into your project!
using UnityEngine;

/// <summary>
/// Captures all Debug.Log messages and displays them in a TextMeshPro component
/// for real-time in-game debugging, especially useful on standalone builds like Android.
/// </summary>
public class Console : MonoBehaviour
{
    // Assign this component in the Inspector to a TextMeshPro UGUI object
    [Tooltip("The TextMeshProUGUI component that will display the console output.")]
    public TextMeshProUGUI consoleText;

    [Tooltip("The maximum number of lines to display to prevent lag and memory issues.")]
    public int maxLines = 20;

    private readonly StringBuilder logBuilder = new StringBuilder();

    /// <summary>
    /// Ensures this GameObject and script persist across scenes and starts listening for logs.
    /// </summary>
    void Awake()
    {
        // Optional: Keep this object alive across scene loads
        DontDestroyOnLoad(gameObject);

        // Register the method to be called whenever a log message is sent
        Application.logMessageReceived += HandleLog;

        if (consoleText == null)
        {
            Debug.LogError("InGameConsole: TextMeshProUGUI component is not assigned. Disabling console.");
            enabled = false;
        }
    }

    /// <summary>
    /// Removes the log listener when the object is disabled or destroyed.
    /// </summary>
    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    /// <summary>
    /// Clears the in-game console display and the underlying log buffer.
    /// This method can be connected directly to a Unity UI Button's onClick event.
    /// </summary>
    public void ClearConsole()
    {
        logBuilder.Clear();
        if (consoleText != null)
        {
            consoleText.text = "";
        }
    }

    /// <summary>
    /// The callback method for processing log messages.
    /// </summary>
    /// <param name="logString">DThe message passed to the log function.</param>
    /// <param name="stackTrace">The stack trace (usually only available for errors).</param>
    /// <param name="type">The type of log message (Log, Warning, Error).</param>
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 1. Format the log message with color coding
        string formattedMessage = FormatLogMessage(logString, type);

        // 2. Append the new message to the StringBuilder
        logBuilder.AppendLine(formattedMessage);

        // 3. Check for line limit and truncate if necessary
        TruncateLog(maxLines);

        // 4. Update the TextMeshPro component
        consoleText.text = logBuilder.ToString();
    }

    /// <summary>
    /// Helper to format the log message with color based on LogType.
    /// </summary>
    private string FormatLogMessage(string logString, LogType type)
    {
        // Add a timestamp for better debugging context
        string prefix = $"[{Time.time:0.0}] ";
        string color = "white"; // Default for LogType.Log

        switch (type)
        {
            case LogType.Error:
                color = "red";
                break;
            case LogType.Warning:
                color = "yellow";
                break;
            case LogType.Exception:
                color = "magenta";
                break;
            case LogType.Assert:
                color = "cyan";
                break;
        }

        // Use TextMeshPro rich text tags for coloring
        return $"{prefix}<color={color}>{type}: {logString}</color>";
    }

    /// <summary>
    /// Truncates the log history if it exceeds the max line count.
    /// </summary>
    private void TruncateLog(int max)
    {
        // This is a simple (but slightly inefficient) way to handle line limits with StringBuilder
        // For very high-frequency logging, a List<string> buffer might be better, but this is simpler.

        string fullLog = logBuilder.ToString();
        string[] lines = fullLog.Split('\n');

        if (lines.Length > max)
        {
            logBuilder.Clear();

            // Rebuild the log only with the last 'max' lines
            for (int i = lines.Length - max; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    logBuilder.AppendLine(lines[i]);
                }
            }
        }
    }
}
