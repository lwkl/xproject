using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XLoad : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int init(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			var ret=self.init();
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
	static public int getAssetInfoFromCache(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			XAssetKey a1;
			checkValueType(l,2,out a1);
			XLoadStatus a2;
			checkEnum(l,3,out a2);
			var ret=self.getAssetInfoFromCache(a1,a2);
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
	static public int unloadAssetInfoFromCache(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			XAssetKey a1;
			checkValueType(l,2,out a1);
			var ret=self.unloadAssetInfoFromCache(a1);
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
	static public int getAssetInfo(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			XLoadRes a1;
			checkType(l,2,out a1);
			var ret=self.getAssetInfo(a1);
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
	static public int fillDesc(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			XLoadDesc a1;
			checkType(l,2,out a1);
			var ret=self.fillDesc(a1);
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
	static public int realLoad(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			System.Collections.Generic.List<XLoadRes> a1;
			checkType(l,2,out a1);
			System.Action<XLoadDesc> a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.realLoad(a1,a2);
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
	static public int Update(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			self.Update();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int load_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(List<System.String>),typeof(System.Action<XLoadDesc>))){
				System.Collections.Generic.List<System.String> a1;
				checkType(l,1,out a1);
				System.Action<XLoadDesc> a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=XLoad.load(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<XLoadRes>),typeof(System.Action<XLoadDesc>))){
				System.Collections.Generic.List<XLoadRes> a1;
				checkType(l,1,out a1);
				System.Action<XLoadDesc> a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=XLoad.load(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.Action<XLoadDesc>))){
				System.String a1;
				checkType(l,1,out a1);
				System.Action<XLoadDesc> a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=XLoad.load(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.String a1;
				checkType(l,1,out a1);
				System.Type a2;
				checkType(l,2,out a2);
				System.Action<XLoadDesc> a3;
				LuaDelegation.checkDelegate(l,3,out a3);
				var ret=XLoad.load(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function load to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int loadAll_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				System.String a1;
				checkType(l,1,out a1);
				System.Action<XLoadDesc> a2;
				LuaDelegation.checkDelegate(l,2,out a2);
				var ret=XLoad.loadAll(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.String a1;
				checkType(l,1,out a1);
				System.Type a2;
				checkType(l,2,out a2);
				System.Action<XLoadDesc> a3;
				LuaDelegation.checkDelegate(l,3,out a3);
				var ret=XLoad.loadAll(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function loadAll to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getABPath_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=XLoad.getABPath(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(bool))){
				System.String a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				var ret=XLoad.getABPath(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function getABPath to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getABDataPath_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=XLoad.getABDataPath(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(bool))){
				System.String a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				var ret=XLoad.getABDataPath(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function getABDataPath to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getABDataWritePath_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=XLoad.getABDataWritePath(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(bool))){
				System.String a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				var ret=XLoad.getABDataWritePath(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function getABDataWritePath to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_assets(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.assets);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_assets(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			System.Collections.Generic.Dictionary<XAssetKey,XAssetInfo> v;
			checkType(l,2,out v);
			self.assets=v;
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
			XLoad self=(XLoad)checkSelf(l);
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
			XLoad self=(XLoad)checkSelf(l);
			XPackInfo v;
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
	static public int get__fixInfo(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._fixInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set__fixInfo(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			XPackInfo v;
			checkType(l,2,out v);
			self._fixInfo=v;
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
			pushValue(l,XLoad.instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isOK(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isOK);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isLoadFromAB(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isLoadFromAB);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_localver(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.localver);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isReinstall(IntPtr l) {
		try {
			XLoad self=(XLoad)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isReinstall);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"XLoad");
		addMember(l,init);
		addMember(l,getAssetInfoFromCache);
		addMember(l,unloadAssetInfoFromCache);
		addMember(l,getAssetInfo);
		addMember(l,fillDesc);
		addMember(l,realLoad);
		addMember(l,Update);
		addMember(l,load_s);
		addMember(l,loadAll_s);
		addMember(l,getABPath_s);
		addMember(l,getABDataPath_s);
		addMember(l,getABDataWritePath_s);
		addMember(l,"assets",get_assets,set_assets,true);
		addMember(l,"_info",get__info,set__info,true);
		addMember(l,"_fixInfo",get__fixInfo,set__fixInfo,true);
		addMember(l,"instance",get_instance,null,false);
		addMember(l,"isOK",get_isOK,null,true);
		addMember(l,"isLoadFromAB",get_isLoadFromAB,null,true);
		addMember(l,"localver",get_localver,null,true);
		addMember(l,"isReinstall",get_isReinstall,null,true);
		createTypeMetatable(l,null, typeof(XLoad),typeof(UnityEngine.MonoBehaviour));
	}
}
