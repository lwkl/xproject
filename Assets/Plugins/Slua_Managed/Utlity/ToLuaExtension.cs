using System;
using System.Collections.Generic;
using System.Text;

namespace SLua
{
    public static class ToLuaExtension
    {
        static HashSet<string> s_keywords = new HashSet<string>(){
        "and", "break", "do", "else", "elseif", "end", "false", "for",
        "function", "if", "in", "local", "nil", "not", "or", "repeat",
        "return", "then", "true", "until", "while"};

        static bool IsKeyword(string s)
        {
            return s_keywords.Contains(s);
        }

        static bool IsToken(string s)
        {
            if (IsKeyword(s))
                return false;

            if (!char.IsLetter(s[0]) && s[0] != '_')
            {
                return false;
            }

            for (int i = 1; i < s.Length; ++i)
            {
                char c = s[i];
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    return false;
                }
            }

            return true;
        }

        static void AddQuoted(StringBuilder sstr, string s)
        {
            int i = 0;
            sstr.Append('"');
            while (i < s.Length)
            {
                char c = s[i];
                switch (c)
                {
                    case '"':
                    case '\\':
                    case '\n':
                        {
                            sstr.Append('\\');
                            sstr.Append(c);
                            break;
                        }
                    case '\r':
                        {
                            sstr.Append("\\r");
                            break;
                        }
                    case '\0':
                        {
                            sstr.Append("\\000");
                            break;
                        }
                    default:
                        {
                            sstr.Append(c);
                            break;
                        }
                }
                i++;
            }
            sstr.Append('"');
        }

        static bool IsNumberType(object o)
        {
            if (o is int || o is float || o is double
                || o is sbyte || o is byte || o is short
                || o is ushort || o is uint || o is long
                || o is ulong)
            {
                return true;
            }
            return false;
        }

        static void TabToStr(LuaTable table, ref StringBuilder stream, int indent)
        {
            stream.Append("{\n");

            bool arrayPart = true;
            int lastArrayIndex = 0;

            foreach (var pair in table)
            {
                stream.Append('\t', indent + 1);
                if (IsNumberType(pair.key))
                {
                    int index = Convert.ToInt32(pair.key);
                    if (arrayPart && (index - lastArrayIndex) == 1)
                    {
                        lastArrayIndex = index;
                    }
                    else
                    {
                        arrayPart = false;
                        stream.AppendFormat("[{0}]=", pair.key);
                    }
                }
                else if (pair.key is string)
                {
                    arrayPart = false;

                    string key = (string)pair.key;
                    if (string.IsNullOrEmpty(key))
                    {
                        throw new Exception("Can't use empty string as key");
                    }
                    if (IsToken(key))
                    {
                        stream.Append(key);
                        stream.Append("=");
                    }
                    else
                    {
                        stream.Append("[");
                        AddQuoted(stream, key);
                        stream.Append("]=");
                    }
                }
                else
                {
                    throw new Exception("don't support key type:" + pair.key.GetType().Name);
                }

                if (pair.value is LuaTable)
                {
                    LuaTable subTable = pair.value as LuaTable;
                    TabToStr(subTable, ref stream, indent + 1);
                }
                else
                {
                    if (LuaUtlity.IsBaseType(pair.value.GetType()))
                    {
                        if (pair.value is string)
                        {
                            string stringValue = pair.value as string;
                            AddQuoted(stream, stringValue);
                        }
                        else if (pair.value is bool)
                        {
                            bool b = (bool)pair.value;
                            stream.Append(b ? "true" : "false");
                        }
                        else
                        {
                            stream.Append(pair.value);
                        }
                    }
                    else
                    {
                        LuaState ls = LuaState.get(table.L);
                        LuaTable value = LuaUtlity.ToLua(ls, pair.value);
                        stream.Append(value.ToLua(false));
                    }
                }

                stream.Append(",");
                stream.Append("\n");
            }

            stream.Append('\t', indent);
            stream.Append("}");
        }

        public static string ToLua(this LuaTable self, bool asRoot)
        {
            StringBuilder sstr = new StringBuilder();
            TabToStr(self, ref sstr, 0);
            if (asRoot)
            {
                return "return " + sstr.ToString();
            }
            else
            {
                return sstr.ToString();
            }
        }

        public static string ToLua(this LuaTable self)
        {
            return self.ToLua(true);
        }
    }
}
