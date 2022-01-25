using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using System.Reflection;


namespace AutoRegister
{
    /// <summary>
    /// The main plugin class should always be decorated with an ApiVersion attribute. The current API Version is 1.25
    /// </summary>
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Name => "AutoRegister";

        public override string Description => "如果服务器要求登录，会为新用户自动注册和登录。";

        public override string Author => "brian91292 & hufang360";

        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        private static string saveFilename = Path.Combine(TShock.SavePath, "AutoRegister.json");

        private static JsonPasswordRepository passwordRecords;

        /// <summary>
        /// The plugin's constructor
        /// Set your plugin's order (optional) and any other constructor logic here
        /// </summary>
        public Plugin(Main game) : base(game)
        {
        }


        public static void Log(string msg,
                        [CallerMemberName] string member = "",
                        [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"AutoRegister::{member}({line}): {msg}");
        }

        /// <summary>
        /// Performs plugin initialization logic.
        /// Add your hooks, config file read/writes, etc here
        /// </summary>
        public override void Initialize()
        {
            passwordRecords = new JsonPasswordRepository(saveFilename);

            ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreetPlayer, 420);
            Commands.ChatCommands.Add(new Command(new List<string>() { "autoregister" }, ProcessCommand, "autoregister", "ar"));
            Commands.ChatCommands.Add(new Command(new List<string>() { "" }, MyPassword, "mypassword", "pwd"));
        }

        private Dictionary<string, string> tmpPasswords = new Dictionary<string, string>();
        /// <summary>
        /// Tell the player their password if the account was newly generated
        /// </summary>
        /// <param name="args"></param>
        void OnGreetPlayer(GreetPlayerEventArgs args)
        {
            var player = TShock.Players[args.Who];

            if (tmpPasswords.TryGetValue(player.Name + player.UUID + player.IP, out string newPass))
            {
                try
                {
                    player.SendSuccessMessage("已为你自动注册;-)");
                    player.SendSuccessMessage("角色：" + player.Name);
                    player.SendSuccessMessage("密码：" + newPass);

                    // 记录到json
                    passwordRecords.RecordPassword(player.Name, newPass);
                }
                catch { }
                tmpPasswords.Remove(player.Name + player.UUID + player.IP);
            }
            else if (!player.IsLoggedIn && Compatible.RequireLogin && passwordRecords.GetStatus())
            {
                player.SendErrorMessage("抱歉, " + player.Name + " 已被注册！");
                player.SendErrorMessage("请更换角色!");
            }
        }

        /// <summary>
        /// Fired when a new user joins the server.
        /// </summary>
        /// <param name="args"></param>
        void OnServerJoin(JoinEventArgs args)
        {
            //config.json 中配置 RequireLogin=true 时，就自动注册
            //而非 开启SSC
            //TShock.ServerSideCharacterConfig.Enabled
            if (Compatible.RequireLogin && passwordRecords.GetStatus() )
            {
                var player = TShock.Players[args.Who];

                if (TShock.UserAccounts.GetUserAccountByName(player.Name) == null && player.Name != TSServerPlayer.AccountName)
                {
                    //密码长度改成6位，全为数字，但不包含4和6
                    //tmpPasswords[player.Name + player.UUID + player.IP] = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4);

                    // 更安全的密码规则
                    // tmpPasswords[player.Name + player.UUID + player.IP] = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('l', 'L')
                            // .Replace('1', '7').Replace('I', 'i').Replace('O', 'o').Replace('0', 'o');

                    tmpPasswords[player.Name + player.UUID + player.IP] = GenerateRandomNumber(6);
                    TShock.UserAccounts.AddUserAccount(new UserAccount(
                        player.Name,
                        BCrypt.Net.BCrypt.HashPassword(tmpPasswords[player.Name + player.UUID + player.IP].Trim()),
                        player.UUID,
                        Compatible.DefaultRegistrationGroupName,
                        DateTime.UtcNow.ToString("s"),
                        DateTime.UtcNow.ToString("s"),
                        ""));

                    TShock.Log.ConsoleInfo(player.Name + $"注册了账户 ");
                }
            }
        }

        private static char[] constant = { '0', '1', '2', '3', '5', '7', '8', '9'};

        private static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(8)]);
            }
            return newRandom.ToString();
        }



        /// <summary>
        /// 处理命令行指令
        /// </summary>
        private void ProcessCommand(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
            {
                ShowHelpText(args);
                return;
            }

            switch (args.Parameters[0].ToLowerInvariant())
            {
                default:
                case "help":
                    ShowHelpText(args);
                    return;

                case "on":
                    passwordRecords.SetStatus(true);
                    args.Player.SendSuccessMessage("已开启 自动注册功能;-)");
                    return;

                case "off":
                    passwordRecords.SetStatus(false);
                    args.Player.SendInfoMessage("已关闭 自动注册功能");
                    return;

                case "info":
                    args.Player.SendInfoMessage("自动注册情况");
                    args.Player.SendInfoMessage("记录：{0} 条", passwordRecords.GetCount());
                    if (passwordRecords.GetStatus())
                        args.Player.SendInfoMessage("功能：已开启");
                    else
                        args.Player.SendInfoMessage("功能：已关闭");
                    args.Player.SendInfoMessage("用户名："+passwordRecords.GetNameList());
                    return;

                case "player":
                case "p":
                    if(args.Parameters.Count<2)
                    {
                        args.Player.SendErrorMessage("语法错误，用法：/ar player <playername>");
                        return;
                    }
                    string password = passwordRecords.GetPassword(args.Parameters[1]);
                    if(password != "")
                    {
                        args.Player.SendSuccessMessage("角色：{0}", args.Parameters[1]);
                        args.Player.SendSuccessMessage("密码：{0}", password);
                    } else {
                        args.Player.SendErrorMessage("用户 {0} 不是自动注册的，找不到密码记录！", args.Parameters[1]);
                        args.Player.SendErrorMessage("可联系管理员重置密码：");
                        args.Player.SendErrorMessage("/user password <username> <newpassword>", args.Parameters[1]);
                    }
                    return;
            }

        }

        private void MyPassword(CommandArgs args)
        {
            if(!args.Player.RealPlayer){
                args.Player.SendErrorMessage("请在游戏内使用此指令");
                return;
            }

            string password = passwordRecords.GetPassword(args.Player.Name);
            if( password != "" )
            {
                args.Player.SendSuccessMessage("你的密码：{0}", password);
                args.Player.SendSuccessMessage("登录指令：/login {0}", password);
                args.Player.SendSuccessMessage("改密指令：/password <oldpassword> <newpassword>", password);
            } else {
                args.Player.SendErrorMessage("用户 {0} 不是自动注册的，找不到密码记录！", args.Player.Name);
                args.Player.SendErrorMessage("可联系管理员重置密码：");
                args.Player.SendErrorMessage("/user password <username> <newpassword>", args.Player.Name);
            }
        }

        /// <summary>
        /// 命令行说明
        /// </summary>
        private void ShowHelpText(CommandArgs args)
        {
            args.Player.SendInfoMessage("/ar on，打开自动注册");
            args.Player.SendInfoMessage("/ar off，关闭自动注册");
            args.Player.SendInfoMessage("/ar info，服务状态查询");
            args.Player.SendInfoMessage("/ar player <playername>，查询指定角色的密码");
            args.Player.SendInfoMessage("/mypassword，查询自己的密码（普通用户）");
            args.Player.SendInfoMessage("/user password <username> <newpassword>，重置某个用户的密码（管理员）");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreetPlayer);
            }
            base.Dispose(disposing);
        }
    }
}
