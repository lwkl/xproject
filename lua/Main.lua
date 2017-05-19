Vector3 = UnityEngine.Vector3
Vector2 = UnityEngine.Vector2
Rect = UnityEngine.Rect
Quaternion = UnityEngine.Quaternion
GameObject = UnityEngine.GameObject
Time = UnityEngine.Time


function main()


    --local x = GameObject.Find('UIRoot/style0/Info'):GetComponent( UnityEngine.UI.Text )
    --x.text = 'use lua ok'
    
    print('use lua ok'..tostring(XLoading.instance.info))
    XLoading.instance.info = 'lua也好了...开始装载poke'
    
    XLoading.instance:endShow()
    
    XLoad.loadAll('Assets/Res/poke.png', UnityEngine.Sprite, function(ld)
        print('111111111111111111111111111111111111111111111111')
        print(ld.obs, ld.status)
        if ld.obs ~= nil then
            print('load poker ok', ld.obs.Length )
            for i=1, ld.obs.Length, 1 do
            
                local go = GameObject()
                
                local sprite = ld.obs[i]
                local img = go:AddComponent( UnityEngine.UI.Image)
                img.sprite = sprite
                
                go.transform:SetParent( XApp.instance.uiWindow )
                img.rectTransform.anchoredPosition = Vector2(i* 100, 0)
                
                
                LuaTimer.Add(5000,  function () 
                    ld.assetInfo:unload()
                end)
            end
            
        end
    end )
    
    
end


function imok()
    print('iamok')
end