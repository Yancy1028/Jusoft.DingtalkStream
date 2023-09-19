using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Internals
{
    internal class GetGatewayEndpointResponse
    {
        public string EndPoint { get; set; }

        public string Ticket { get; set; }

        public Uri ToUri()
        {
            return new Uri($"{EndPoint}?ticket={Ticket}");
        }
    }
}
