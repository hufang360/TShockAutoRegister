如果服务器要求登录，会为新用户自动注册和登录。
Fork至：https://github.com/brian91292/TShockAutoRegister
修改点：
1、密码改成纯数字，只有6位，不含4和6；
2、密码保存在AutoRegister.json，且支持命令查询；


命令用法：
/ar on，打开自动注册
/ar off，关闭自动注册
/ar info，服务状态查询
/ar player <playername>，查询指定角色的密码
/ar = /autoregister


AutoRegister.json 示例

```json
{
  "records": {
    "hf": "729351",
    "GoodGoy": "390573"
  },
  "status": 0
}
```
