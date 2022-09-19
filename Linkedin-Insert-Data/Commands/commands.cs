using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Linkedin_Insert_Data.Commands
{
    public class Comando
    {
        //variables
        private List<Comandos> _MainsCommands;

        //Propiedades
        public List<Comandos> CommandosPrincipales { get { return this._MainsCommands; } set { this._MainsCommands = value; } }

        //Constructor
        public Comando()
        {
            this._MainsCommands = new List<Comandos>();
        }

        //Metodos
        public void MainCommans(string[] args)
        {
            var comando = (from cms in _MainsCommands where args[0] == cms.comando select cms).FirstOrDefault();
            if (comando != null)
            {
                comando.Accion();
            }
            else
            {
                Console.WriteLine($"-> El comando {args[0]} no esta definido!!!");
            }
        }
        public void ExectCommands(string args)
        {

        }
    }
    public class Comandos
    {
        public string comando { get; set; }
        public Action Accion { get; set; }
    }
}
