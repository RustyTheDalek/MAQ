using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine.UI;

public static class SaveLoad
{
    public static List<PlanetData> LocalExoData = new List<PlanetData>();

    public static PlanetAverages localAvg = new PlanetAverages(); 

    //TODO:Add Folder 
    static string animalDir = "/";

    //FilePath of animals
    public static string animalsFP
    {
        get
        {
            return Application.persistentDataPath + animalDir;
        }
    }

    public static void createFolders()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/Thumbnails"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Thumbnails");
        }
    }

    //Saves list of downloaded planets to text file
    public static void SavePlanetDataList(ref List<PlanetData> PVList, string filename, string extension)
    {
//        Debug.Log("Starting local save of " + filename);

        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream file = File.Create (Application.persistentDataPath + "/" + filename + extension);
        
        bf.Serialize(file, PVList);
        
        file.Close();

        Debug.Log("File " + filename + " saved");
    }

    public static bool LoadPlanetDataList(ref List<PlanetData> PVList, string filename, string extension)
    {
        //Debug.Log("Starting local load of " + filename);

        if(File.Exists(Application.persistentDataPath + "/" + filename + extension)) 
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + filename + extension, FileMode.Open);

            PVList = (List<PlanetData>)bf.Deserialize(file);

            file.Close();

            //Debug.Log("File " + filename + " loaded");

            return true; 
        }
        else
        {
            Debug.LogError("File : " + filename + extension + " does not exist");

            OnlineExoData.GetLocalCopy();

            return false;
        }
    }

    public static void SavePlanetAverages(ref PlanetAverages PA, string filename, string extension)
    {
        Debug.Log("Starting local save of " + filename);
        
        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream file = File.Create (Application.persistentDataPath + "/" + filename + extension);
        
        bf.Serialize(file, PA);
        
        file.Close();
        
        Debug.Log("File " + filename + " saved");
    }

    public static bool LoadPlanetAverages(ref PlanetAverages PA, string filename, string extension)
    {
        //Debug.Log("Starting local load of " + filename);
        
        if(File.Exists(Application.persistentDataPath + "/" + filename + extension)) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename + extension, FileMode.Open);
            
            PA = (PlanetAverages)bf.Deserialize(file);
            
            file.Close();

            //PA.debugVariables();
            
            //Debug.Log("File " + filename + " loaded");
            
            return true; 
        }
        else
        {
            Debug.LogError("File : " + filename + extension + " does not exist");
            
            return false;
        }
    }

    public static string[] LoadAnimals()
    {
        string[] animals;
        
        animals = Directory.GetFiles(animalsFP, "*.an");

        if(animals.Length > 0)
        {
//            Debug.Log("Files found");

            return animals;
        }

        Debug.LogWarning("No Animals found");

        return animals;
    }

    public static Sprite loadImage(string fileName)
    {
        if(File.Exists(Application.persistentDataPath + "/Thumbnails/" + fileName + ".png")) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            FileStream file = File.Open(Application.persistentDataPath + "/Thumbnails/" + fileName + ".png", FileMode.Open);

            Debug.Log("here");
            Image test = (Image)bf.Deserialize(file);
            Debug.Log("here");
            file.Close();
        }

        Debug.LogWarning("No Image found");

        return null;
    }

    public static void deleteAnimal(string name)
    {
        if(File.Exists(Application.persistentDataPath + "/" + name + ".an"))
        {
            File.Delete(Application.persistentDataPath + "/" + name + ".an");

            File.Delete(Application.persistentDataPath + "/Thumbnails/" + name + ".png");
        }
    }

    public static void SaveGame()
    {
        Debug.Log("Starting save");
        
        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream file = File.Create (Application.persistentDataPath + "/save.sg");
        
        bf.Serialize(file, Game.Current);
        
        file.Close();
        
        Debug.Log("Game Saved");
    }

    public static void LoadGame()
    {
        Debug.Log("Loading game");
        
        if(File.Exists(Application.persistentDataPath + "/save.sg")) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            FileStream file = File.Open(Application.persistentDataPath + "/save.sg", FileMode.Open);
            
            Game.Current = (Game)bf.Deserialize(file);
            
            file.Close();
        }
        
        Debug.Log("Game Loaded");    
    }

    //TODO:Get rid function if not needed
    //Saves list of players planets to text file
//    public static void SavePlanet(PlanetVariables pV) 
//    {
//        savedPlanets.Add(pV);
//
//        BinaryFormatter bf = new BinaryFormatter();
//
//        FileStream file = File.Create (Application.persistentDataPath + "/savedPlanets.txt");
//
//        bf.Serialize(file, SaveLoad.savedPlanets);
//
//        file.Close();
//    }

    //Checks to see if the Local Data is open in Memory
    public static bool LocalDataAvailable()
    {
        if(LocalExoData.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool localAvgAvailable()
    {
        if(localAvg != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Checks to see if a save of the local file exists
    public static bool LocalDataExists(string filename, string extension)
    {
//        Debug.Log(Application.persistentDataPath + "/" + filename + extension);

        if(File.Exists(Application.persistentDataPath + "/" + filename + extension)) 
        {
//            Debug.Log("File does exist");
            return true;
        }
        else
        {
//            Debug.Log("File Doesn't exist");
            return false;
        }
    }



    public static void delete(string filePath, string extension)
    {
        File.Delete(Application.persistentDataPath + "/" + filePath + extension);
    }
}