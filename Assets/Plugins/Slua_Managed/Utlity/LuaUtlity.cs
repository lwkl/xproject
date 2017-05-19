using System;
using System.Collections;
using System.Collections.Generic;
#if !SLUA_STANDALONE
using UnityEngine;
#endif

namespace SLua
{
    public static class LuaUtlity
    {
        public delegate object CustomReadDelegate(ref object o, LuaTable table);
        public delegate void CustomWriteDelegate(object o, LuaTable table);
        static Dictionary<Type, CustomReadDelegate> customReadDelegateMap = new Dictionary<Type, CustomReadDelegate>();
        static Dictionary<Type, CustomWriteDelegate> customWriteDelegateMap = new Dictionary<Type, CustomWriteDelegate>();

        public static void AddCustomType(Type t, CustomReadDelegate reader, CustomWriteDelegate writer)
        {
            if (reader != null)
                customReadDelegateMap[t] = reader;
            if (writer != null)
                customWriteDelegateMap[t] = writer;
        }

        static LuaUtlity()
        {
#if !SLUA_STANDALONE
            AddCustomType(typeof(BoneWeight),
            (ref object o, LuaTable table) =>
            {
                BoneWeight target = (BoneWeight)o;
                int boneIndex0 = Convert.ToInt32(table["boneIndex0"]);
                int boneIndex1 = Convert.ToInt32(table["boneIndex1"]);
                int boneIndex2 = Convert.ToInt32(table["boneIndex2"]);
                int boneIndex3 = Convert.ToInt32(table["boneIndex3"]);
                float weight0 = Convert.ToSingle(table["weight0"]);
                float weight1 = Convert.ToSingle(table["weight1"]);
                float weight2 = Convert.ToSingle(table["weight2"]);
                float weight3 = Convert.ToSingle(table["weight3"]);

                target.boneIndex0 = boneIndex0;
                target.boneIndex1 = boneIndex1;
                target.boneIndex2 = boneIndex2;
                target.boneIndex3 = boneIndex3;
                target.weight0 = weight0;
                target.weight1 = weight1;
                target.weight2 = weight2;
                target.weight3 = weight3;

                return target;
            },
            (object o, LuaTable table) =>
            {
                BoneWeight target = (BoneWeight)o;
                table["boneIndex0"] = target.boneIndex0;
                table["boneIndex1"] = target.boneIndex1;
                table["boneIndex2"] = target.boneIndex2;
                table["boneIndex3"] = target.boneIndex3;
                table["weight0"] = target.weight0;
                table["weight1"] = target.weight1;
                table["weight2"] = target.weight2;
                table["weight3"] = target.weight3;
            });

            AddCustomType(typeof(Rect),
            (ref object o, LuaTable table) =>
            {
                Rect target = (Rect)o;
                float x = Convert.ToSingle(table["x"]);
                float y = Convert.ToSingle(table["y"]);
                float width = Convert.ToSingle(table["width"]);
                float height = Convert.ToSingle(table["height"]);

                target.x = x;
                target.y = y;
                target.width = width;
                target.height = height;

                return target;
            },
            (object o, LuaTable table) =>
            {
                Rect target = (Rect)o;
                table["x"] = target.x;
                table["y"] = target.y;
                table["width"] = target.width;
                table["height"] = target.height;
            });

            AddCustomType(typeof(AnimationCurve),
            (ref object o, LuaTable table) =>
            {
                AnimationCurve target = (AnimationCurve)o;
                var keys = table["keys"] as LuaTable;
                target.keys = FromLua(keys, typeof(Keyframe[])) as Keyframe[];
                return target;
            },
            (object o, LuaTable table) =>
            {
                AnimationCurve target = (AnimationCurve)o;
                LuaState ls = LuaState.get(table.L);
                table["keys"] = ToLua(ls, target.keys);
            });

            AddCustomType(typeof(Keyframe),
            (ref object o, LuaTable table) =>
            {
                Keyframe target = (Keyframe)o;
                target.inTangent = Convert.ToSingle(table["inTangent"]);
                target.outTangent = Convert.ToSingle(table["outTangent"]);
                target.tangentMode = Convert.ToInt32(table["tangentMode"]);
                target.time = Convert.ToSingle(table["time"]);
                target.value = Convert.ToSingle(table["value"]);
                return target;
            },
            (object o, LuaTable table) =>
            {
                Keyframe target = (Keyframe)o;
                table["inTangent"] = target.inTangent;
                table["outTangent"] = target.outTangent;
                table["tangentMode"] = target.tangentMode;
                table["time"] = target.time;
                table["value"] = target.value;
            });
#endif
        }

        static HashSet<Type> baseTypes = new HashSet<Type>()
        {
            { typeof(float)},
            { typeof(int)},
            { typeof(uint) },
            { typeof(ulong)},
            { typeof(long)},
            { typeof(short) },
            { typeof(ushort) },
            { typeof(double)},
            { typeof(byte)},
            { typeof(char)},
            { typeof(bool) },
            { typeof(string)},
            { typeof(Enum)},
        };

