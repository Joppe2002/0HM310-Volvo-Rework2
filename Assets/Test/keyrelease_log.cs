using System.Collections;
using System.Collections.Generic;
using System.IO;    // Required for file handling
using System;       // Required for accessing system paths
using UnityEngine;  // Required for Unity's classes

public class keyrelease_log : MonoBehaviour
{
    private string filePath;  // The path to the file where the key release will be saved

    void Start()
    {
        // Example: Absolute path to a specific directory (change this to your desired path)
        string customPath = "/Users/mikeberben/Documents/SF_Project_datalog";  // Change this to the desired path on Windows

        // Ensure the directory exists (create it if it doesn't)
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
        }

        // Create a unique file name based on the current date and time (YYYYMMDD_HHMMSS)
        string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = "KeyRelease_" + timeStamp + ".txt";

        // Define the file path for the new file in the custom folder
        filePath = Path.Combine(customPath, fileName);

        Debug.Log("Key release log will be saved at: " + filePath);
    }

    void Update()
    {
        // Check if the 'L' key is released
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            // Save the time when the key is released
            SaveKeyReleaseTimeToFile();
        }
    }

    // This method writes the time when the 'L' key is released to a .txt file
    void SaveKeyReleaseTimeToFile()
    {
        try
        {
            // Get the game runtime in seconds
            float gameTime = Time.time;

            // Append the game time when the key is released to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))  // 'true' to append to file
            {
                writer.WriteLine(gameTime.ToString("F2"));
            }

            Debug.Log("Up key release saved at runtime " + gameTime.ToString("F2") + " seconds");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save key release: " + e.Message);
        }
    }
}
