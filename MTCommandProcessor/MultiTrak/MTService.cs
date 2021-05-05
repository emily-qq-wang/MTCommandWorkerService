// <copyright file="MTService.cs" company="Sentinel Offender Services LLC">
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
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DNAMTService" />.
    /// </summary>
    public class MTService : IMTService
    {
        /// <summary>
        /// Defines the baseURL.
        /// </summary>
        private string baseURL;

        /// <summary>
        /// Defines the ErrorMessage.
        /// </summary>
        private string ErrorMessage = "Function:{0}, URL:{1}";

        /// <summary>
        /// Defines the LogCallMessage.
        /// </summary>
        private string LogCallMessage = "MTServiceCall:::{0}:: {1} :: {2} :: {3} :: {4}";// [Function-Request Type-Request URL-success/failure-Message-POST Data]

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<MTService> logger;

        /// <summary>
        /// Defines the configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Defines the httpClientFactory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Defines the mtServiceAccess.
        /// </summary>
        private readonly IServiceAccess mtServiceAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="MTService"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{MTService}"/>.</param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        /// <param name="httpClientFactory">The httpClientFactory<see cref="IHttpClientFactory"/>.</param>
        /// <param name="mtServiceAccess">The mtServiceAccess<see cref="IServiceAccess"/>.</param>
        public MTService(ILogger<MTService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory, IServiceAccess mtServiceAccess)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.mtServiceAccess = mtServiceAccess;

            baseURL = configuration.GetSection("mtservice")["mtservice-url"];
        }

        /// <summary>
        /// The SendMTCommandAsync.
        /// </summary>
        /// <param name="serialNumber">The serialNumber<see cref="string"/>.</param>
        /// <param name="command">The command<see cref="MTCommand"/>.</param>
        /// <returns>The <see cref="Task{MTServiceResponse}"/>.</returns>
        public async Task<MTServiceResponse> SendMTCommandAsync(string serialNumber, MTCommand command)
        {
            MTServiceResponse serviceResponse = new MTServiceResponse();
            string errorMsg = string.Empty;

            string url = string.Format("{0}{1}/commands/{2}", baseURL, serialNumber, command.Code);
            try
            {
                // This is for MT Command Processor to authenticate with MT Service
                string bearerToken = await mtServiceAccess.GetAuthorizationTokenAsync();


                using (var httpClient = httpClientFactory.CreateClient())
                {
                    httpClient.BaseAddress = new Uri(baseURL);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/multitrak/{serialNumber}/commands/{command.Code}");
                    request.Content = new StringContent(command.Message, Encoding.UTF8, "application/json"); //CONTENT-TYPE header
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    var httpResponse = await httpClient.SendAsync(request);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var respData = await httpResponse.Content.ReadAsStringAsync();
                        serviceResponse = JsonConvert.DeserializeObject<MTServiceResponse>(respData);
                        serviceResponse.Success = true;
                    }
                    else
                    {
                        var respData = await httpResponse.Content.ReadAsStringAsync();
                        serviceResponse = JsonConvert.DeserializeObject<MTServiceResponse>(respData);
                        serviceResponse.Success = false;
                    }

                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                errorMsg = ex.Message;
                logger.LogError(string.Format(ErrorMessage, "MTService-SendMTCommand", url), ex);
            }

            logger.LogInformation(string.Format(LogCallMessage, "SendMTCommand", "POST", url, serviceResponse.Success, command.Message));
            return serviceResponse;
        }
    }
}
