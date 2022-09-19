using Linkedin_bot.models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Linkedin_bot.controllers
{
    public class CreateConnections
    {
        public static async Task<bool> CreateAConnections(Page page, accounts account, List<clients> clients,messages message)
        {
            await Task.Delay(1000);
            return true;
        }
    }
}
