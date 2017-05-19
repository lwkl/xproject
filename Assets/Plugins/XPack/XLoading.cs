using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XLoading : MonoBehaviour
{
    public static XLoading instance = null;
    public class XLoadingStyle
    {
        public int styleId = 0;
        public Image image;
        public GameObject go;
        public Slider slider;
        public Text info;
        public Text tip;
    }
    public Dictionary<int, XLoadingStyle> styles = new Dictionary<int, XLoadingStyle>();
    public float _progress = 0f;
    public string _info;
    public string _tip; 
    public int _showSyleId = -1;

    /// <summary>
    ///  自动进度
    /// </summary>
    public XIProgress _xprogress = null;

    // 当有自动进度时候xprogress完成时候会调用，显示界面，否则需要手动调用
    public bool _isAutoEnd = false;

    // 添加一种样式
    public bool addStyle(int styleId, GameObject go, Slider slider, Image image, Text info, Text tip )
    {
        delStyle( styleId );

        var st = new XLoadingStyle
        {
            styleId = styleId, image = image, go = go, slider = slider, info = info, tip = tip
        };

        styles.Add(styleId, st);

        if ( go )
        {
            if( go.transform.parent != XApp.instance.uiProgress.transform )
            {
                go.transform.SetParent(XApp.instance.uiProgress.transform);
            }
            go.name = "style" + styleId;
            go.SetActive(false);
        }

        return true;
    }

    // 删除一种样式
    public bool delStyle(int styleId)
    {
        if( styles.ContainsKey(styleId) )
        {
            if( styles[styleId].go )
            {
                // 直接删除
                GameObject.Destroy( styles[styleId].go );
            }
            
            styles.Remove( styleId );

            return true;
        }

        return false;
    }

    // 清除所有的样式
    public void clearStyles()
    {
        foreach (var item in styles)
        {
            if (item.Value.go)
            {
                // 直接删除
                GameObject.Destroy(item.Value.go);
            }
        }
        styles.Clear();
    }

    public void  beginShow( int styleId, XIProgress x = null, bool isAutoEnd = false )
    {
        // 显示自己
        XApp.instance.uiProgress.gameObject.SetActive( true );
        
        foreach( var item in styles )
        {
            if( item.Value.go )
            {
                if( item.Key != styleId )
                {
                    item.Value.go.SetActive(false);
                }
                else
                {
                    item.Value.go.SetActive(true);
                }
            }
        }
        _showSyleId = styleId;
        _isAutoEnd = isAutoEnd;

        setAutoProgress( x );

        // 重新赋值
        progress = _progress;
        info = _info;
        tip = _tip;
    }

    public void endShow()
    {
        foreach (var item in styles)
        {
            if ( item.Value.go )
            {
                item.Value.go.SetActive( false );
            }
            if (item.Value.info)
            {
                item.Value.info.text = "";
            }
            if (item.Value.tip)
            {
                item.Value.tip.text = "";
            }
        }
        XApp.instance.uiProgress.gameObject.SetActive(false);
        _showSyleId = -1;
        setAutoProgress( null );
    }


    public bool isShowProgress
    {
        get
        {
            return _showSyleId >= 0;
        }
    }
    public float progress // 取值0-1
    {
        get
        {
            return _progress;
        }
        set
        {
            _progress = value;

            if( styles.ContainsKey(_showSyleId) )
            {
                if( styles[_showSyleId].slider )
                {
                    styles[_showSyleId].slider.value = _progress;
                }
            }
        }
    }

    public string info
    {
        get
        {
            return _info;
        }
        set
        {
            _info = value;

            if (styles.ContainsKey(_showSyleId))
            {
                if (styles[_showSyleId].info)
                {
                    styles[_showSyleId].info.text = _info;
                }
            }
        }
    }

    public string tip
    {
        get
        {
            return _tip;
        }
        set
        {
            _tip = value;

            if (styles.ContainsKey(_showSyleId))
            {
                if (styles[_showSyleId].tip)
                {
                    styles[_showSyleId].tip.text = _tip;
                }
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // 旋转
        if ( styles.ContainsKey(_showSyleId ) )
        {
            if (styles[_showSyleId].image )
            {
                styles[_showSyleId].image.transform.Rotate(new Vector3(0f, 0f, -180f * Time.deltaTime));
            }
        }

        updateInfo();
    }

    // 更新信息
    public void updateInfo()
    {
        if( _xprogress != null )
        {
            this.progress = _xprogress.progress;
            this.tip = _xprogress.tip;
            this.info = _xprogress.info;

            if( this._isAutoEnd && _xprogress.isComplete )
            {
                endShow();
            }
        }
    }

    public void setAutoProgress( XIProgress x )
    {
        if( _xprogress != x )
        {
            _xprogress = x;
        }
    }

}
