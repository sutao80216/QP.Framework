using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
namespace QP.Framework{
    public class DelegateDefined
    {
        [CSharpCallLua]
        public delegate void DStringInt(string s, int i);
        [CSharpCallLua]
        public delegate void DStringFloat(string s, float f);
        [CSharpCallLua]
        public delegate void DString(string s);
        [CSharpCallLua]
        public delegate void DInt(int i);

        [CSharpCallLua]
        public delegate void DVoid();
    }
} 

