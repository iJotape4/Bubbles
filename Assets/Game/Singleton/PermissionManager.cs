using UnityEngine;
using System.IO;

public class PermissionManager : MonoBehaviour
{
    void Start()
    {
        // Check if permission is granted
        if (!HasPermission())
        {
            // Permission not granted, request it
            RequestPermission();
        }
        else
        {
            // Permission already granted, proceed with accessing the persistent data path
            Debug.Log("Permission already granted.");
            AccessPersistentDataPath();
        }
    }

    bool HasPermission()
    {
        // Check if the application has write access to the persistent data path
        try
        {
            // Attempt to write to the persistent data path
            string testFilePath = Path.Combine(Application.persistentDataPath, "test.txt");
            File.WriteAllText(testFilePath, "Test");
            File.Delete(testFilePath); // Clean up test file
            return true;
        }
        catch (System.Exception)
        {
            // Permission denied or other error
            return false;
        }
    }

    void RequestPermission()
    {
        // Display a message or UI prompting the user to grant permission
        Debug.Log("Please grant permission to write to the persistent data path. "+ Application.persistentDataPath);
    }

    void AccessPersistentDataPath()
    {
        // Proceed with accessing the persistent data path
        // Your code for writing to the persistent data path goes here
    }
}
