using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBankingApp.Models
{
    public class ApprovedBanks
    {
        public long BankId { get; set; }
        public string Name { get; set; }
        public long Asset { get; set; }
        public long Limit { get; set; }
        public long Rating { get; set; }
        public string Date { get; set; }
    }
}