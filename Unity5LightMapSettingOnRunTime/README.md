# Unity5.x LightMapSettingOnRunTime
## 问题
* Unity5.x运行时设置光照贴图
* 分别烘焙过得Prefab在同一场景中实例

## 方案
在一个场景中调好灯光及烘焙参数后，将要烘焙的GameObject添加到一个空对象下，制成Prefab，进行烘焙，这些操作均由3D设计操作，成功后导出Prefab和光照贴图到项目使用。这样就可以在一个场景中可以添加许多相同或者不同Prefab，并且都是烘焙好的效果。

## 使用方法
1. 在Prefab上添加控制脚本
2. 在所有含有Renderer的GameObject上添加数据存储脚本
3. 烘焙
4. 使用时不需要做其他设置，Prefab实例化时会做相应得操作

