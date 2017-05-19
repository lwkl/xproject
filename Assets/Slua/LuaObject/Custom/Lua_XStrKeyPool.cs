using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XStrKeyPool : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getObj(IntPtr l) {
		try {
			XStrKeyPool self=(XStrKeyPool)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getObj(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int delObj(IntPtr l) {
		try {
			XStrKeyPool self=(XStrKeyPool)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.delObj(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int clear(IntPtr l) {
		try {
			XStrKeyPool self=(XStrKeyPool)checkSelf(l);
			self.clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int min(IntPtr l) {
		try {
			XStrKeyPool self=(XStrKeyPool)checkSelf(l);
			self.min();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int createPool_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=XStrKeyPool.createPool(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XStrKeyPool");
		addMember(l,getObj);
		addMember(l,delObj);
		addMember(l,clear);
		addMember(l,min);
		addMember(l,createPool_s);
		createTypeMetatable(l,null, typeof(XStrKeyPool),typeof(XPool));
	}
}
