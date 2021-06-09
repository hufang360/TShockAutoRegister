Fork至：https://github.com/brian91292/TShockAutoRegister<br>



# 修改点：<br>
1、密码策略，6位、纯数字、不含4和6；<br>
2、config.json 中 RequireLogin 配置为 true时，会为新用户自动注册和登录。<br>
3、密码保存在 AutoRegister.json，且支持命令查询；<br>
<br>
<br>


# 命令用法：<br>
/ar info，服务状态查询<br>
/ar on，打开自动注册<br>
/ar off，关闭自动注册<br>
/ar p <playername>，查询指定角色的密码<br>

ar 是 AutoRegister 的缩写


# AutoRegister.json 示例 <br>
```json
{
  "records": {
    "hf": "729351",
    "GoodGoy": "390573"
  },
  "status": 0
}
```

# 关于 1.2.1 版本
由于原版使用用户的 IPv4 地址、客户端 UUID 和字符名称，可以预测的注册用户的密码，因此被TShock认定为危险插件。[GHSA-w3h6-j2gm-qf7q](https://github.com/Pryaxis/Plugins/security/advisories/GHSA-w3h6-j2gm-qf7q)<br>

fork项目后，密码策略为6位随机数，用户密码不可预测，于是仅更改插件版本号，以便能在 TShock v4.5.4 上使用。