        //static bool IsDefaultValue(object o)
        //{
        //    Type t = o.GetType();
        //    if (!t.IsValueType)
        //        return (o == null);
        //    else
        //        return o.Equals(Activator.CreateInstance(t));
        //}

        public static LuaTable ToLua(LuaState ls, object o)
        {
            Type t = o.GetType();
            if (t.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                return null;

            LuaTable result = null;
            if (o is IList)
            {
                Type listImplementation = t.GetInterface(typeof(IList<>).Name);
                if (listImplementation != null)
                {
                    IList lst = o as IList;
                    Type elemT = listImplementation.GetGenericArguments()[0];
                    if (!IsNoSerializeType(elemT))
                    {
                        result = result ?? new LuaTable(ls);
                        for (int i = 0; i < lst.Count; ++i)
                        {
                            object elem = lst[i];
                            if (elem == null)
                                continue;

                            if (IsBaseType(elemT))
                            {
                                elem = ChangeType(elem, elemT);
                                result[i + 1] = elem;
                            }
                            else
                            {
                                result[i + 1] = ToLua(ls, elem);
                            }
                        }
                    }
                }
            }

            if (o is IDictionary)
            {
                Type dictImplementation = t.GetInterface(typeof(IDictionary<,>).Name);
                if (dictImplementation != null)
                {
                    Type keyT = dictImplementation.GetGenericArguments()[0];
                    Type valueT = dictImplementation.GetGenericArguments()[1];

                    if (IsBaseType(keyT) && !IsNoSerializeType(valueT))
                    {
                        result = result ?? new LuaTable(ls);
                        IDictionary dict = o as IDictionary;
                        var iter = dict.GetEnumerator();
                        while (iter.MoveNext())
                        {
                            if (iter.Value == null)
                                continue;

                            if (iter.Key is string)
                            {
                                string stringKey = iter.Key as string;
                                if (IsBaseType(valueT))
                                {
                                    object value = ChangeType(iter.Value, valueT);
                                    result[stringKey] = value;
                                }
                                else
                                {
                                    result[stringKey] = ToLua(ls, iter.Value);
                                }
                            }
                            else
                            {
                                int intKey = Convert.ToInt32(iter.Key);
                                if (IsBaseType(valueT))
                                {
                                    object value = ChangeType(iter.Value, valueT);
                                    result[intKey] = value;
                                }
                                else
                                {
                                    result[intKey] = ToLua(ls, iter.Value);
                                }
                            }
                        }
                    }
                }
            }

            if (!IsNoSerializeType(t))
            {
                result = result ?? new LuaTable(ls);

                Type curT = t;
                while (curT != typeof(System.Object))
                {
                    var fis = curT.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                    for (int i = 0; i < fis.Length; ++i)
                    {
                        var fi = fis[i];
                        if (fi.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                            continue;

                        Type elemT = fi.FieldType;
                        object elem = fi.GetValue(o);
                        if (elem == null)
                            continue;

                        if (IsBaseType(elemT))
                        {
                            elem = ChangeType(elem, elemT);
                            result[fi.Name] = elem;
                        }
                        else
                        {
                            result[fi.Name] = ToLua(ls, elem);
                        }
                    }

                    CustomWriteDelegate writer = null;
                    if (customWriteDelegateMap.TryGetValue(curT, out writer))
                    {
                        writer(o, result);
                    }

                    curT = curT.BaseType;
                }
            }

            return result;
        }

        public static T FromLua<T>(LuaTable table)
        {
            return (T)FromLua(table, typeof(T));
        }

        public static object FromLua(LuaTable table, Type t)
        {
            object result = null;

            if (t.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                return null;

            if (typeof(IList).IsAssignableFrom(t))
            {
                if (t.IsArray)
                {
                    Type elemT = t.GetElementType();
                    if (!IsNoSerializeType(elemT))
                    {
                        int elemCount = table.length();
                        Array arr = Array.CreateInstance(elemT, elemCount);
                        result = arr;
                        for (int i = 1; i <= elemCount; ++i)
                        {
                            object elem = table[i];
                            if (IsBaseType(elemT))
                            {
                                elem = ChangeType(elem, elemT);
                                arr.SetValue(elem, i - 1);
                            }
                            else
                            {
                                arr.SetValue(FromLua(elem as LuaTable, elemT), i - 1);
                            }
                        }
                    }
                }
                else
                {
                    Type listImplementation = t.GetInterface(typeof(IList<>).Name);
                    if (listImplementation != null)
                    {
                        Type elemT = listImplementation.GetGenericArguments()[0];
                        if (!IsNoSerializeType(elemT))
                        {
                            result = result ?? Activator.CreateInstance(t);
                            IList lst = result as IList;
                            int elemCount = table.length();
                            for (int i = 1; i <= elemCount; ++i)
                            {
                                object elem = table[i];
                                if (IsBaseType(elemT))
                                {
                                    elem = ChangeType(elem, elemT);
                                    lst.Add(elem);
                                }
                                else
                                {
                                    lst.Add(FromLua(elem as LuaTable, elemT));
                                }
                            }
                        }
                    }
                }
            }

            if (typeof(IDictionary).IsAssignableFrom(t))
            {
                Type dictImplementation = t.GetInterface(typeof(IDictionary<,>).Name);
                if (dictImplementation != null)
                {
                    Type keyT = dictImplementation.GetGenericArguments()[0];
                    Type valueT = dictImplementation.GetGenericArguments()[1];

                    if (IsBaseType(keyT) && !IsNoSerializeType(valueT))
                    {
                        result = result ?? Activator.CreateInstance(t);
                        IDictionary dict = result as IDictionary;
                        foreach (var pair in table)
                        {
                            if (pair.value == null)
                                continue;
                            object key = ChangeType(pair.key, keyT);
                            if (IsBaseType(valueT))
                            {
                                object value = ChangeType(pair.value, valueT);
                                dict.Add(key, value);
                            }
                            else
                            {
                                dict.Add(key, FromLua(pair.value as LuaTable, valueT));
                            }
                        }
                    }
                }
            }

            if (!IsNoSerializeType(t))
            {
                result = result ?? Activator.CreateInstance(t);

                Type curT = t;
                while (curT != typeof(System.Object))
                {
                    var fis = curT.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                    for (int i = 0; i < fis.Length; ++i)
                    {
                        var fi = fis[i];
                        if (fi.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                            continue;

                        Type elemT = fi.FieldType;
                        object elem = table[fi.Name];
                        if (elem == null)
                            continue;
                        if (IsBaseType(elemT))
                        {
                            elem = ChangeType(elem, elemT);
                            fi.SetValue(result, elem);
                        }
                        else
                        {
                            fi.SetValue(result, FromLua(elem as LuaTable, elemT));
                        }
                    }

                    CustomReadDelegate reader = null;
                    if (customReadDelegateMap.TryGetValue(curT, out reader))
                    {
                        result = reader(ref result, table);
                    }

                    curT = curT.BaseType;
                }
            }

            return result;
        }

        static bool IsNoSerializeType(Type t)
        {
            if (IsBaseType(t))
                return false;

            if (t.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                return true;

            if (customReadDelegateMap.ContainsKey(t) || customWriteDelegateMap.ContainsKey(t))
                return false;

            if (t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Length == 0)
                return true;

            if (t.IsValueType)
                return false;

            if (t.GetConstructors().Length == 0)
                return true;

            if (t.GetConstructor(Type.EmptyTypes) == null)
                return true;

            return false;
        }

        public static bool IsBaseType(Type t)
        {
            if (baseTypes.Contains(t))
            {
                return true;
            }
            else
            {
                var iter = baseTypes.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (iter.Current.IsAssignableFrom(t))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static object ChangeType(object o, Type t)
        {
            Type fromT = o.GetType();
            Type toT = t;

            if (typeof(Enum).IsAssignableFrom(t))
            {
                int i = Convert.ToInt32(o);
                string name = Enum.GetName(t, i);
                return Enum.Parse(t, name);
            }

            if (toT.IsAssignableFrom(fromT))
            {
                return o;
            }
            else
            {
                return Convert.ChangeType(o, toT);
            }
        }

        //class Test : List<int>
        //{
        //    public enum E { a, b, c }
        //    public E e = E.c;

        //    public Vector3 v = Vector3.one;
        //    public Vector3[] arr = new Vector3[] { Vector3.zero, Vector3.one };
        //    public List<Vector3> list = new List<Vector3>() { Vector3.one, Vector3.one };
        //    public Dictionary<int, Vector3> intDict = new Dictionary<int, Vector3>() { { 2, Vector3.one }, { 5, Vector3.one } };
        //    public Dictionary<string, Vector3> keyDict = new Dictionary<string, Vector3>() { { "2", Vector3.one }, { "5", Vector3.one } };
        //    public float f = 123f;
        //    public float d = 234;
        //    public int i = 1263;
        //    public bool b = true;
        //    public string s = "asdasd";
        //    //public Test next;
        //    public BoneWeight boneWeight;
        //    public Rect rect;
        //    public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        //}

        //[MenuItem("SLua/test")]
        //static void test()
        //{
        //    using (LuaState ls = new LuaState())
        //    {
        //        Test source = new Test();
        //        source.Add(23);
        //        source.Add(26);

        //        LuaTable table = ToLua(ls, source);
        //        SLua.Logger.Log(table.ToLua());
        //        object dest = FromLua(table, typeof(Test));
        //        SLua.Logger.Log(dest);
        //        SLua.Logger.Log(ToLua(ls, dest).ToLua());
        //    }
        //}
    }
}
