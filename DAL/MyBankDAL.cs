using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyBankingApp.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyBankingApp.DAL
{
    public class MyBankDAL
    {
        public static List<ApprovedBanks> GetBanks()
        {
           List<ApprovedBanks> banks = new List<ApprovedBanks>();

            DataTable dt = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyBank"].ConnectionString))
            using (var cmd = new SqlCommand("[MyBank].[dbo].[GetBanks]", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }

            foreach (DataRow row in dt.Rows)
            {
                ApprovedBanks bank = new ApprovedBanks();

                bank.BankId = Convert.ToInt64(row["BankId"]);
                bank.Name = row["Name"].ToString();
                bank.Rating = Convert.ToInt64(row["Rating"]);
                bank.Asset = Convert.ToInt64(row["Asset"]);
                bank.Limit = getLimit(bank);
                bank.Date = DateTime.Now.ToString();

                banks.Add(bank);
            }

            return banks;
        }


        // limits are calculated based on ratings/assets
        private static long getLimit(ApprovedBanks bank)
        {
            long limit = 2000000;
            double difference = 0;

            // limit increase/decreate depending on rating
            switch (bank.Rating)
            {
                case -5:
                case -4:
                case -3:
                    difference = limit * 0.12;
                    limit = limit - (long)Math.Round(difference);
                    break;

                case -2:
                case -1:
                case  0:
                    difference = limit * 0.09;
                    limit = limit - (long)Math.Round(difference);
                    break;

                case 1:
                case 2:
                case 3:
                    difference = limit * 0.05;
                    limit = limit + (long)Math.Round(difference);
                    break;

                case 4:
                case 5:
                case 6:
                    difference = limit * 0.08;
                    limit = limit + (long)Math.Round(difference);
                    break;

                case 7:
                case 8:
                case 9:
                case 10:
                    difference = limit * 0.13;
                    limit = limit + (long)Math.Round(difference);
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }


            // if bank's total assets exceeds 3M then increase limit by 23%
            if (bank.Asset > 3000000)
            {
                difference = limit * 0.23;
                limit = limit + (long)Math.Round(difference);
            }


            return limit;
        }
    }
}