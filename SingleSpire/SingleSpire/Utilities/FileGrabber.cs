using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpireVenture.Utilities
{
    public class FileGrabber
    {
        public static PlayerSave getPlayerSave(string playerName)
        {
            String directory;

            String currentPath = Directory.GetCurrentDirectory();
            directory = Path.Combine(currentPath, "Saves");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            String savename = Path.Combine(directory, playerName + ".sav");
            if (File.Exists(savename))
            {
                Stream streamRead = File.OpenRead(savename);
                BinaryFormatter binaryRead = new BinaryFormatter();
                PlayerSave player = (PlayerSave)binaryRead.Deserialize(streamRead);
                streamRead.Close();
                return player;
            }
            else
            {
                PlayerSave playerSave = new PlayerSave(playerName);
                Stream streamWrite = File.Create(savename);
                BinaryFormatter binaryWrite = new BinaryFormatter();
                binaryWrite.Serialize(streamWrite, playerSave);
                streamWrite.Close();
                return playerSave;
            }
        }

        public static void SavePlayer(bool isSingleplayer, PlayerSave player)
        {
            String directory;

            String currentPath = Directory.GetCurrentDirectory();
            directory = Path.Combine(currentPath, "Saves");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            String savename = Path.Combine(directory, player.Username + ".sav");

            Stream streamWrite = File.Create(savename);
            BinaryFormatter binaryWrite = new BinaryFormatter();
            binaryWrite.Serialize(streamWrite, player);
            streamWrite.Close();
        }

        public static string[] findLocalProfiles()
        {
            String directory;

            String currentPath = Directory.GetCurrentDirectory();
            directory = Path.Combine(currentPath, "Saves");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return Directory.GetFiles(directory, "*.sav");
        }

        public static void createNewProfile(string name)
        {
            String directory;

            String currentPath = Directory.GetCurrentDirectory();
            directory = Path.Combine(currentPath, "Saves");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            String profileFilePath = Path.Combine(directory, name + ".sav");

            PlayerSave save = new PlayerSave(name);

            Stream streamWrite = File.Create(profileFilePath);
            BinaryFormatter binaryWrite = new BinaryFormatter();
            binaryWrite.Serialize(streamWrite, save);
            streamWrite.Close();
        }
    }
}
