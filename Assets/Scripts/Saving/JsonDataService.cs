using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class JsonDataService : IDataService
{
    private string path_offset;

    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.Auto, // Automatically handle type information for polymorphic deserialization
        //PreserveReferencesHandling = PreserveReferencesHandling.Objects // Preserve object references during deserialization
    };

    public JsonDataService(string path_offset = null)
    {
        this.path_offset = path_offset;
    }

    private void CheckFolder()
    {
        string directoryPath = Application.persistentDataPath + path_offset;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Created directory: " + directoryPath);
        }
        else
        {
            Debug.Log("Directory already exists: " + directoryPath);
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + path_offset + relativePath;

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"{path} does not exist!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), jsonSerializerSettings);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
    }

    public bool SaveData<T>(string relativePath, T data)
    {
        CheckFolder();

        string path = Application.persistentDataPath + path_offset + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing file for the first time!");
            }
            using (FileStream stream = File.Create(path))
            {
                // Serialize the data object to JSON and write it to the file
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, jsonSerializerSettings);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                stream.Write(bytes, 0, bytes.Length);
            }

            Debug.Log("Data saved successfully to: " + path);
            return true;
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.LogError("Unauthorized access error: " + e.Message);
            return false;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogError("Directory not found error: " + e.Message);
            return false;
        }
        catch (PathTooLongException e)
        {
            Debug.LogError("Path too long error: " + e.Message);
            return false;
        }
        catch (IOException e)
        {
            Debug.LogError("IO error occurred: " + e.Message);
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError("An unexpected error occurred: " + e.Message);
            Debug.LogError("Stack Trace: " + e.StackTrace);

            // Check inner exception
            if (e.InnerException != null)
            {
                Debug.LogError("Inner Exception: " + e.InnerException.Message);
                Debug.LogError("Inner Exception Stack Trace: " + e.InnerException.StackTrace);
            }
            return false;
        }
    }

    public void DeleteFiles()
    {
        string path = Application.persistentDataPath + path_offset;

        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] files = directory.GetFiles();

        foreach (FileInfo file in files)
        {
            string fileName = file.Name;
            string filePath = Path.Combine(path, fileName);
            
            Debug.Log("File path to delete: " + filePath);
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Delete the file
                File.Delete(filePath);
                Debug.Log("Deleted file: " + fileName);
            }
            else
            {
                Debug.Log("File not found: " + fileName);
            }
        }
    }
}
