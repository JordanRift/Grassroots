//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

namespace JordanRift.Grassroots.Web.Models
{
    public class CauseTemplateDetailsModel
    {
        public int CauseTemplateID { get; set; }
        public string Name { get; set; }
        public string ActionVerb { get; set; }
        public string GoalName { get; set; }
        public bool Active { get; set; }
        public bool AmountIsConfigurable { get; set; }
        public decimal DefaultAmount { get; set; }
        public bool TimespanIsConfigurable { get; set; }
        public int DefaultTimespanInDays { get; set; }
        public string Summary { get; set; }
        public string VideoEmbedHtml { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImagePath { get; set; }
    }
}