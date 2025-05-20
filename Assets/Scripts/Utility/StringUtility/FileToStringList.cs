using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ParameterTree.Utility
{
    public static class FileToStringList
    {
        public static List<string> ReadFromStreamingAssets(string path)
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, path);
            return Read(fullPath);
        }

        public static List<string> Read(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    list.Add(sr.ReadLine());
                }
            }
            return list;
        }
        
        public static void WriteToStreamingAssets(string path, List<string> list)
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, path);
            Write(fullPath, list);
        }
        
        public static void Write(string path, List<string> list)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var line in list)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}

