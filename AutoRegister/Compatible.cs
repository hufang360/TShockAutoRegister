using TShockAPI;


namespace AutoRegister
{
    class Compatible
    {
        public static bool RequireLogin
        {
            // 1.4.0.5
            // get { return TShock.Config.RequireLogin; }

            get { return TShock.Config.Settings.RequireLogin; }
        }

        public static string DefaultRegistrationGroupName
        {
            // 1.4.0.5
            // get { return TShock.Config.DefaultRegistrationGroupName; }

            get { return TShock.Config.Settings.DefaultRegistrationGroupName; }
        }

        public static string DefaultGuestGroupName
        {
            // 1.4.0.5
            // get { return TShock.Config.DefaultGuestGroupName; }

            get { return TShock.Config.Settings.DefaultGuestGroupName; }
        }
    }
}