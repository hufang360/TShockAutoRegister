如果服务器要求登录，会为新用户自动注册和登录。<br>
Fork至：https://github.com/brian91292/TShockAutoRegister<br>

修改点：<br>
1、密码改成纯数字，只有6位，不含4和6；<br>
2、密码保存在AutoRegister.json，且支持命令查询；<br>
<br>
<br>
命令用法：<br>
/ar on，打开自动注册<br>
/ar off，关闭自动注册<br>
/ar info，服务状态查询<br>
/ar player <playername>，查询指定角色的密码<br>
/ar = /autoregister<br>


AutoRegister.json 示例<br>

```json
{
  "records": {
    "hf": "729351",
    "GoodGoy": "390573"
  },
  "status": 0
}
```
