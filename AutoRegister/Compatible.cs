using TShockAPI;
using System;


namespace AutoRegister
{
    class Compatible
    {
        public static Boolean RequireLogin
        {
            // 1.4.0.5
            // get { return TShock.Config.RequireLogin; }

            get { return TShock.Config.Settings.RequireLogin; }
        }

        public static String DefaultRegistrationGroupName
        {
            // 1.4.0.5
            // get { return TShock.Config.DefaultRegistrationGroupName; }

            get { return TShock.Config.Settings.DefaultRegistrationGroupName; }
        }
    }
}