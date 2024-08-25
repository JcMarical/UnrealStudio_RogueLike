using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class UIType
    {
        private string path;
        private string name;
        //获取面板的存储路径与名称
        public string Path { get => path; }
        public string Name { get => name; }
        public UIType(string ui_path,string ui_name)
        {
            path = ui_path;
            name = ui_name;
        }
    }
}
