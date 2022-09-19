using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Linkedin.Net.Models;
using Linkedin.Net.Db;
using Linkedin_Insert_Data.Models;
namespace Linkedin_Insert_Data
{
    class Program
    {
        private static database database;
        static void Main( string[] args )
        {
           

           
            database = new database();
            Console.WriteLine("-> Ingrese la dir de las cuentas : ");
            AccountToModel.Insert();
            Console.WriteLine("-> Ingrese la dir de las proxys : ");
            ProxyToModel.Insert();
            Console.WriteLine("-> Ingrese la dir de las mensajes : ");
            MessageToModel.Insert();
            ProxyToModel.ReAsignacionProxys();
            //ClientToModel./*AutoComplete*/();
            ////MessageToModel.Insert();
            ////ClientToModel.Insert();
            //Console.WriteLine("-> Ingrese la dir de las clientes : ");
            //ClientToModel.Insert();
        }
    }
}
