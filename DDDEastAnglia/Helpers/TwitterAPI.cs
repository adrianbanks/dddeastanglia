using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using DDDEastAnglia.Areas.Admin.Models.Twitter;
using Newtonsoft.Json;

namespace DDDEastAnglia.Helpers
{
    // https://dev.twitter.com/docs/auth/application-only-auth
    public class TwitterAPI
    {
        public IList<Profile> GetProfiles(List<string> twitterHandles)
        {
            string oAuthConsumerKey = ConfigurationManager.AppSettings["TwitterOAuthConsumerKey"];
            string oAuthConsumerSecret = ConfigurationManager.AppSettings["TwitterOAuthConsumerSecret"];
            string oAuthUrl = ConfigurationManager.AppSettings["TwitterOAuthUrl"];

            // Do the Authenticate
            var authHeader = string.Format("Basic {0}",
                                           Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                                                                                         Uri.EscapeDataString((oAuthConsumerSecret)))));
            HttpWebRequest authorizationRequest = (HttpWebRequest) WebRequest.Create(oAuthUrl);
            authorizationRequest.Headers.Add("Accept-Encoding", "gzip");
            authorizationRequest.Headers.Add("Authorization", authHeader);
            authorizationRequest.Method = "POST";
            authorizationRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authorizationRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var stream = authorizationRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes("grant_type=client_credentials");
                stream.Write(content, 0, content.Length);
            }

            AuthorizationRepsonse authorizationRepsonse;

            using (WebResponse authResponse = authorizationRequest.GetResponse())
            {
                using (var reader = new StreamReader(authResponse.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    authorizationRepsonse = JsonConvert.DeserializeObject<AuthorizationRepsonse>(responseText);
                }
            }

            List<Profile> profiles = new List<Profile>();

            var chunkedTwitterHandles = twitterHandles.Chunk(100);

            foreach (var chunk in chunkedTwitterHandles)
            {
                // profile
                var profileUrlFormat = "https://api.twitter.com/1.1/users/lookup.json?screen_name={0}";
                var screenNames = string.Join(",", chunk);
                var profileUrl = string.Format(profileUrlFormat, screenNames);

                HttpWebRequest timeLineRequest = (HttpWebRequest) WebRequest.Create(profileUrl);
                timeLineRequest.Headers.Add("Authorization", string.Format("{0} {1}", authorizationRepsonse.token_type, authorizationRepsonse.access_token));
                timeLineRequest.Method = "Get";


                using (WebResponse timeLineResponse = timeLineRequest.GetResponse())
                {
                    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                    {
                        string profileText = reader.ReadToEnd();
                        var userProfiles = JsonConvert.DeserializeObject<List<Profile>>(profileText);
                        profiles.AddRange(userProfiles);
                    }
                }
            }

            return profiles;
        }
    }
}