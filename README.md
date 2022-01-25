Fork至：https://github.com/brian91292/TShockAutoRegister<br>



# 修改点：<br>
1、密码策略，6位、纯数字、不含4和6；<br>
2、config.json 中 RequireLogin 配置为 true时，会为新用户自动注册和登录。<br>
3、密码保存在 AutoRegister.json，且支持命令查询；<br>
<br>
<br>


# 命令用法：
```shell
/ar info，服务状态查询
/ar on，打开 自动注册 功能
/ar off，关闭 自动注册 功能
/ar player <playername>，查询指定角色的密码

# ar 是 AutoRegister 的缩写

/mypassword, 查看自己的密码
/pwd, 上个指令的简写
# 这两个指令建议开放给普通用户，授权方式举例：/group addperm default mypassword
```
<br>

# 原版TShock中和密码相关的指令：
```shell
/password <旧密码> <新密码>, 更改密码（普通用户用）
/user password <玩家名> <新密码>, 重设密码（管理员用）
```
<br>

# 权限
```shell
autoregister
mypassword
```

授权示例：
```shell
/group addperm default mypassword
```

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
由于原版使用了 玩家IP + 玩家UUID + 玩家名 的规则来生成密码，可以预测出注册用户的密码，有安全隐患，因此被TShock认定为危险插件。[GHSA-w3h6-j2gm-qf7q](https://github.com/Pryaxis/Plugins/security/advisories/GHSA-w3h6-j2gm-qf7q)<br>

fork项目后，密码策略为6位随机数，用户密码不可预测，原项目的维护项目已经更改了这个问题。