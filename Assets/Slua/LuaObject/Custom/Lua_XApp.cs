using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XApp : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int closeldb(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			self.closeldb();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int loadLua(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			System.Action<XLoadDesc> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.loadLua(a1);
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
	static public int get_instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,XApp.instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_instance(IntPtr l) {
		try {
			XApp v;
			checkType(l,2,out v);
			XApp.instance=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_uiRoot(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.uiRoot);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_uiRoot(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.uiRoot=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_uiProgress(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.uiProgress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_uiProgress(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.uiProgress=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_uiMsgBox(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.uiMsgBox);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_uiMsgBox(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.uiMsgBox=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_uiWindow(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.uiWindow);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_uiWindow(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.uiWindow=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_uiBack(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.uiBack);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_uiBack(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			UnityEngine.Transform v;
			checkType(l,2,out v);
			self.uiBack=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_xloading(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.xloading);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_xloading(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			XLoading v;
			checkType(l,2,out v);
			self.xloading=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_xpool(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.xpool);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_xpool(IntPtr l) {
		try {
			XApp self=(XApp)checkSelf(l);
			XPoolManager v;
			checkType(l,2,out v);
			self.xpool=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XApp");
		addMember(l,closeldb);
		addMember(l,loadLua);
		addMember(l,"instance",get_instance,set_instance,false);
		addMember(l,"uiRoot",get_uiRoot,set_uiRoot,true);
		addMember(l,"uiProgress",get_uiProgress,set_uiProgress,true);
		addMember(l,"uiMsgBox",get_uiMsgBox,set_uiMsgBox,true);
		addMember(l,"uiWindow",get_uiWindow,set_uiWindow,true);
		addMember(l,"uiBack",get_uiBack,set_uiBack,true);
		addMember(l,"xloading",get_xloading,set_xloading,true);
		addMember(l,"xpool",get_xpool,set_xpool,true);
		createTypeMetatable(l,null, typeof(XApp),typeof(UnityEngine.MonoBehaviour));
	}
}
