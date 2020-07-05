using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public HomeController(IConfiguration configuration, IServiceMarvel serviceMarvel, IMemoryCache cache)
        {
            _configuration = configuration;
            _serviceMarvel = serviceMarvel;
            _cache = cache;
        }

        public IActionResult Index()
        {
            List<Heroes> heroes = new List<Heroes>();
            string keyCache = "AllHeroes";

            try
            {
                if(!_cache.TryGetValue<List<Heroes>>(keyCache, out heroes)){
                    
                    heroes = ConvertTo.ConvertDynamicToHeroesObject(_serviceMarvel.GetAllCharacters().Result);
                    _cache.Set(keyCache, heroes, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
                }
                else
                {
                    heroes = _cache.Get<List<Heroes>>(keyCache);
                }
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
            string keyCache = "heroesWithLetter" + letter;

            try
            {
                if (!String.IsNullOrEmpty(letter))
                {
                    if(!_cache.TryGetValue<List<Heroes>>(keyCache, out heroes))
                    {
                        heroes = ConvertTo.ConvertDynamicToHeroesObject(_serviceMarvel.GetCharacterByName(letter).Result);
                        _cache.Set<List<Heroes>>(keyCache, heroes, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
                    }
                    else
                    {
                        heroes = _cache.Get<List<Heroes>>(keyCache);
                    }
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
            string keyCache = name;

            try
            {
                if (!String.IsNullOrEmpty(name))
                {
                    if(!_cache.TryGetValue(keyCache, out heroes))
                    {
                        heroes = ConvertTo.ConvertDynamicToHeroesObject(_serviceMarvel.GetCharacterByName(name).Result);
                        _cache.Set<List<Heroes>>(keyCache, heroes);
                    }
                    else
                    {
                        heroes = _cache.Get<List<Heroes>>(keyCache);
                    }
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
