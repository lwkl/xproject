// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SLua
{
    using System.Collections.Generic;
    using System;

    public class CustomExport
    {
        public static void OnGetAssemblyToGenerateExtensionMethod(out List<string> list) {
            list = new List<string> {
                "Assembly-CSharp",
            };
        }

        public static void OnAddCustomClass(LuaCodeGen.ExportGenericDelegate add)
        {
			// below lines only used for demostrate how to add custom class to export, can be delete on your app

            add(typeof(System.Func<int>), null);
            add(typeof(System.Action<int, string>), null);
            add(typeof(System.Action<int, Dictionary<int, object>>), null);
            add(typeof(List<int>), "ListInt");
            add(typeof(Dictionary<int, string>), "DictIntStr");
            add(typeof(string), "String");

            // 添加Lean相关

            add(typeof(TweenAction), null);
            add(typeof(LeanTweenType), null);
            add(typeof(LTDescr), null);
            add(typeof(LTUtility), null);
            add(typeof(LeanTween), null);
            add(typeof(LTBezier), null);
            add(typeof(LTBezierPath), null);
            add(typeof(LTSpline), null);
            add(typeof(LTRect), null);
            add(typeof(LTEvent), null);
            add(typeof(LTGUI), null);


            // X相关
            add(typeof(XConf), null);
            add(typeof(XLoading), null);
            add(typeof(XApp), null);

            add(typeof(XPool), null);
            add(typeof(XSinglePool), null);
            add(typeof(XIntKeyPool), null);
            add(typeof(XStrKeyPool), null);
            add(typeof(XGOSinglePool), null);
            add(typeof(XGOIntKeyPool), null);
            add(typeof(XGOStrKeyPool), null);
            add(typeof(XPoolManager), null);
            add(typeof(XLoadDesc), null);
            add(typeof(XLoad), null);

            // add your custom class here
            // add( type, typename)
            // type is what you want to export
            // typename used for simplify generic type name or rename, like List<int> named to "ListInt", if not a generic type keep typename as null or rename as new type name
        }

        public static void OnAddCustomAssembly(ref List<string> list)
        {
            // add your custom assembly here
            // you can build a dll for 3rd library like ngui titled assembly name "NGUI", put it in Assets folder
            // add its name into list, slua will generate all exported interface automatically for you

            //list.Add("NGUI");
        }

        public static HashSet<string> OnAddCustomNamespace()
        {
            return new HashSet<string>
            {
                //"NLuaTest.Mock"
            };
        }

        // if uselist return a white list, don't check noUseList(black list) again
        public static void OnGetUseList(out List<string> list)
        {
            list = new List<string>
            {
                //"UnityEngine.GameObject",
            };
        }

        public static List<string> FunctionFilterList = new List<string>()
        {
            "UIWidget.showHandles",
            "UIWidget.showHandlesWithMoveTool",
            //nodecanvas
            "NodeCanvas.DialogueTrees.DialogueTree.CreateDialogueTree",
            "NodeCanvas.DialogueTrees.DialogueTree.Editor_CreateGraph",
            "NodeCanvas.DialogueTrees.StatementNode.OnNodeGUI",
            "NodeCanvas.DialogueTrees.StatementNode.drawdialoglistutil",
            "NodeCanvas.DialogueTrees.StatementNode.OnNodeInspectorGUI",
            "NodeCanvas.DialogueTrees.StatementNode.npcSelectList",
            "NodeCanvas.DialogueTrees.StatementNode.Editor_CreateGraph2",  
            //fuck camera path
            "CameraPath.ToXML",
            "CameraPath.FromXML",
            "CameraPathAnimator.ToXML",
            "CameraPathAnimator.FromXML",
            "CameraPathControlPoint.ToXML",
            "CameraPathControlPoint.FromXML",
            "CameraPathPoint.ToXML",
            "CameraPathPoint.FromXML",
            "CameraPathSpeed.ToXML",
            "CameraPathSpeed.FromXML",
            "CameraPathEvent.ToXML",
            "CameraPathEvent.FromXML",
            "DayNightController.updateInEditMode",
            "Framework.Base.ScenePrefab.DumpLightmapInfo",
#if UNITY_5_5
            "UnityEngine.MonoBehaviour.runInEditMode",
            "UnityEngine.Light.lightmappingMode",
#endif
        };
        // black list if white list not given
        public static void OnGetNoUseList(out List<string> list)
        {
            list = new List<string>
            {      
                "AI",
                "Analytics",
                "Audio",
                "Device",
                "Diagnostics",
                "Experimental",
                "Procedural",
                "Rendering",
                "Rigidbody",
                "VR",
                "Wheel",
                "Gradient",
                "HideInInspector",
                "ExecuteInEditMode",
                "AddComponentMenu",
                "ContextMenu",
                "RequireComponent",
                "DisallowMultipleComponent",
                "SerializeField",
                "AssemblyIsEditorAssembly",
                "Attribute", 
                "Types",
                "UnitySurrogateSelector",
                "TrackedReference",
                "TypeInferenceRules",
                "FFTWindow",
                "RPC",
                "Network",
                "MasterServer",
                "BitStream",
                "HostData",
                "ConnectionTesterStatus",
                "GUI",
                "EventType",
                "EventModifiers",
                "FontStyle",
                "TextAlignment",
                "TextEditor",
                "TextEditorDblClickSnapping",
                "TextGenerator",
                "TextClipping",
                "Gizmos",
                "ADBannerView",
                "ADInterstitialAd",            
                "Android",
                "Tizen",
                "jvalue",
                "iPhone",
                "iOS",
                "CalendarIdentifier",
                "CalendarUnit",
                "CalendarUnit",
                "ClusterInput",
                "FullScreenMovieControlMode",
                "FullScreenMovieScalingMode",
                "Handheld",
                "LocalNotification",
                "NotificationServices",
                "RemoteNotificationType",      
                "RemoteNotification",
                "SamsungTV",
                "TextureCompressionQuality",
                "TouchScreenKeyboardType",
                "TouchScreenKeyboard",
                "MovieTexture",
                "UnityEngineInternal",
                "Terrain",                            
                "Tree",
                "SplatPrototype",
                "DetailPrototype",
                "DetailRenderMode",
                "MeshSubsetCombineUtility",
                "AOT",
                "Social",
                "Enumerator",       
                "SendMouseEvents",               
                "Cursor",
                "Flash",
                "ActionScript",
                "OnRequestRebuild",
                "Ping",
                "ShaderVariantCollection",
                "SimpleJson.Reflection",
                "CoroutineTween",
                "GraphicRebuildTracker",
                "Advertisements",
                "UnityEditor",
			    "WSA",
			    "EventProvider",
			    "Apple",
			    "ClusterInput",
				"Motion",
#if UNITY_5_4
                "Windows",
#elif UNITY_5_5
                "ReflectionMethodsCache",
                "DictationRecognizer",
                "Speech",
                "RemoteSettings",
#endif
            };
        }
    }
}