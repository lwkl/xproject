using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

namespace SLua
{
    public class LuaConsole : EditorWindow
    {
        [MenuItem("SLua/LuaConsole")]
        public static void openLuaConsole()
        {
            EditorWindow.GetWindow<LuaConsole>();
        }

        string inputText_ = "";
        string filterPattern = "";

        struct OutputRecord
        {
            public string text;
            public enum OutputType
            {
                Log = 0,
                Err = 1,
            }
            public OutputType type;
            public OutputRecord(string text, OutputType type)
            {
                this.type = type;
                if (type == OutputType.Err)
                {
                    this.text = "<color=#a52a2aff>" + text + "</color>";
                }
                else
                {
                    this.text = text;
                }
            }
        }

        string outputText_ = "LuaConsole:\n";
        StringBuilder outputBuffer = new StringBuilder();
        List<OutputRecord> recordList = new List<OutputRecord>();

        List<string> history_ = new List<string>();
        int historyIndex_ = 0;

        Vector2 scrollPosition_ = Vector2.zero;
        GUIStyle textAreaStyle = new GUIStyle();
        bool initedStyle = false;
        bool toggleLog = true;
        bool toggleErr = true;

        void addLog(string str)
        {
            recordList.Add(new OutputRecord(str, OutputRecord.OutputType.Log));
            consoleFlush();
        }

        void addError(string str)
        {
            recordList.Add(new OutputRecord(str, OutputRecord.OutputType.Err));
            consoleFlush();
        }

        void consoleFlush()
        {
            outputBuffer.Length = 0;

            Regex filter = null;
            string pat = filterPattern.Trim();
            if (!string.IsNullOrEmpty(pat) && pat != "*")
            {
                try
                {
                    filter = new Regex(pat);
                }
                catch { }
            }

            for (int i = 0; i < recordList.Count; ++i)
            {
                OutputRecord record = recordList[i];
                if (record.type == OutputRecord.OutputType.Log && !toggleLog)
                    continue;
                else if (record.type == OutputRecord.OutputType.Err && !toggleErr)
                    continue;

                if (filter != null && !filter.IsMatch(record.text))
                {
                    continue;
                }

                outputBuffer.AppendLine(record.text);
            }

            outputText_ = outputBuffer.ToString();
            scrollPosition_.y = float.MaxValue;
            Repaint();
        }

        void OnEnable()
        {
            LuaState.logDelegate += addLog;
            LuaState.errorDelegate += addError;
        }

        void OnDisable()
        {
            LuaState.logDelegate -= addLog;
            LuaState.errorDelegate -= addError;
        }

        void OnDestroy()
        {
            LuaState.logDelegate -= addLog;
            LuaState.errorDelegate -= addLog;
        }

