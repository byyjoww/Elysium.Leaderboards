using Elysium.Networking;
using Elysium.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Elysium.Leaderboards
{
    public static partial class Leaderboards
    {
        private static HttpSender sender = default;

        public static HttpSender Sender
        {
            get
            {
                if (sender == null) { sender = new HttpSender(); }
                return sender;
            }

            private set => sender = value;
        }

        public static async Task PostPlayerScore(string _levelID, string _playerID, long _score, Action<bool> _callback)
        {
            await PostIndividualScore(_levelID, _playerID, _score, _callback);
        }

        private static async Task PostIndividualScore(string _levelID, string _playerID, long _score, Action<bool> _callback)
        {
            var ctx = new SetScoreRequest
            {
                score = _score,
            };

            var serializer = new DataContractJsonSerializer(typeof(SetScoreRequest));
            var output = string.Empty;

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, ctx);
                output = Encoding.UTF8.GetString(ms.ToArray());
            }

            var url = RouteBuilder.BuildPostIndividualScoreRoute(_levelID, _playerID);
            var body = new StringContent(output, Encoding.UTF8, "application/json");

            var success = await Sender.SendHttpsRequest(HttpMethod.Post, url, APIData.User, APIData.Password, body);

            _callback(success.isSuccess);
        }

        public static async Task GetPlayerScore(string _levelID, string _playerID, Action<bool, GetScoreRequest> _callback)
        {
            await GetIndividualScore(_levelID, _playerID, _callback);
        }

        private static async Task GetIndividualScore(string _levelID, string _playerID, Action<bool, GetScoreRequest> _callback)
        {
            var output = string.Empty;

            var url = RouteBuilder.BuildGetIndividualScoreRoute(_levelID, _playerID);
            var body = new StringContent(output, Encoding.UTF8, "application/json");

            var response = await Sender.SendHttpsRequest(HttpMethod.Get, url, APIData.User, APIData.Password, body);

            var playerScore = JsonUtility.FromJson<GetScoreRequest>(response.content);
            if (!response.isSuccess || playerScore.message == "error")
            {
                _callback(false, new GetScoreRequest());
                return;
            }

            _callback(true, playerScore);
        }

        public static async Task GetTopScores(string _levelID, Action<bool, GetHighestScoresRequest> _callback)
        {
            await GetHighestScores(_levelID, _callback);
        }

        private static async Task GetHighestScores(string _levelID, Action<bool, GetHighestScoresRequest> _callback)
        {
            var output = string.Empty;

            var url = RouteBuilder.BuildGetHighestScoresRoute(_levelID);
            var body = new StringContent(output, Encoding.UTF8, "application/json");

            var response = await Sender.SendHttpsRequest(HttpMethod.Get, url, APIData.User, APIData.Password, body);

            // var highestScores = SimpleJson.SimpleJson.DeserializeObject<GetHighestScoresRequest>(response.Content);
            var highestScores = JsonUtility.FromJson<GetHighestScoresRequest>(response.content);
            if (!response.isSuccess)
            {
                _callback(false, new GetHighestScoresRequest());
                return;
            }            

            _callback(true, highestScores);
        }
    }    
}
