using System.Collections;
using System.Collections.Generic;
using ParameterTree.Utility.GuiTools;
using UnityEngine;

public class ImguiContentsMaster : MonoBehaviour
{
    [SerializeField] private Vector2Int _sizeOfContents = new Vector2Int(1000, 500);
    [SerializeField] private List<GameObject> _objectsList;
    private List<IImguiContent> _contents;

    private int _currSelected = 0;
    private const int _numsInRow = 5;
    void Start()
    {
        _contents = new List<IImguiContent>();
        foreach (var obj in _objectsList)
        {
            var content = obj.GetComponent<IImguiContent>();
            if (content != null)
            {
                _contents.Add(content);
            }
        }
    }

    
    private void OnGUI()
    {
        GUILayout.BeginVertical( GUILayout.Width(_sizeOfContents.x), GUILayout.Height(_sizeOfContents.y));
        
        GUI.color = new Color(1.0f,0.5f,0.0f);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < _contents.Count; i++)
        {
            if (i % _numsInRow == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            
            if (GUILayout.Button(_contents[i].NameImguiContent))
            {
                _currSelected = i;
            }
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        
        _contents[_currSelected].VoidImguiContent();
        
        GUILayout.EndVertical();
    }
}
