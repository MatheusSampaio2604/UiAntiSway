
namespace Application.Important_Area
{
    public class ApiRouteMgmt
    {
        public static string Link { get; set; } = "https://localhost";
        public static string Port { get; set; } = ":7022";
        public static string Api { get; set; } = "/api";
    }

    public class ApiRoutePlc
    {
        public static  string Link { get; set; } = "https://localhost";
        public static  string Port { get; set; } = ":5000";
        public static  string Api { get; set; }  = "/api";
    }
}