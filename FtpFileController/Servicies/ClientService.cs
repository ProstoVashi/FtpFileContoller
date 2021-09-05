using System;
using System.Net;
using FtpFileController.Configs;

namespace FtpFileController.Servicies {
    public class ClientService {
        private readonly string _baseUri;
        private readonly ICredentials _credentials;
        
        public ClientService(ConnectionSettings connectionSettings, UserCredentials credentialSettings) {
            _baseUri = connectionSettings.Uri;
            _credentials = new NetworkCredential(credentialSettings.UserName, credentialSettings.Password);
        }

        internal FtpWebRequest GetRequest(string method, string externalUri = null) {
            var uri = string.IsNullOrEmpty(externalUri)
                ? new Uri(_baseUri)
                : new Uri($"{_baseUri}/{externalUri}");
            
            var request = (FtpWebRequest) WebRequest.Create(uri);
            request.Credentials = _credentials;
            request.Method = method;

            return request;
        }
    }
}