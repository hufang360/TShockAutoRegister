
tshock v4.5.0 api有变动
Updated OTAPI and TSAPI to Terraria 1.4.2.1.



tshock v4.5.0.1(1.4.2.1) 报错
```
127.0.0.1:53146正在连接...
[Server API] Warning Plugin "AutoRegister" has had an unhandled exception thrown by one of its ServerJoin handlers:
System.MissingMethodException: 找不到方法:“TShockAPI.ConfigFile TShockAPI.TShock.get_Config()”。
   在 AutoRegister.Plugin.OnServerJoin(JoinEventArgs args)
   在 TerrariaApi.Server.HandlerCollection`1.Invoke(ArgsType args) 位置 D:\a\TShock\TShock\TerrariaServerAPI\TerrariaServerAPI\TerrariaApi.Server\HandlerCollection.cs:行号 109
hf authenticated successfully as user hf.
[Server API] Warning Plugin "AutoRegister" has had an unhandled exception thrown by one of its NetGreetPlayer handlers: 
System.MissingMethodException: 找不到方法:“TShockAPI.ConfigFile TShockAPI.TShock.get_Config()”。
   在 AutoRegister.Plugin.OnGreetPlayer(GreetPlayerEventArgs args)
   在 TerrariaApi.Server.HandlerCollection`1.Invoke(ArgsType args) 位置 D:\a\TShock\TShock\TerrariaServerAPI\TerrariaServerAPI\TerrariaApi.Server\HandlerCollection.cs:行号 109
```


- 测试dll
T:\TShock\1421\测试插件.bat
```bash
@echo off
set "file1=T:\\TShock\\TShockAutoRegister\\AutoRegister\\bin\\Debug\\AutoRegister.dll"
set "file2=T:\\TShock\\1412\\tshock-client\\ServerPlugins\\AutoRegister.dll"
copy /y "%file1%" "%file2%"

cd /d "T:\\TShock\\1421\\"
run.bat
```

```bash
/group addperm default autoregister
```