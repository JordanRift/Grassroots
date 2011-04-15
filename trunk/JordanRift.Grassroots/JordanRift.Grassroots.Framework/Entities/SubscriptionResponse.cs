//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;

namespace JordanRift.Grassroots.Framework.Entities
{
    public class SubscriptionResponse
    {
        public string ResponseCode { get; set; }
        public List<string> Messages { get; set; }
    }
}
