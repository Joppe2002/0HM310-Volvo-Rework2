using System.Collections;
using System.Collections.Generic;
using System.IO;    // Required for file handling
using System;       // Required for accessing system paths
using UnityEngine;  // Required for Unity's classes

public class keypress_log : MonoBehaviour
{
    private string filePath;  // The path to the file where the key press will be saved

    void Start()
    {
        // Example: Absolute path to a specific directory (change this to your desired path)
        string customPath = "C:\\Users\\neppo\\OneDrive\\Bureaublad\\";  // On macOS

        // Ensure the directory exists (create it if it doesn't)
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
        }

        // Create a unique file name based on the current date and time (YYYYMMDD_HHMMSS)
        string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = "KeyPress_" + timeStamp + ".txt";

        // Define the file path for the new file in the custom folder
        filePath = Path.Combine(customPath, fileName);

        Debug.Log("Key press will be saved at: " + filePath);
    }

    void Update()
    {
        // Check if the 'L' key is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Call the method to save the key press to the file
            SaveKeyPressToFile();
        }
    }

    // This method writes the 'L' key press to a .txt file in the custom folder
    void SaveKeyPressToFile()
    {
        try
        {
            // Get the game runtime in seconds
            float gameTime = Time.time;

            // Append the 'L' key press and game runtime (in seconds) to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))  // 'true' to append to file
            {
                writer.WriteLine(gameTime.ToString("F2"));
            }

            Debug.Log("'L' key press saved successfully at runtime " + gameTime.ToString("F2") + " seconds");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save key press: " + e.Message);
        }
    }
}