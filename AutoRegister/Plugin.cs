using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Localization;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using TShockAPI.Hooks;
using static TShockAPI.GetDataHandlers;

namespace AutoRegister
{
    /// <summary>
    /// The main plugin class should always be decorated with an ApiVersion attribute. The current API Version is 1.25
    /// </summary>
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public override string Name => "AutoRegister";

        /// <summary>
        /// The version of the plugin in its current state.
        /// </summary>
        public override Version Version => new Version(1, 1, 0);

        /// <summary>
        /// The author(s) of the plugin.
        /// </summary>
        public override string Author => "brian91292 · hufang360";

        /// <summary>
        /// A short, one-line, description of the plugin's purpose.
        /// </summary>
        public override string Description => "A Tshock plugin to automatically register a new server-side character if one doesn't already exist for a user.";

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
            ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreetPlayer, 420);
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
                    //player.SendSuccessMessage($"Account \"{player.Name}\" has been registered.");
                    //player.SendSuccessMessage("Your password is " + newPass);
                    player.SendSuccessMessage("已为你自动注册;-)");
                    player.SendSuccessMessage("角色：" + player.Name);
                    player.SendSuccessMessage("密码：" + newPass);

                    TShock.Log.ConsoleInfo("已为你自动注册;-)");
                    TShock.Log.ConsoleInfo("角色：" + player.Name);
                    TShock.Log.ConsoleInfo("密码：" + newPass);
                }
                catch { }
                tmpPasswords.Remove(player.Name + player.UUID + player.IP);
            }
            else if (!player.IsLoggedIn && TShock.Config.RequireLogin)
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
            if (TShock.Config.RequireLogin)
            {
                var player = TShock.Players[args.Who];

                if (TShock.UserAccounts.GetUserAccountByName(player.Name) == null && player.Name != TSServerPlayer.AccountName)
                {
                    //密码长度改成6位，全为数字，但不包含4和6
                    //tmpPasswords[player.Name + player.UUID + player.IP] = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4);
                    tmpPasswords[player.Name + player.UUID + player.IP] = GenerateRandomNumber(6);
                    TShock.UserAccounts.AddUserAccount(new UserAccount(
                        player.Name,
                        BCrypt.Net.BCrypt.HashPassword(tmpPasswords[player.Name + player.UUID + player.IP].Trim()),
                        player.UUID,
                        TShock.Config.DefaultRegistrationGroupName,
                        DateTime.UtcNow.ToString("s"),
                        DateTime.UtcNow.ToString("s"),
                        ""));

                    TShock.Log.ConsoleInfo(player.Name + $"注册了账户: \"{player.Name}\"");
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
        /// Performs plugin cleanup logic
        /// Remove your hooks and perform general cleanup here
        /// </summary>
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
