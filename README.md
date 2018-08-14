# QP.Framework

一个整合了XLua的Unity5 AssetBundle 轻量级的热更新框架，实现了模块化下载的一系列流程，使用简单且灵活

启动
---
启动场景在Assets/Framework/AppStart中

修改Assets/Defined/GameConfig.cs 的 gameModel

* Editor 不更新，需要把Modules下面所有场景添加到File->Build Settings中
* Local  更新，下载后的数据会存放到StreamingAssets目录，在打包AssetBundle的时候也是这个目录，这个时候需要把StreamingAssets下的资源放到服务器，然后删除StreamingAssets下的Modules目录才能看到更新效果
* Remote 更新，发布版本，对应的目录自行百度

*

描述
---
Modules（Modules这个目录可以自定义）下的每个一级子目录都看作是一个独立的模块，可以是一个游戏玩法也可以是游戏大厅，也可以是公共资源
在启动游戏的时候根据自己的需求去下载必要的模块，比如：先下载LuaFramework->下载Common公共资源->GameBox游戏大厅。然后根据需求在适当的时机去下载其他模块。

打包AssetBundle
--
框架会打包Modules目录下的每个模块，每个模块的子目录结构：
* AB_Lua
* AB_Prefab
* AB_Scene
* AB_Texture
* AB_Material
* AB_Audio

如果这些还不够可以在 Editor/Package.cs 中自己添加，每个模块中可以选择性的创建这些目录，框架只会打包这些目录下的所有资源

如果你想把AB_xxx内所有资源打成一个AssetBundle的话，只需要在AB_xxx中创建一个以 `_`下划线开头命名的目录，这代表这个目录下的资源要打成一个资源包,AssetBundle的包名就是这个目录的名，如果没有以`_`下划线开头命名的目录默认会把资源单独打包

注意事项
---
    如果你有一个目录是这样的 AB_Prefab/_package/panel/_common/panel.prefab,
    这种结构会把panel.prefab打到_common中，_common目录之外的会打包到_package中




