using System;
using MongoDB.Bson;
using Server.models;
using System.Collections.Generic;
using Server.database.seeders;
using Server.exceptions;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * NOTE: I will have to rewrite most of the CRUD systems and refacter the models a bit,
             * so just ignore the "baka" code parts like that useless inheritance from MongoCrud. Sheers
             */

            // Will only seed empty collections in the Zephy database
            Seeder s = new Seeder();
            s.Start(entrees:10);

            try
            {
                EmployeeCrud db = new EmployeeCrud("Zephy");

                // This will get a list of all employees in the system
                List<EmployeeModel> employees = db.LoadEmployees<EmployeeModel>();

                // This will load 1 specific employee and replace the roles reference ids with the actual data (population)
                EmployeePopulated populatedEmployee = db.LoadPopulatedEmployeeById<EmployeePopulated>(new ObjectId("5ead3320dde2a721846c4104"));
            }
            catch (InvalidIDException e) {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // Breakpoint here to check data
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }
    } 
}