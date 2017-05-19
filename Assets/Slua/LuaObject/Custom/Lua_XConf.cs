using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XConf : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			XConf o;
			o=new XConf();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_forceLoadAB(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XConf.forceLoadAB);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_forceLoadAB(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			XConf.forceLoadAB=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_publishUrl(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XConf.publishUrl);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_publishUrl(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			XConf.publishUrl=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isUpdateVersion(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XConf.isUpdateVersion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_isUpdateVersion(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			XConf.isUpdateVersion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_luastart(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XConf.luastart);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_luastart(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			XConf.luastart=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_remoteLogUrl(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XConf.remoteLogUrl);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_remoteLogUrl(IntPtr l) {
		try {
			System.String v;
			checkType(l,2,out v);
			XConf.remoteLogUrl=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isRemoteLog(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XConf.isRemoteLog);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_isRemoteLog(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			XConf.isRemoteLog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XConf");
		addMember(l,"forceLoadAB",get_forceLoadAB,set_forceLoadAB,false);
		addMember(l,"publishUrl",get_publishUrl,set_publishUrl,false);
		addMember(l,"isUpdateVersion",get_isUpdateVersion,set_isUpdateVersion,false);
		addMember(l,"luastart",get_luastart,set_luastart,false);
		addMember(l,"remoteLogUrl",get_remoteLogUrl,set_remoteLogUrl,false);
		addMember(l,"isRemoteLog",get_isRemoteLog,set_isRemoteLog,false);
		createTypeMetatable(l,constructor, typeof(XConf));
	}
}
