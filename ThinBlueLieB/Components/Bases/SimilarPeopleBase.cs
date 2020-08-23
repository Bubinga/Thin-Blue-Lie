using DataAccessLibrary.DataAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;


namespace ThinBlueLieB.Components.Bases
{
    public class SimilarPeopleBase : ComponentBase
    {
        private readonly IConfiguration _config;

        public SimilarPeopleBase(IConfiguration configuration)
        {
            _config = configuration;
        }
        DataAccess data = new DataAccess();
        string sql = "SELECT * FROM officers";
        //List<Person> People = await data.LoadData<Person, dynamic>(sql, new { }, _config.GetConnectionString("DataDB"));
       
    }
}
