using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XPoolManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getPool(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			XPoolType a2;
			checkEnum(l,3,out a2);
			var ret=self.getPool(a1,a2);
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
	static public int getSingle(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getSingle(a1);
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
	static public int getIntKey(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getIntKey(a1);
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
	static public int getStrKey(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getStrKey(a1);
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
	static public int getGOSingle(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getGOSingle(a1);
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
	static public int getGOIntKey(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getGOIntKey(a1);
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
	static public int getGOStrKey(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.getGOStrKey(a1);
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
	static public int clear(IntPtr l) {
		try {
			XPoolManager self=(XPoolManager)checkSelf(l);
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
			XPoolManager self=(XPoolManager)checkSelf(l);
			self.min();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XPoolManager");
		addMember(l,getPool);
		addMember(l,getSingle);
		addMember(l,getIntKey);
		addMember(l,getStrKey);
		addMember(l,getGOSingle);
		addMember(l,getGOIntKey);
		addMember(l,getGOStrKey);
		addMember(l,clear);
		addMember(l,min);
		createTypeMetatable(l,null, typeof(XPoolManager),typeof(UnityEngine.MonoBehaviour));
	}
}
