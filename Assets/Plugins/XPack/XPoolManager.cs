using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XPool : MonoBehaviour
{
    public XPoolType PoolType = XPoolType.SINGLE;

    protected virtual void Awake()
    {
    }

    static public XPool createPool(string str)
    {
        return null;
    }

    public virtual void clear()
    {
    }

    public virtual void min()
    {
    }

    public virtual bool isWrong( object ob )
    {
        if( ob == null )
        {
            return true;
        }
        return false;
    }

    public int _maxSize = 20;
    public int _minSize = 5;
}


// 默认的XPool继承类
public class XSinglePool :XPool
{
    public delegate object XPoolRealCreate();
    public delegate void XPoolRealDestroy(object ob);
    public delegate void XPoolObjInit(object ob);
    public delegate void XPoolObjDestroy(object ob);

    protected override void Awake()
    {
        PoolType = XPoolType.SINGLE;
    }

    // 创建自己本身
    public static new XSinglePool createPool(string str)
    {
        GameObject go = new GameObject();
        go.name = str;
        return go.AddComponent<XSinglePool>();
    }

    public object getObj()
    {
        object ob = null;
        if ( _pool.Count > 0 )
        {
            ob = _pool[_pool.Count - 1 ];
            _pool.RemoveAt( _pool.Count - 1 );
        }
        else if ( _realCreate != null )
        {
            ob = _realCreate();
        }

        if ( isWrong( ob) )
        {
            return null;
        }

        if ( _objInit != null )
        {
            _objInit(ob);
        }

        return ob;
    }

    

    public void delObj( Object ob )
    {
        if( isWrong(ob) )
        {
            XDebug.LogError("delObj空对象");
            return;
        }

        if( _objDestroy != null )
        {
            _objDestroy( ob );
        }

        if( _pool.Count < _maxSize )
        {
            _pool.Add(ob);
        }
        else if( _realDestroy != null )
        {
            _realDestroy( ob );
        }
        
    }

    public object createObj()
    {
        object ob = null;
        if (_realCreate != null)
        {
            ob = _realCreate();
        }

        if (isWrong(ob))
        {
            return null;
        }

        if (_objInit != null)
        {
            _objInit(ob);
        }

        return ob;
    }

    public override void clear()
    {
        if( _realDestroy != null )
        {
            List<object> oldPool = _pool;
            _pool = new List<object>();
            for(int i=0; i<oldPool.Count; i++)
            {
                _realDestroy(oldPool[i]);
            }
        }
        else
        {
            _pool.Clear();
        }
    }

    public override void min()
    {
        if( _pool.Count > _minSize )
        {
            int delnum = _pool.Count - _minSize;
            
            if( _realDestroy != null )
            {
                for(int i=delnum -1; i>=0; i-- )
                {
                    _realDestroy(_pool[_minSize + i]);
                    _pool.RemoveAt(_minSize + i);
                }
            }
            else
            {
                for (int i = delnum - 1; i >= 0; i--)
                {
                    _realDestroy(_pool[_minSize + i]);
                    _pool.RemoveAt(_minSize + i);
                }
            }

        }
    }

    protected XPoolRealCreate _realCreate = null;
    protected XPoolRealDestroy _realDestroy = null;
    protected XPoolObjInit _objInit = null;
    protected XPoolObjDestroy _objDestroy = null;
    public List<object> _pool = new List<object>();
}



// 双层int为key的继承
public class XIntKeyPool :XPool
{
    public delegate object XPoolRealCreate(int id);
    public delegate void XPoolRealDestroy(object ob, int id);
    public delegate void XPoolObjInit(object ob, int id);
    public delegate void XPoolObjDestroy(object ob, int id);

    protected override void Awake()
    {
        PoolType = XPoolType.INTKEY;
    }

    // 创建自己本身
    public static new XIntKeyPool createPool(string str)
    {
        GameObject go = new GameObject();
        go.name = str;
        return go.AddComponent<XIntKeyPool>();
    }

    public object getObj(int id)
    {

        List<object> pool = null;
        if( _pool.ContainsKey( id ) )
        {
            pool = _pool[id];
        }

