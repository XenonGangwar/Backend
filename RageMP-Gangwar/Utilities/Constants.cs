namespace RageMP_Gangwar.Utilities
{
    public static class Constants
    {
        public static class DatabaseConfig
        {
            public static readonly string Host = "localhost";
            public static readonly string User = "root";
            public static readonly string Password = "";
            public static readonly string Database = "gangwar";
            public static readonly string Port = "3306";
            //public static readonly string Host = "45.82.121.159";
            //public static readonly string User = "admin";
            //public static readonly string Password = "";
            // static readonly string Database = "gcgw";
            //public static readonly string Port = "3306";
        }

        public static class ServerConfig
        {
            public static int XPMultiplikator = 1;
        }

        public static class EventConfig
        {
            public static bool isEventActive = false;
            public static int team1 = 0;
            public static int team2 = 0;
        }

        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }
    }
}
