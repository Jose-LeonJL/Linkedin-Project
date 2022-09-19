using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace Linkedin_Insert_Data
{
    public static class creardatabase
    {
        public static void crear()
        {
            Database database = Database.getdatabase();
            using (var db = database.connection)
            {
                Console.WriteLine("<-------------------------->");
                Console.WriteLine("-> Creando base de datos...");
                var sql = "CREATE DATABASE IF NOT EXISTS linkedinbot CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                db.Execute(sql);
                Console.WriteLine("-> Base de datos creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla Usuarios...");
                sql = "CREATE TABLE if NOT EXISTS users(user_id INT AUTO_INCREMENT PRIMARY KEY,user_name NVARCHAR(15) UNIQUE NOT NULL,user_pass NVARCHAR(250)  NOT NULL);";
                db.Execute(sql);
                Console.WriteLine("-> Tabla Usuarios creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla proxys...");
                sql = "CREATE Table if NOT EXISTS proxys(proxy_id INT AUTO_INCREMENT PRIMARY KEY,proxy_ip NVARCHAR(15) UNIQUE NOT NULL,proxy_port NVARCHAR(6) NOT NULL,proxy_user NVARCHAR(30) NOT NULL,proxy_pass NVARCHAR(30) NOT NULL);";
                db.Execute(sql);
                Console.WriteLine("-> Tabla Proxys creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla proxys...");
                sql = "CREATE Table if NOT EXISTS accounts(account_id INT AUTO_INCREMENT PRIMARY KEY,account_mail_server NVARCHAR(50) NOT NULL,account_name NVARCHAR(20) NOT NULL,account_mail NVARCHAR(30) UNIQUE NOT NULL,account_pass NVARCHAR(50) NOT NULL,account_linkedin_pass NVARCHAR(50) NOT NULL,account_recovery_mail NVARCHAR(50) NULL,account_recovery_pass NVARCHAR(50) NULL,account_mobile_number NVARCHAR(14) NULL,account_profile_url NVARCHAR(250) NOT NULL,account_banned BIT DEFAULT 0 NOT NULL,account_salenav BIT DEFAULT 0 NOT NULL,account_selected BIT DEFAULT 0 NOT NULL,account_cookie VARCHAR(5000),create_at INT NOT NULL); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla accounts creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla accounts_proxys...");
                sql = "CREATE Table if NOT EXISTS accounts_proxys(account_proxy_id INT AUTO_INCREMENT PRIMARY KEY,proxy_id INT UNIQUE NOT NULL,account_id INT UNIQUE NOT NULL,FOREIGN KEY(proxy_id) REFERENCES proxys(proxy_id),FOREIGN KEY(account_id) REFERENCES accounts(account_id));";
                db.Execute(sql);
                Console.WriteLine("-> Tabla accounts_proxys creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla clients...");
                sql = "CREATE TABLE if NOT EXISTS clients(client_id INT AUTO_INCREMENT PRIMARY KEY,client_name VARCHAR(100) NOT NULL,client_email NVARCHAR(100) ,client_title NVARCHAR(250) NOT NULL,client_url NVARCHAR(250) UNIQUE NOT NULL,client_location NVARCHAR(100),client_company_name NVARCHAR(100),client_company_website NVARCHAR(250),client_company_industry NVARCHAR(250),client_company_linkedin_url NVARCHAR(250),client_description NVARCHAR(250) NOT NULL,create_at INT NOT NULL) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci; ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla clients creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla client_connections...");
                sql = "CREATE Table if NOT EXISTS client_connections(client_connection_id INT AUTO_INCREMENT PRIMARY KEY,client_id INT UNIQUE NOT NULL,account_id INT NOT NULL,fecha_inicio INT NOT NULL,fecha_finalizacion INT NOT NULL,respuesta BIT NOT NULL DEFAULT 0,FOREIGN KEY(client_id) REFERENCES clients(client_id),FOREIGN KEY(account_id) REFERENCES accounts(account_id)); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla client_connections creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla works...");
                sql = "CREATE Table if NOT EXISTS works(work_id INT AUTO_INCREMENT PRIMARY KEY,work_priority INT(1) NOT NULL,work_name NVARCHAR(10) NOT NULL,work_description VARCHAR(250) NOT NULL); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla works creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla workflows...");
                sql = "CREATE TABLE if NOT EXISTS workflows(workflow_id INT AUTO_INCREMENT PRIMARY KEY,work_id INT NOT NULL,account_id INT NOT NULL,starrted BIT NOT NULL DEFAULT 0,finished BIT NOT NULL DEFAULT 0,fecha INT NOT NULL,FOREIGN KEY(work_id) REFERENCES works(work_id),FOREIGN KEY(account_id) REFERENCES accounts(account_id)); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla workflows creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla message_types...");
                sql = "CREATE TABLE if NOT EXISTS message_types(message_type_id INT AUTO_INCREMENT PRIMARY KEY,message_description VARCHAR(30) NOT NULL);";
                db.Execute(sql);
                Console.WriteLine("-> Tabla message_types creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tipos de mensajes...");
                sql = "INSERT INTO message_types(message_type_id,message_description) VALUES (DEFAULT,'initial');";
                db.Execute(sql);
                sql = "INSERT INTO message_types(message_type_id, message_description) VALUES(DEFAULT, 'secondary'); ";
                db.Execute(sql);
                sql = "INSERT INTO message_types(message_type_id,message_description) VALUES (DEFAULT,'tertiary');";
                db.Execute(sql);
                Console.WriteLine("-> tipos de mensajes creados!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla messages...");
                sql = "CREATE TABLE if NOT EXISTS messages(message_id INT AUTO_INCREMENT PRIMARY KEY,message_type_id INT NOT NULL,message TEXT NOT NULL,FOREIGN KEY(message_type_id) REFERENCES message_types(message_type_id)); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla messages creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla message_accounts...");
                sql = "CREATE TABLE if NOT EXISTS message_accounts(message_account_id INT  AUTO_INCREMENT PRIMARY KEY,account_id INT NOT NULL,message_id INT NOT NULL,FOREIGN KEY(account_id) REFERENCES accounts(account_id),FOREIGN KEY(message_id) REFERENCES messages(message_id)); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla message_accounts creada!!!");
                Console.WriteLine("<-------------------------->");

                Console.WriteLine("-> Creando tabla stacks...");
                sql = "CREATE TABLE if NOT EXISTS stacks(stack_id INT AUTO_INCREMENT PRIMARY KEY,invites INT NULL,connections INT NULL,replies INT NULL,messages_send INT NULL,fecha INT NOT NULL); ";
                db.Execute(sql);
                Console.WriteLine("-> Tabla stacks creada!!!");
                Console.WriteLine("<-------------------------->");
            }
        }
        public static void crearinit()
        {
            Database database = new Database("xd");
            using (var db = database.connection)
            {
                Console.WriteLine("<-------------------------->");
                Console.WriteLine("-> Creando base de datos...");
                var sql = "CREATE DATABASE IF NOT EXISTS linkedinbot CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                db.Execute(sql);
                Console.WriteLine("-> Base de datos creada!!!");
                Console.WriteLine("<-------------------------->");
            }
        }
    }
}