        object ob = null;
        if ( pool != null && pool.Count > 0 )
        {
            ob = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        }
        else if ( _realCreate != null )
        {
            ob = _realCreate( id );
        }

        if ( isWrong( ob ) )
        {
            return null;
        }

        if ( _objInit != null )
        {
            _objInit(ob, id);
        }

        return ob;

    }

    public void delObj(Object ob, int id)
    {
        if (isWrong(ob) )
        {
            XDebug.LogError("delObj空对象");
            return;
        }

        if (_objDestroy != null)
        {
            _objDestroy(ob, id);
        }

        List<object> pool = null;
        if (_pool.ContainsKey(id))
        {
            pool = _pool[id];
        }
        else
        {
            pool = new List<object>();
            _pool[id] = pool;
        }

        if ( pool.Count <= _maxSize)
        {
            pool.Add(ob);
        }
        else if (_realDestroy != null)
        {
            _realDestroy(ob, id);
        }

    }

    Dictionary<int, List<object>> _delvp = new Dictionary<int, List<object>>();

    public override void clear()
    {
        if ( _realDestroy != null )
        {
            _delvp.Clear();
            foreach (var vpool in _pool)
            {
                _delvp.Add(vpool.Key, vpool.Value);
            }
            _pool.Clear();


            foreach (var vpool in _delvp)
            {
                List<object> oldPool = vpool.Value;
                for (int i = 0; i < oldPool.Count; i++)
                {
                    _realDestroy(oldPool[i], vpool.Key);
                }
            }
            _delvp.Clear();

        }
        else
        {
            _pool.Clear();
        }
    }

    List<int> _delPool = new List<int>();
    public override void min()
    {
        _delPool.Clear();

        foreach(var vpool in _pool )
        {
            List<object> pool = vpool.Value;

            if (pool.Count > _minSize)
            {
                int delnum = pool.Count - _minSize;

                if (_realDestroy != null)
                {
                    for (int i = delnum - 1; i >= 0; i--)
                    {
                        _realDestroy(pool[_minSize + i], vpool.Key);
                        pool.RemoveAt(_minSize + i);
                    }

                }
                else
                {
                    for (int i = delnum - 1; i >= 0; i--)
                    {
                        pool.RemoveAt(_minSize + i);
                    }
                }

            }
            
            if( pool.Count == 0 )
            {
                _delPool.Add(vpool.Key);
            }
        }

        for(int i=0; i< _delPool.Count; i++ )
        {
            _pool.Remove( _delPool[i] );
        }

    }

    protected XPoolRealCreate _realCreate = null;
    protected XPoolRealDestroy _realDestroy = null;
    protected XPoolObjInit _objInit = null;
    protected XPoolObjDestroy _objDestroy = null;
    Dictionary<int, List<object>> _pool = new Dictionary<int,List<object>>();
}

// 双层int为key的继承
public class XStrKeyPool : XPool
{
    public delegate object XPoolRealCreate(string id);
    public delegate void XPoolRealDestroy(object ob, string id);
    public delegate void XPoolObjInit(object ob, string id);
    public delegate void XPoolObjDestroy(object ob, string id);


    protected override void Awake()
    {
        PoolType = XPoolType.STRKEY;
    }

    // 创建自己本身
    public static new XStrKeyPool createPool(string str)
    {
        GameObject go = new GameObject();
        go.name = str;
        return go.AddComponent<XStrKeyPool>();
    }

    public object getObj(string id)
    {

        List<object> pool = null;
        if ( _pool.ContainsKey(id) )
        {
            pool = _pool[id];
        }


        object ob = null;
        if ( pool != null && pool.Count > 0 )
        {
            ob = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        }
        else if (_realCreate != null)
        {
            ob = _realCreate(id);
        }

        
        if ( isWrong(ob) )
        {
            return null;
        }

        if (_objInit != null)
        {
            _objInit(ob, id);
        }

        return ob;

    }

