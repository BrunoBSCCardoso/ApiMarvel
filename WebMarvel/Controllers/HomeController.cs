using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Interface;
using WebMarvel.Models;
using WebMarvel.Services;

namespace WebMarvel.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceMarvel _serviceMarvel;

        public HomeController(IConfiguration configuration, IServiceMarvel serviceMarvel)
        {
            _configuration = configuration;
            _serviceMarvel = serviceMarvel;
        }

        public IActionResult Index()
        {
            List<Heroes> heroes = new List<Heroes>();
            try
            {
                heroes = ConvertTo.ConvertDynamicToHeroesObject(_serviceMarvel.GetAllCharacters().Result);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(heroes);
        }

        [HttpPost]
        public IActionResult GetHeroesByLetter(string letter)
        {
            List<Heroes> heroes = new List<Heroes>();

            try
            {
                if (!String.IsNullOrEmpty(letter))
                {
                    heroes = ConvertTo.ConvertDynamicToHeroesObject(_serviceMarvel.GetCharacterByName(letter).Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("HeroesList",heroes);

        }

        [HttpPost]
        public IActionResult GetHeroesByName(string name)
        {
            List<Heroes> heroes = new List<Heroes>();

            try
            {
                if (!String.IsNullOrEmpty(name))
                {
                    heroes = ConvertTo.ConvertDynamicToHeroesObject(_serviceMarvel.GetCharacterByName(name).Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("HeroesList", heroes);

        }

    }
}
