using System;
#if !SLUA_STANDALONE
using UnityEngine;
#endif

namespace SLua
{
    public class LuaValueCast
    {
        public object value;

        public LuaValueCast(object value)
        {
            if (value is LuaValueCast)
            {
                this.value = ((LuaValueCast)value).value;
            }
            else
            {
                this.value = value;
            }
        }

        public static implicit operator int(LuaValueCast node) { return Convert.ToInt32(node.value); }
        public static implicit operator string(LuaValueCast node) { return Convert.ToString(node.value); }
        public static implicit operator float(LuaValueCast node) { return Convert.ToSingle(node.value); }
        public static implicit operator double(LuaValueCast node) { return Convert.ToDouble(node.value); }
        public static implicit operator bool(LuaValueCast node) { return Convert.ToBoolean(node.value); }
        public static implicit operator LuaTable(LuaValueCast node) { return node.value as LuaTable; }
#if !SLUA_STANDALONE
        public static implicit operator Texture(LuaValueCast node)
        {
            if (node.value is Texture)
                return (Texture)node.value;
            return null;
        }

        public static implicit operator Vector2(LuaValueCast node)
        {
            if (node.value is Vector2)
            {
                return (Vector2)node.value;
            }

            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(Vector2);
            }
            float x = Convert.ToSingle(t["x"] ?? t[1]);
            float y = Convert.ToSingle(t["y"] ?? t[2]);
            return new Vector2(x, y);
        }
        public static implicit operator Vector3(LuaValueCast node)
        {
            if (node.value is Vector3)
            {
                return (Vector3)node.value;
            }

            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(Vector3);
            }
            float x = Convert.ToSingle(t["x"] ?? t[1]);
            float y = Convert.ToSingle(t["y"] ?? t[2]);
            float z = Convert.ToSingle(t["z"] ?? t[3]);
            return new Vector3(x, y, z);
        }
        public static implicit operator Vector4(LuaValueCast node)
        {
            if (node.value is Vector4)
            {
                return (Vector4)node.value;
            }

            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(Vector4);
            }
            float x = Convert.ToSingle(t["x"] ?? t[1]);
            float y = Convert.ToSingle(t["y"] ?? t[2]);
            float z = Convert.ToSingle(t["z"] ?? t[3]);
            float w = Convert.ToSingle(t["w"] ?? t[4]);
            return new Vector4(x, y, z, w);
        }
        public static implicit operator Quaternion(LuaValueCast node)
        {
            if (node.value is Quaternion)
            {
                return (Quaternion)node.value;
            }

            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(Quaternion);
            }
            float x = Convert.ToSingle(t["x"] ?? t[1]);
            float y = Convert.ToSingle(t["y"] ?? t[2]);
            float z = Convert.ToSingle(t["z"] ?? t[3]);
            float w = Convert.ToSingle(t["w"] ?? t[4]);
            return new Quaternion(x, y, z, w);
        }
        public static implicit operator Color(LuaValueCast node)
        {
            if (node.value is Color)
            {
                return (Color)node.value;
            }

            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(Color);
            }
            float r = Convert.ToSingle(t["r"] ?? t[1]);
            float g = Convert.ToSingle(t["g"] ?? t[2]);
            float b = Convert.ToSingle(t["b"] ?? t[3]);
            float a = Convert.ToSingle(t["a"] ?? t[4]);
            return new Color(r, g, b, a);
        }
        public static implicit operator Keyframe(LuaValueCast node)
        {
            if (node.value is Keyframe)
            {
                return (Keyframe)node.value;
            }
            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(Keyframe);
            }
            float time = Convert.ToSingle(t["time"] ?? t[1]);
            float v = Convert.ToSingle(t["value"] ?? t[2]);
            float i = Convert.ToSingle(t["inTangent"] ?? t[3]);
            float o = Convert.ToSingle(t["outTangent"] ?? t[4]);
            return new Keyframe(time, v, i, o);
        }
        public static implicit operator AnimationCurve(LuaValueCast node)
        {
            if (node.value is AnimationCurve)
            {
                return (AnimationCurve)node.value;
            }
            LuaTable t = node.value as LuaTable;
            if (t == null)
            {
                return default(AnimationCurve);
            }

            LuaTableReader tr = new LuaTableReader(t);
            Keyframe[] keys = new Keyframe[t.length()];
            for (int i = 0, len = t.length(); i < len; i++)
            {
                Keyframe keyframe = tr[i + 1];
                keys.SetValue(keyframe, i);
            }
            return new AnimationCurve(keys);
        }
        public static implicit operator CameraClearFlags(LuaValueCast v)
        {
            return (CameraClearFlags)Enum.Parse(typeof(CameraClearFlags), v);
        }
#endif

        public LuaValueCast this[string key]
        {
            get
            {
                LuaTable table = (LuaTable)this.value;
                if (table == null)
                {
                    throw new NullReferenceException();
                }
                LuaTableReader reader = new LuaTableReader(table);
                return reader[key];
            }
        }

        public LuaValueCast this[int index]
        {
            get
            {
                LuaTable table = (LuaTable)this.value;
                if (table == null)
                {
                    throw new NullReferenceException();
                }
                LuaTableReader reader = new LuaTableReader(table);
                return reader[index];
            }
        }
    }
}