    public void delObj(Object ob, string id)
    {

        if ( isWrong(ob) )
        {
            XDebug.LogError("delObj空对象");
            return;
        }

        if (_objDestroy != null)
        {
            _objDestroy(ob, id);
        }

        List<object> pool = null;
        if (_pool.ContainsKey(id))
        {
            pool = _pool[id];
        }
        else
        {
            pool = new List<object>();
            _pool[id] = pool;
        }

        if (pool.Count <= _maxSize)
        {
            pool.Add(ob);
        }
        else if (_realDestroy != null)
        {
            _realDestroy(ob, id);
        }

    }

    Dictionary<string, List<object>> _delvp = new Dictionary<string, List<object>>();

    public override void clear()
    {
        if (_realDestroy != null)
        {
            _delvp.Clear();
            foreach (var vpool in _pool)
            {
                _delvp.Add(vpool.Key, vpool.Value);
            }
            _pool.Clear();

            foreach ( var vpool in _delvp )
            {
                  
                List<object> oldPool = vpool.Value;

                for (int i = 0; i < oldPool.Count; i++)
                {
                    _realDestroy(oldPool[i], vpool.Key);
                }
            }
            _delvp.Clear();

        }
        else
        {

            _pool.Clear();
        }

    }

    List<string> _delPool = new List<string>();
    public override void min()
    {
        _delPool.Clear();

        foreach (var vpool in _pool)
        {
            List<object> pool = vpool.Value;

            if (pool.Count > _minSize)
            {
                int delnum = _pool.Count - _minSize;

                if (_realDestroy != null)
                {

                        for (int i = delnum - 1; i >= 0; i--)
                        {
                            _realDestroy(pool[_minSize + i], vpool.Key);
                            pool.RemoveAt(_minSize + i);
                        }


                }
                else
                {
                    for (int i = delnum - 1; i >= 0; i--)
                    {
                        pool.RemoveAt(_minSize + i);
                    }
                }

            }

            if (pool.Count == 0)
            {
                _delPool.Add(vpool.Key);
            }
        }

        for (int i = 0; i < _delPool.Count; i++)
        {
            _pool.Remove(_delPool[i]);
        }

    }

    protected XPoolRealCreate _realCreate = null;
    protected XPoolRealDestroy _realDestroy = null;
    protected XPoolObjInit _objInit = null;
    protected XPoolObjDestroy _objDestroy = null;

    Dictionary<string, List<object>> _pool = new Dictionary<string,List<object>>();
}

public class XGOSinglePool :XSinglePool
{
    public new delegate GameObject XPoolRealCreate();
    public new delegate void XPoolRealDestroy(GameObject ob);
    public new delegate void XPoolObjInit(GameObject ob);
    public new delegate void XPoolObjDestroy(GameObject ob);

    protected override void Awake()
    {
        PoolType = XPoolType.GOSINGLE;

        _realCreate = () => { return new GameObject(); };

        setRealDestroy( realDestroyGameObject );
        setInit( initGameObject );
        setDestroy( destroyGameObject );
    }

    // 创建自己本身
    public static new XGOSinglePool createPool(string str)
    {
        GameObject go = new GameObject();
        go.name = str;
        return go.AddComponent<XGOSinglePool>();
    }

    public void setRealCreate( XPoolRealCreate real )
    {
        _realCreate = () => { return real(); };
    }

    public void setRealDestroy( XPoolRealDestroy real )
    {
        _realDestroy = (ob) => real( ob as GameObject );
    }

    public void setInit(XPoolObjInit real)
    {
        _objInit = (ob) => real(ob as GameObject);
    }

    public void setDestroy(XPoolObjDestroy real)
    {
        _objDestroy = (ob) => real(ob as GameObject);
    }

    public void setRealCreate( GameObject ob )
    {
        setRealCreate(() => { return GameObject.Instantiate(ob) as GameObject; });
    }

    public void setRealCreate( string str )
    {
        GameObject ob = Resources.Load( str ) as GameObject;
        setRealCreate( ob );
    }


    protected void realDestroyGameObject(GameObject go)
    {
        GameObject.Destroy( go );
    }

