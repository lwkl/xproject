using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XLoading : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int addStyle(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			UnityEngine.GameObject a2;
			checkType(l,3,out a2);
			UnityEngine.UI.Slider a3;
			checkType(l,4,out a3);
			UnityEngine.UI.Image a4;
			checkType(l,5,out a4);
			UnityEngine.UI.Text a5;
			checkType(l,6,out a5);
			UnityEngine.UI.Text a6;
			checkType(l,7,out a6);
			var ret=self.addStyle(a1,a2,a3,a4,a5,a6);
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
	static public int delStyle(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.delStyle(a1);
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
	static public int clearStyles(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			self.clearStyles();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int beginShow(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			XIProgress a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.beginShow(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int endShow(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			self.endShow();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int updateInfo(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			self.updateInfo();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int setAutoProgress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			XIProgress a1;
			checkType(l,2,out a1);
			self.setAutoProgress(a1);
			pushValue(l,true);
			return 1;
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
			pushValue(l,XLoading.instance);
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
			XLoading v;
			checkType(l,2,out v);
			XLoading.instance=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_styles(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.styles);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_styles(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Collections.Generic.Dictionary<System.Int32,XLoading.XLoadingStyle> v;
			checkType(l,2,out v);
			self.styles=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__progress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._progress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__progress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self._progress=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__info(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._info);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__info(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self._info=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__tip(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._tip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__tip(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self._tip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__showSyleId(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._showSyleId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__showSyleId(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self._showSyleId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__xprogress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._xprogress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__xprogress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			XIProgress v;
			checkType(l,2,out v);
			self._xprogress=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get__isAutoEnd(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._isAutoEnd);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__isAutoEnd(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self._isAutoEnd=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isShowProgress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isShowProgress);
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
			XLoading self=(XLoading)checkSelf(l);
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
	static public int set_progress(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.progress=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_info(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
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
	static public int set_info(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.info=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_tip(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
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
	static public int set_tip(IntPtr l) {
		try {
			XLoading self=(XLoading)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.tip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XLoading");
		addMember(l,addStyle);
		addMember(l,delStyle);
		addMember(l,clearStyles);
		addMember(l,beginShow);
		addMember(l,endShow);
		addMember(l,updateInfo);
		addMember(l,setAutoProgress);
		addMember(l,"instance",get_instance,set_instance,false);
		addMember(l,"styles",get_styles,set_styles,true);
		addMember(l,"_progress",get__progress,set__progress,true);
		addMember(l,"_info",get__info,set__info,true);
		addMember(l,"_tip",get__tip,set__tip,true);
		addMember(l,"_showSyleId",get__showSyleId,set__showSyleId,true);
		addMember(l,"_xprogress",get__xprogress,set__xprogress,true);
		addMember(l,"_isAutoEnd",get__isAutoEnd,set__isAutoEnd,true);
		addMember(l,"isShowProgress",get_isShowProgress,null,true);
		addMember(l,"progress",get_progress,set_progress,true);
		addMember(l,"info",get_info,set_info,true);
		addMember(l,"tip",get_tip,set_tip,true);
		createTypeMetatable(l,null, typeof(XLoading),typeof(UnityEngine.MonoBehaviour));
	}
}
