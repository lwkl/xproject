using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_XGOIntKeyPool : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int setRealCreate(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.setRealCreate(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				self.setRealCreate(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(XGOIntKeyPool.XPoolRealCreate))){
				XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
				XGOIntKeyPool.XPoolRealCreate a1;
				LuaDelegation.checkDelegate(l,2,out a1);
				self.setRealCreate(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function setRealCreate to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int setRealDestroy(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			XGOIntKeyPool.XPoolRealDestroy a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.setRealDestroy(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int setInit(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			XGOIntKeyPool.XPoolObjInit a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.setInit(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int setDestroy(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			XGOIntKeyPool.XPoolObjDestroy a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.setDestroy(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int realDestroyGameObject(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.realDestroyGameObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int initGameObject(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.initGameObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int destroyGameObject(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.destroyGameObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getgo(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.getgo(a1);
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
	static public int delgo(IntPtr l) {
		try {
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.delgo(a1,a2);
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
			XGOIntKeyPool self=(XGOIntKeyPool)checkSelf(l);
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
			var ret=XGOIntKeyPool.createPool(a1);
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
		getTypeTable(l,"XGOIntKeyPool");
		addMember(l,setRealCreate);
		addMember(l,setRealDestroy);
		addMember(l,setInit);
		addMember(l,setDestroy);
		addMember(l,realDestroyGameObject);
		addMember(l,initGameObject);
		addMember(l,destroyGameObject);
		addMember(l,getgo);
		addMember(l,delgo);
		addMember(l,isWrong);
		addMember(l,createPool_s);
		createTypeMetatable(l,null, typeof(XGOIntKeyPool),typeof(XIntKeyPool));
	}
}