    protected void initGameObject( GameObject go )
    {
        // XDebug.LogError("initGameObject" + go.ToString());
        go.transform.SetParent( null);
        go.SetActive(true);
    }

    protected void destroyGameObject( GameObject go)
    {
        // XDebug.LogError("destroyGameobject" + go.ToString());
        go.transform.SetParent(this.gameObject.transform);
        go.SetActive(false);
    }

    public GameObject getgo()
    {
        return getObj() as GameObject;
    }

    public void delgo( GameObject go )
    {
        delObj(go);
    }

    public override bool isWrong( object ob )
    {
        

        GameObject go = ob as GameObject;
        if( go == null )
        {
            XDebug.LogError("isWrone" + ob.ToString());
            return true;
        }

        return false;

    }
}


public class XGOIntKeyPool : XIntKeyPool
{
    public new delegate GameObject XPoolRealCreate(int id);
    public new delegate void XPoolRealDestroy(GameObject ob, int id);
    public new delegate void XPoolObjInit(GameObject ob, int id);
    public new delegate void XPoolObjDestroy(GameObject ob, int id);

    protected override void Awake()
    {
        PoolType = XPoolType.GOINTKEY;

        _realCreate = (id) => { return new GameObject(); };

        // setRealCreate( realCreateGameObject );
        setRealDestroy( realDestroyGameObject );
        setInit( initGameObject );
        setDestroy( destroyGameObject );
    }


    // 创建自己本身
    public static new XGOIntKeyPool createPool(string str)
    {
        GameObject go = new GameObject();
        go.name = str;
        return go.AddComponent<XGOIntKeyPool>();
    }

    public void setRealCreate(XPoolRealCreate real)
    {
        _realCreate = (id) => { return real(id); };
    }

    public void setRealDestroy(XPoolRealDestroy real)
    {
        _realDestroy = (ob, id) => real(ob as GameObject, id);
    }

    public void setInit(XPoolObjInit real)
    {
        _objInit = (ob, id) => real(ob as GameObject, id);
    }

    public void setDestroy(XPoolObjDestroy real)
    {
        _objDestroy = (ob, id) => real(ob as GameObject, id);
    }

    public void setRealCreate( GameObject ob )
    {
        setRealCreate((id) => { return GameObject.Instantiate(ob) as GameObject; } );
    }

    public void setRealCreate(string str)
    {
        GameObject ob = Resources.Load(str) as GameObject;
        setRealCreate(ob);
    }


    public void realDestroyGameObject( GameObject go, int id )
    {
        GameObject.Destroy(go);
    }

    public void initGameObject( GameObject go, int id )
    {
        go.transform.SetParent(null);
        go.SetActive(true);
    }

    public void destroyGameObject( GameObject go, int id )
    {
        go.transform.SetParent(this.gameObject.transform);
        go.SetActive(false);
    }

    public GameObject getgo( int id)
    {
        return getObj( id ) as GameObject;
    }

    public void delgo(GameObject go, int id )
    {
        delObj(go, id);
    }

    public override bool isWrong(object ob)
    {
        GameObject go = ob as GameObject;
        if (go == null)
        {
            return true;
        }

        return false;

    }

}

public class XGOStrKeyPool : XStrKeyPool
{
    public new delegate GameObject XPoolRealCreate(string id);
    public new delegate void XPoolRealDestroy(GameObject ob, string id);
    public new delegate void XPoolObjInit(GameObject ob, string id);
    public new delegate void XPoolObjDestroy(GameObject ob, string id);


    protected override void Awake()
    {
        PoolType = XPoolType.GOSTRKEY;
        _realCreate = (str) => { return new GameObject(); };
        setRealDestroy( realDestroyGameObject );
        setInit( initGameObject );
        setDestroy( destroyGameObject );
    }


    // 创建自己本身
    public static new XGOStrKeyPool createPool(string str)
    {
        GameObject go = new GameObject();
        go.name = str;
        return go.AddComponent<XGOStrKeyPool>();
    }
    public void setRealCreate(XPoolRealCreate real)
    {
        _realCreate = (id) => { return real(id); };
    }

