namespace SLua
{
    public class LuaTableReader
    {
        private LuaTable _table;
        public LuaTable luaTable
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
            }
        }

        public LuaTableReader() { }

        public LuaTableReader(LuaTable table)
        {
            _table = table;
        }

        public LuaValueCast this[string key]
        {
            get
            {
                return new LuaValueCast(_table[key]);
            }
        }

        public LuaValueCast this[int index]
        {
            get
            {
                return new LuaValueCast(_table[index]);
            }
        }

        public LuaValueCast get(int index, object defaultValue)
        {
            object value = _table[index] ?? defaultValue;
            return new LuaValueCast(value);
        }

        public LuaValueCast get(string key, object defaultValue)
        {
            object value = _table[key] ?? defaultValue;
            return new LuaValueCast(value);
        }
    }
}