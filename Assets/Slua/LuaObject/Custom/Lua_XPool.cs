using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XPool : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int clear(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
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
			XPool self=(XPool)checkSelf(l);
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
	static public int isWrong(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.isWrong(a1);
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
	static public int createPool_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=XPool.createPool(a1);
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
	static public int get_PoolType(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.PoolType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PoolType(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			XPoolType v;
			checkEnum(l,2,out v);
			self.PoolType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__maxSize(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._maxSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__maxSize(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self._maxSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__minSize(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._minSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__minSize(IntPtr l) {
		try {
			XPool self=(XPool)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self._minSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XPool");
		addMember(l,clear);
		addMember(l,min);
		addMember(l,isWrong);
		addMember(l,createPool_s);
		addMember(l,"PoolType",get_PoolType,set_PoolType,true);
		addMember(l,"_maxSize",get__maxSize,set__maxSize,true);
		addMember(l,"_minSize",get__minSize,set__minSize,true);
		createTypeMetatable(l,null, typeof(XPool),typeof(UnityEngine.MonoBehaviour));
	}
}
