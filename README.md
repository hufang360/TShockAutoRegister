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
