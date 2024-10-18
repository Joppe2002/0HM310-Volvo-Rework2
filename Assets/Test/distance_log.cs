using System.Collections;
using System.Collections.Generic;
using System.IO;    // Required for file handling
using System;      // Required for accessing system paths
using UnityEngine;  // Required for Unity's classes

public class distance_log : MonoBehaviour
{
    private string filePath;  // The path to the file where distance values will be saved
    private float saveInterval = 0.01f;  // Save every 0.01 seconds
    private float nextSaveTime = 0f;    // Keeps track of when to save next

    private WaypointMover_AV waypointMover_AV;  // Reference to WaypointMover_AV script

    void Start()
    {
        // Find the WaypointMover_AV script on the same GameObject or another GameObject
        waypointMover_AV = FindObjectOfType<WaypointMover_AV>(); // Find it in the scene

        if (waypointMover_AV == null)
        {
            Debug.LogError("WaypointMover_AV script not found in the scene!");
        }

        // Example: Absolute path to a specific directory (change this to your desired path)
        string customPath = "C:\\Users\\neppo\\OneDrive\\Bureaublad\\";  // On macOS

        // Ensure the directory exists (create it if it doesn't)
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
        }

        // Create a unique file name based on the current date and time (YYYYMMDD_HHmmss)
        string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = "Distance_" + timeStamp + ".txt";

        // Define the file path for the new file in the custom folder
        filePath = Path.Combine(customPath, fileName);

        Debug.Log("Data values will be saved at: " + filePath);
    }

    void Update()
    {
        // Get the current in-game time in seconds
        float gameTime = Time.time;

        // Check if the next save time has been reached
        if (gameTime >= nextSaveTime && waypointMover_AV != null)
        {
            // Access the distance from the WaypointMover_AV script
            float distance = waypointMover_AV.distance;

            // Save the distance value to the file
            SaveValueToFile(gameTime, distance);

            // Schedule the next save time (0.01 seconds later)
            nextSaveTime = gameTime + saveInterval;
        }
    }

    // This method writes the distance value at the current in-game time to a file
    void SaveValueToFile(float time, float value)
    {
        try
        {
            // Append the distance value and game time to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))  // 'true' to append to the file
            {
                writer.WriteLine(time.ToString("F2") + ";" + value.ToString("F4"));
            }

            //Debug.Log("Saved distance at time " + time.ToString("F2") + " d: " + value.ToString("F4"));
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save data: " + e.Message);
        }
    }
}