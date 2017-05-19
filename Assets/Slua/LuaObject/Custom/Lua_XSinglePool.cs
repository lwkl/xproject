using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XSinglePool : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getObj(IntPtr l) {
		try {
			XSinglePool self=(XSinglePool)checkSelf(l);
			var ret=self.getObj();
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
			XSinglePool self=(XSinglePool)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			self.delObj(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int createObj(IntPtr l) {
		try {
			XSinglePool self=(XSinglePool)checkSelf(l);
			var ret=self.createObj();
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
			XSinglePool self=(XSinglePool)checkSelf(l);
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
			XSinglePool self=(XSinglePool)checkSelf(l);
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
			var ret=XSinglePool.createPool(a1);
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
	static public int get__pool(IntPtr l) {
		try {
			XSinglePool self=(XSinglePool)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._pool);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__pool(IntPtr l) {
		try {
			XSinglePool self=(XSinglePool)checkSelf(l);
			System.Collections.Generic.List<System.Object> v;
			checkType(l,2,out v);
			self._pool=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XSinglePool");
		addMember(l,getObj);
		addMember(l,delObj);
		addMember(l,createObj);
		addMember(l,clear);
		addMember(l,min);
		addMember(l,createPool_s);
		addMember(l,"_pool",get__pool,set__pool,true);
		createTypeMetatable(l,null, typeof(XSinglePool),typeof(XPool));
	}
}
