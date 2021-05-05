// <copyright file="MTServiceAccess.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MTServiceAccess" />.
    /// </summary>
    public class MTServiceAccess : IServiceAccess
    {
        /// <summary>
        /// Defines the MT_SERVICE_SESSION_CACHE_TIME.
        /// </summary>
        private int MT_SERVICE_SESSION_CACHE_TIME = 57;

        /// <summary>
        /// Defines the baseURL.
        /// </summary>
        private string baseURL;

        /// <summary>
        /// Defines the mtServiceApplication.
        /// </summary>
        private string mtServiceApplication;

        /// <summary>
        /// Defines the mtServiceUsername.
        /// </summary>
        private string mtServiceUsername;

        /// <summary>
        /// Defines the mtServicePassword.
        /// </summary>
        private string mtServicePassword;

        /// <summary>
        /// Defines the sessionToken.
        /// </summary>
        private string sessionToken = string.Empty;

        /// <summary>
        /// Defines the sessionExpiryTime.
        /// </summary>
        private DateTime sessionExpiryTime = DateTime.MinValue;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<MTServiceAccess> logger;

        /// <summary>
        /// Defines the configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Defines the httpClientFactory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MTServiceAccess"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{MTServiceAccess}"/>.</param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        /// <param name="httpClientFactory">The httpClientFactory<see cref="IHttpClientFactory"/>.</param>
        public MTServiceAccess(ILogger<MTServiceAccess> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;

            try
            {
                MT_SERVICE_SESSION_CACHE_TIME = Convert.ToInt32(configuration.GetSection("mtservice")["mtservice-session-cache-time"]);
                mtServiceApplication = configuration.GetSection("mtservice")["mtservice-application"];
                mtServiceUsername = configuration.GetSection("mtservice")["mtservice-login"];
                mtServicePassword = configuration.GetSection("mtservice")["mtservice-access"];
                baseURL = configuration.GetSection("mtservice")["authapi"];
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// The GetAuthorizationToken.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public async Task<string> GetAuthorizationTokenAsync()
        {
            string token = GetTokenFromCache();

            if (string.IsNullOrEmpty(token))
            {
                return await AuthenticateServiceAsync();
            }

            return token;
        }

        /// <summary>
        /// The AuthenticateServiceAsync.
        /// </summary>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        private async Task<string> AuthenticateServiceAsync()
        {
            string token = string.Empty;
            string requestURL = string.Empty;
            string respData = string.Empty;
            try
            {


                using (var httpClient = httpClientFactory.CreateClient())
                {
                    httpClient.BaseAddress = new Uri(baseURL);
                    string body = GetAuthBody();

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/security/accounts/authenticate");
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json"); //CONTENT-TYPE header

                    logger.LogInformation($"POST: {baseURL}/security/accounts/authenticate");
                    logger.LogDebug($"Params: {body}");

                    var httpResponse = await httpClient.SendAsync(request);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        respData = await httpResponse.Content.ReadAsStringAsync();
                        AuthServiceResponse eresponse = JsonConvert.DeserializeObject<AuthServiceResponse>(respData);

                        token = eresponse.jwt;
                        SaveTokenForFutureAccess(token);
                    }
                    else 
                    {
                        logger.LogInformation($"Auth Failed: {baseURL}/accounts/authenticate {await httpResponse.Content.ReadAsStringAsync()}");
                    }

                }
            }
            catch (WebException ex)
            {
                logger.LogError("MTService Access : AuthenticateService", ex);
            }
            catch (Exception ex)
            {
                logger.LogError("MTService Access : AuthenticateService", ex);
            }

            return token;
        }

        /// <summary>
        /// The SaveTokenForFutureAccess.
        /// </summary>
        /// <param name="token">The token<see cref="string"/>.</param>
        private void SaveTokenForFutureAccess(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                sessionToken = token;
                sessionExpiryTime = DateTime.Now.AddMinutes(MT_SERVICE_SESSION_CACHE_TIME);
            }
        }

        /// <summary>
        /// The GetTokenFromCache.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetTokenFromCache()
        {
            if (DateTime.Now > sessionExpiryTime)
            {
                sessionToken = string.Empty;
            }

            return sessionToken;
        }

        /// <summary>
        /// The GetAuthBody.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetAuthBody()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"signInName\":\"" + mtServiceUsername + "\",");
            builder.Append("\"password\":\"" + mtServicePassword + "\"");
            //builder.Append("\"applicationId\":\"" + mtServiceApplication + "\"");
            builder.Append("}");
            return builder.ToString();
        }
    }
}
