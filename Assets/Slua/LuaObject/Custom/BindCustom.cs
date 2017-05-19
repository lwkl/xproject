using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_System_Collections_Generic_Dictionary_2_int_string.reg,
				Lua_System_String.reg,
				Lua_TweenAction.reg,
				Lua_LeanTweenType.reg,
				Lua_LTDescr.reg,
				Lua_LTUtility.reg,
				Lua_LeanTween.reg,
				Lua_LTBezier.reg,
				Lua_LTBezierPath.reg,
				Lua_LTSpline.reg,
				Lua_LTRect.reg,
				Lua_LTEvent.reg,
				Lua_LTGUI.reg,
				Lua_XConf.reg,
				Lua_XLoading.reg,
				Lua_XApp.reg,
				Lua_XPool.reg,
				Lua_XSinglePool.reg,
				Lua_XIntKeyPool.reg,
				Lua_XStrKeyPool.reg,
				Lua_XGOSinglePool.reg,
				Lua_XGOIntKeyPool.reg,
				Lua_XGOStrKeyPool.reg,
				Lua_XPoolManager.reg,
				Lua_XLoadDesc.reg,
				Lua_XLoad.reg,
			};
			return list;
		}
	}
}
