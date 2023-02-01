using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entidades
{
    public class Jwt
    {
        public object actor { get; set; }
        public List<object> audiences { get; set; }
        public List<ClaimResponse> claims { get; set; }
        public string encodedHeader { get; set; }
        public string encodedPayload { get; set; }
        public Header header { get; set; }
        public object id { get; set; }
        public object issuer { get; set; }
        public Payload payload { get; set; }
        public object innerToken { get; set; }
        public object rawAuthenticationTag { get; set; }
        public object rawCiphertext { get; set; }
        public string rawData { get; set; }
        public object rawEncryptedKey { get; set; }
        public object rawInitializationVector { get; set; }
        public string rawHeader { get; set; }
        public string rawPayload { get; set; }
        public string rawSignature { get; set; }
        public object securityKey { get; set; }
        public string signatureAlgorithm { get; set; }
        public object signingCredentials { get; set; }
        public object encryptingCredentials { get; set; }
        public object signingKey { get; set; }
        public object subject { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
    }
        
    public class Header
    {
        public string alg { get; set; }
        public string typ { get; set; }
    }

    public class Payload
    {
        public string unique_name { get; set; }
        public string role { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
    }

    public class ClaimResponse
    {
        public string issuer { get; set; }
        public string originalIssuer { get; set; }
        public Properties properties { get; set; }
        public object subject { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string valueType { get; set; }
    }

    public class Properties
    {
    }    
}
