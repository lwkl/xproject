using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XLoadDesc : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			XLoadDesc o;
			o=new XLoadDesc();
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
	static public int get_cursor(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cursor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_cursor(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			XLoadRes v;
			checkType(l,2,out v);
			self.cursor=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__isComplete(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._isComplete);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__isComplete(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self._isComplete=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_reses(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.reses);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_reses(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			System.Collections.Generic.List<XLoadRes> v;
			checkType(l,2,out v);
			self.reses=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_keepWaiting(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.keepWaiting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_info(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.info);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_tip(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.tip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isComplete(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isComplete);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_progress(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.progress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ob(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ob);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_obs(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.obs);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_obDict(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.obDict);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_status(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.status);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_assetInfo(IntPtr l) {
		try {
			XLoadDesc self=(XLoadDesc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.assetInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XLoadDesc");
		addMember(l,"cursor",get_cursor,set_cursor,true);
		addMember(l,"_isComplete",get__isComplete,set__isComplete,true);
		addMember(l,"reses",get_reses,set_reses,true);
		addMember(l,"keepWaiting",get_keepWaiting,null,true);
		addMember(l,"info",get_info,null,true);
		addMember(l,"tip",get_tip,null,true);
		addMember(l,"isComplete",get_isComplete,null,true);
		addMember(l,"progress",get_progress,null,true);
		addMember(l,"ob",get_ob,null,true);
		addMember(l,"obs",get_obs,null,true);
		addMember(l,"obDict",get_obDict,null,true);
		addMember(l,"status",get_status,null,true);
		addMember(l,"assetInfo",get_assetInfo,null,true);
		createTypeMetatable(l,constructor, typeof(XLoadDesc),typeof(UnityEngine.CustomYieldInstruction));
	}
}