        void OnGUI()
        {
            if (!initedStyle)
            {
                GUIStyle entryInfoTyle = "CN EntryInfo";
                textAreaStyle.richText = true;
                textAreaStyle.normal.textColor = entryInfoTyle.normal.textColor;
                initedStyle = true;
            }

            //输出
            scrollPosition_ = GUILayout.BeginScrollView(scrollPosition_, GUILayout.Width(Screen.width), GUILayout.ExpandHeight(true));
            EditorGUILayout.TextArea(outputText_, textAreaStyle, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();

            //选项
            GUILayout.BeginHorizontal();
            bool oldToggleLog = toggleLog;
            bool oldToggleErr = toggleErr;
            toggleLog = GUILayout.Toggle(oldToggleLog, "log", GUILayout.ExpandWidth(false));
            toggleErr = GUILayout.Toggle(oldToggleErr, "error", GUILayout.ExpandWidth(false));

            //过滤
            GUILayout.Space(10f);
            GUILayout.Label("filter:", GUILayout.ExpandWidth(false));
            string oldFilterPattern = filterPattern;
            filterPattern = GUILayout.TextField(oldFilterPattern, GUILayout.Width(200f));
            GUILayout.EndHorizontal();

            if (toggleLog != oldToggleLog || toggleErr != oldToggleErr || filterPattern != oldFilterPattern)
            {
                consoleFlush();
            }

            //输入
            GUI.SetNextControlName("Input");
            inputText_ = EditorGUILayout.TextField(inputText_, GUILayout.Height(50));

            //按钮
            if (GUILayout.Button("clear", GUILayout.ExpandWidth(true)))
            {
                recordList.Clear();
                consoleFlush();
            }

            if (Event.current.isKey && Event.current.type == EventType.KeyUp)
            {
                bool refresh = false;
                if (Event.current.keyCode == KeyCode.Return)
                {
                    if (inputText_ != "")
                    {
                        if (history_.Count == 0 || history_[history_.Count - 1] != inputText_)
                        {
                            history_.Add(inputText_);
                        }
                        addLog(inputText_);
                        doCommand(inputText_);
                        inputText_ = "";
                        refresh = true;
                        historyIndex_ = history_.Count;
                    }
                }
                else if (Event.current.keyCode == KeyCode.UpArrow)
                {
                    if (history_.Count > 0)
                    {
                        historyIndex_ = historyIndex_ - 1;
                        if (historyIndex_ < 0)
                        {
                            historyIndex_ = 0;
                        }
                        else
                        {
                            inputText_ = history_[historyIndex_];
                            refresh = true;
                        }
                    }
                }
                else if (Event.current.keyCode == KeyCode.DownArrow)
                {
                    if (history_.Count > 0)
                    {
                        historyIndex_ = historyIndex_ + 1;
                        if (historyIndex_ > history_.Count - 1)
                        {
                            historyIndex_ = history_.Count - 1;
                        }
                        else
                        {
                            inputText_ = history_[historyIndex_];
                            refresh = true;
                        }
                    }
                }

                if (refresh)
                {
                    Repaint();
                    EditorGUIUtility.editingTextField = false;
                    GUI.FocusControl("Input");
                }
            }
        }

        void doCommand(string str)
        {
            LuaState luaState = LuaState.main;
            if (luaState == null)
                return;

            if (string.IsNullOrEmpty(str))
                return;

            int index = str.IndexOf(" ");
            string cmd = str;
            string tail = "";
            if (index > 0)
            {
                cmd = str.Substring(0, index).Trim().ToLower();
                tail = str.Substring(index + 1);
            }
            if (cmd == "p")
            {
                if (tail == "")
                {
                    return;
                }

                var luaFunc = luaState.getFunction("Slua.ldb.printExpr");
                if (luaFunc != null)
                {
                    luaFunc.call(tail);
                }
            }
            else if (cmd == "dir")
            {
                if (tail == "")
                {
                    return;
                }

                var luaFunc = luaState.getFunction("Slua.ldb.dirExpr");
                if (luaFunc != null)
                {
                    luaFunc.call(tail);
                }
            }
            else
            {
                var luaFunc = luaState.getFunction("Slua.ldb.doExpr");
                if (luaFunc != null)
                {
                    luaFunc.call(str);
                }
            }
        }


        [MenuItem("CONTEXT/Component/Push Component To Lua")]
        static void PushComponentObjectToLua(MenuCommand cmd)
        {
            Component tf = cmd.context as Component;
            if (tf == null)
                return;

            LuaState luaState = LuaState.main;
            if (luaState == null)
                return;

            LuaObject.pushObject(luaState.L, tf);
            LuaDLL.lua_setglobal(luaState.L, "_");
        }

        [MenuItem("CONTEXT/Component/Push GameObject To Lua")]
        static void PushGameObjectToLua(MenuCommand cmd)
        {
            Transform tf = cmd.context as Transform;
            if (tf == null)
                return;

            LuaState luaState = LuaState.main;
            if (luaState == null)
                return;

            SLua.LuaObject.pushObject(luaState.L, tf.gameObject);
            LuaDLL.lua_setglobal(luaState.L, "_");
        }
    }
}