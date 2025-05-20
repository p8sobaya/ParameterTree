using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ParameterTree.Utility;
using ParameterTree.Utility.GuiTools;
using Utility.HolderInterface;


namespace ParameterTree
{
    public class ImguiSmallVer : MonoBehaviour, IImguiContent, IOutputHolder<List<List<string>>>
    {
        [SerializeField] private bool _isFromStreamingAssets;
        [SerializeField] private string _fileName= "ImguiSmallVer/Default.txt";

        [SerializeField] private char _separatorMain;
        [SerializeField] private char _separatorSub;
        
        private List<List<string>> _currentStrings;
        public List<List<string>> OutputObject { get => _currentStrings; set => _currentStrings = value; }

        private Vector2 _scrollPosition;
         
        void Awake()
        {
            LoadOfFileName(_fileName);
        }

        private void ShowTextboxOfLlstring(List<List<string>> llstring)
        {
            GUILayout.BeginVertical( GUILayout.Width(1000), GUILayout.Height(500));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < llstring.Count; i++)
            {
                if (llstring[i] != null && llstring[i].Count < 3)
                {
                    continue;
                }
                GUILayout.BeginHorizontal();
                llstring[i][0]=GUILayout.TextField(llstring[i][0], GUILayout.Width(300));
                llstring[i][1]=GUILayout.TextField(llstring[i][1], GUILayout.Width(500));
                llstring[i][2]=GUILayout.TextField(llstring[i][2], GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void LoadOfFileName(string fileName)
        {
            if (_isFromStreamingAssets)
            {
                (string separator, List<string> strings) = SeparateFirstElement(FileToStringList.ReadFromStreamingAssets(fileName)); 
                _currentStrings = SplitWithString(strings, separator);
            }
            else
            {
                (string separator, List<string> strings) = SeparateFirstElement(FileToStringList.Read(fileName)); 
                _currentStrings = SplitWithString(strings, separator);
            }
        }
        
        private (T,List<T>) SeparateFirstElement<T>(List<T> list)
        {
            return (list[0],list.GetRange(1, list.Count - 1));
        }

        private List<List<string>> SplitWithString(List<string> strings, string separator)
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var str in strings)
            {
                result.Add(new List<string>(str.Split(separator)));
            }
            return result;
        }

        private void SaveOnFileName(string fileName)
        {
            int valMax = MaxFromListList(_currentStrings);
            string separator = "";
            if (valMax == 0)
            {
                separator = _separatorMain.ToString();
            }
            else
            {
                separator = _separatorSub + ConsecutiveLetters(_separatorMain, valMax + 1) +_separatorSub;
            }
            
            List<string> content = new List<string>{separator}.Concat(ConnectWithString(_currentStrings, separator)).ToList();
            
            if (_isFromStreamingAssets)
            {
                FileToStringList.WriteToStreamingAssets(fileName, content);
            }
            else
            {
                FileToStringList.Write(fileName, content);
            }
        }

        private string ConsecutiveLetters(char c, int i)
        {
            string result = "";
            for (int j = 0; j < i; j++)
            {
                result += c;
            }
            return result;
        }

        private List<string> ConnectWithString(List<List<string>> llstring, string s)
        {
            List<string> result = new List<string>();
            foreach (var list in llstring)
            {
                result.Add(string.Join(s, list));
            }
            return result;
        }
        
        private int MaxFromListList(List<List<string>> listList)
        {
            int max = 0;
            foreach (var list in listList)
            {
                foreach (var str in list)
                {
                    max = Mathf.Max(max, ContainConsecutiveLetters(str, _separatorMain));
                }
            }
            return max;
        }

        private int ContainConsecutiveLetters(string sentence, char c)
        {
            int maxCount = 0;
            int currentCount = 0;
            for (int i = 0; i < sentence.Length; i++)
            {
                if (sentence[i] == c)
                {
                    currentCount++;
                }
                else
                {
                    maxCount = Mathf.Max(maxCount, currentCount);
                    currentCount = 0;
                }
            }
            maxCount = Mathf.Max(maxCount, currentCount);

            return maxCount;
        }

        public string NameImguiContent => "Data Editor";
        public void VoidImguiContent()
        {
            GUILayout.Label("ImguiSmallVer");
            GUILayout.Label("FileName");
            _fileName = GUILayout.TextField(_fileName);
            if (GUILayout.Button("Load"))
            {
                LoadOfFileName(_fileName);
            }
            if (GUILayout.Button("Save"))
            {
                SaveOnFileName(_fileName);
            }
            if(_currentStrings == null)
            {
                _currentStrings = new List<List<string>>();
            }
            ShowTextboxOfLlstring(_currentStrings); 
        }
    }

}
