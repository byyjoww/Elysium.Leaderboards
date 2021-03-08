using UnityEngine;

namespace Elysium.Leaderboards
{
    public static partial class Leaderboards
    {
        public static string LeaderboardID = "testing";
    }

    public static class APIData
    {
        public static string Host = "abbe42b6a37574de58e02cb20d5621f8-1478690628.us-east-1.elb.amazonaws.com:8080";
        public static string User = "potato";
        public static string Password = "FKAMb8j4UfGXj9Li9T4";

        public static string LocalHost = "localhost:8080";
        public static string LocalUser = "user";
        public static string LocalPassword = "pass";

        public static string BuildPublicHost() => $"http://{Host}/leaderboard";

        public static string BuildLocalHost() => $"http://{LocalHost}/leaderboard";
    }

    public static class RouteBuilder
    {
        public static string BuildPostIndividualScoreRoute(string _levelID, string _playerID)
        {
            return $"{APIData.BuildPublicHost()}/{_levelID}/{_playerID}".Replace("#", "%23");
        }

        public static string BuildGetIndividualScoreRoute(string _levelID, string _playerID)
        {
            return $"{APIData.BuildPublicHost()}/{_levelID}/{_playerID}".Replace("#", "%23");
        }

        public static string BuildGetHighestScoresRoute(string _levelID)
        {
            return $"{APIData.BuildPublicHost()}/{_levelID}";
        }
    }
}
