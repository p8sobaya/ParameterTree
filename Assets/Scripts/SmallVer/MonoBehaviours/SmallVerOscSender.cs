using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using extOSC;
using ParameterTree.Utility;
using ParameterTree.Utility.GuiTools;
using Utility.HolderInterface;

namespace ParameterTree
{
    public class SmallVerOscSender : MonoBehaviour, IImguiContent
    {
        
        [SerializeField] private string _prefixForParameter = "/param";
        
        
        [SerializeField] private GameObject _dataHolder;
        [SerializeField] private bool _isFromStreamingAssets;
        [SerializeField] private string _fileName;
        IOutputHolder<List<List<string>>> _currentStrings;
        
        private OSCTransmitter _transmitter;
        private List<(string host, int ip)> _hosts = new List<(string host, int ip)>();

        private float _valueTestPattern = 0f;
        private float _valueFluidVisualization = 0f;
        private float _valueSensorCursor = 0f;
        
        private void Start()
        {
            _transmitter = gameObject.AddComponent<OSCTransmitter>();
            SetHosts();
            
            _currentStrings = _dataHolder.GetComponent<IOutputHolder<List<List<string>>>>();
        }

        private void Update()
        {
            
        }

        public void SendParameters(string hostName, int port)
        {
            _transmitter.RemoteHost = hostName;
            _transmitter.RemotePort = port;
            SendParameters();
        }
        
        public void SendParameters()
        {
            foreach (var lstr in _currentStrings.OutputObject)
            {
                if(lstr==null) continue;
                if(lstr.Count<3) continue;
                var oscMessage = new OSCMessage(_prefixForParameter + lstr[0]);
                oscMessage.AddValue(OSCValue.String(lstr[1]));
                oscMessage.AddValue(OSCValue.String(lstr[2]));
                GetComponent<OSCTransmitter>().Send(oscMessage);
            }
        }
        
        private void SetHosts()
        {
            List<string> stringsFromFile = _isFromStreamingAssets ?
                    FileToStringList.ReadFromStreamingAssets(_fileName) : FileToStringList.Read(_fileName);
            
            _hosts.Clear();
            string splitter = ",";
            for(int i = 0; i < stringsFromFile.Count; i++)
            {
                if (i==0)
                {
                    splitter = stringsFromFile[i];
                    continue;
                }
                string[] splited = stringsFromFile[i].Split(splitter);
                
                if (splited.Length != 2)
                {
                    Debug.LogError("Hosts file is not correct");
                    return;
                }
                _hosts.Add((splited[0], int.Parse(splited[1])));
            }
                
        }
        public string NameImguiContent => "Osc Sender";
        public void VoidImguiContent()
        {
            GUI.color = Color.cyan;
            if (GUILayout.Button("Send Parameters"))
            {
                foreach (var hostIp in _hosts)
                {
                    SendParameters(hostIp.host, hostIp.ip);
                }
            }
            GUI.color = Color.white;
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Test Pattern", GUILayout.Width(200));
            _valueTestPattern = GUILayout.HorizontalSlider(_valueTestPattern, 0f, 1f, GUILayout.Width(300));
            GUILayout.Label( _valueTestPattern.ToString("F3"));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Fluid Visualization", GUILayout.Width(200));
            _valueFluidVisualization = GUILayout.HorizontalSlider(_valueFluidVisualization, 0f, 1f, GUILayout.Width(300));
            GUILayout.Label( _valueFluidVisualization.ToString("F3"));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Sensor Cursor", GUILayout.Width(200));
            _valueSensorCursor = GUILayout.HorizontalSlider(_valueSensorCursor, 0f, 1f, GUILayout.Width(300));
            GUILayout.Label( _valueSensorCursor.ToString("F3"));
            GUILayout.EndHorizontal();
            
            for(int i = 0; i < _hosts.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(_hosts[i].host, GUILayout.Width(200));
                GUILayout.Label(_hosts[i].ip.ToString(), GUILayout.Width(200));
                GUILayout.EndHorizontal();
            }
        }

    }
}