    public void setRealDestroy(XPoolRealDestroy real)
    {
        _realDestroy = (ob, id) => real(ob as GameObject, id);
    }

    public void setInit(XPoolObjInit real)
    {
        _objInit = (ob, id) => real(ob as GameObject, id);
    }

    public void setDestroy(XPoolObjDestroy real)
    {
        _objDestroy = (ob, id) => real(ob as GameObject, id);
    }

    public void setRealCreate(GameObject ob)
    {
        setRealCreate((id) => { return GameObject.Instantiate(ob) as GameObject; });
    }

    public void setRealCreate(string str)
    {
        GameObject ob = Resources.Load(str) as GameObject;
        setRealCreate(ob);
    }

    protected void realDestroyGameObject(GameObject go, string id)
    {
        GameObject.Destroy(go);
    }

    protected void initGameObject(GameObject go, string id)
    {
        go.transform.SetParent( null);
        go.SetActive(true);
    }

    protected void destroyGameObject(GameObject go, string id)
    {
        go.transform.SetParent(this.gameObject.transform);
        go.SetActive(false);
    }

    public GameObject getgo(string id)
    {
        return getObj(id) as GameObject;
    }

    public void delgo(GameObject go, string id)
    {
        delObj(go, id);
    }

    public override bool isWrong(object ob)
    {        
        GameObject go = ob as GameObject;
        if (go == null)
        {
            return true;
        }

        return false;
    }
}

public enum XPoolType
{
    SINGLE,
    INTKEY,
    STRKEY,
    GOSINGLE,
    GOINTKEY,
    GOSTRKEY,
}



// 管理所有Pool对象的类
public class XPoolManager : MonoBehaviour 
{
    Dictionary<string, XPool> _pools = new Dictionary<string,XPool>();

    public XPool getPool( string str, XPoolType tp )
    {
        if( _pools.ContainsKey(str) )
        {
            if (tp != _pools[str].PoolType)
            {
                XDebug.LogError("！！注意！！2次获取的同名pool类型不一样" + str );
            }
            return _pools[str];
        }
        
        XPool pool = null;
        if( tp == XPoolType.SINGLE )
        {
            pool = XSinglePool.createPool(str);
        }
        else if( tp == XPoolType.INTKEY )
        {
            pool = XIntKeyPool.createPool(str);
        }
        else if (tp == XPoolType.STRKEY )
        {
            pool = XStrKeyPool.createPool(str);
        }
        else if(tp == XPoolType.GOSINGLE )
        {
            pool = XGOSinglePool.createPool(str);
        }
        else if (tp == XPoolType.GOINTKEY )
        {
            pool = XGOIntKeyPool.createPool(str);
        }
        else if (tp == XPoolType.GOSTRKEY )
        {
            pool = XGOStrKeyPool.createPool(str);
        }

        if( pool != null )
        {
            pool.transform.SetParent(this.transform);
            _pools.Add(str, pool);
        }

        return pool;

    }

    public XSinglePool getSingle(string str)
    {
        return getPool(str, XPoolType.SINGLE) as XSinglePool;
    }

    public XIntKeyPool getIntKey(string str)
    {
        return getPool(str, XPoolType.INTKEY) as XIntKeyPool;
    }

    public XStrKeyPool getStrKey(string str)
    {
        return getPool(str, XPoolType.STRKEY) as XStrKeyPool;
    }


    public XGOSinglePool getGOSingle( string str )
    {
        return getPool(str, XPoolType.GOSINGLE) as XGOSinglePool;
    }

    public XGOIntKeyPool getGOIntKey(string str)
    {
        return getPool(str, XPoolType.GOINTKEY) as XGOIntKeyPool;
    }

    public XGOStrKeyPool getGOStrKey(string str)
    {
        return getPool(str, XPoolType.GOSTRKEY) as XGOStrKeyPool;
    }


    public void clear()
    {
        foreach( var v in _pools )
        {
            v.Value.clear();
        }
    }

    public void min()
    {
        foreach (var v in _pools)
        {
            v.Value.min();
        }
    }

}
