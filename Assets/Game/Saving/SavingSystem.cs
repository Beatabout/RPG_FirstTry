using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    [RequireComponent(typeof(SavingWrapper))]
    public class SavingSystem : MonoBehaviour
    {
        

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mundo you bustard!");
                stream.Write(bytes, 0, bytes.Length);
            }
            // byte[] holaMundo = {0xc2, 0xa1, 0x48, 111, 108, 97, 0x20, 77, 117, 110, 100, 111, 33};
            // foreach(byte character in holaMundo){
            //     stream.WriteByte(character);
            // }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                print(Encoding.UTF8.GetString(buffer));
                
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".gsd") ;
        }
    }
}

