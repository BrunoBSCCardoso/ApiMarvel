using System.Collections.Generic;
using WebMarvel.Models;

namespace WebMarvel.Services
{
    public static class ConvertTo
    {
        public static List<Heroes> ConvertDynamicToHeroesObject(dynamic heroesCollection)
        {
            List<Heroes> heroes = new List<Heroes>();

            foreach (var hero in heroesCollection.data.results)
            {
                heroes.Add(new Heroes 
                {
                    Name = hero.name,
                    Description = hero.description,
                    UrlImage = hero.thumbnail.path + "." +
                    hero.thumbnail.extension,
                    UrlWiki = hero.urls[1].url
                });
            }

            return heroes;
        }

        public static List<string> ConvertDynamicToHeroesNameObject(dynamic heroesCollection)
        {
            List<string> nameHeroes = new List<string>();

            foreach (var hero in heroesCollection.data.results)
            {
                nameHeroes.Add(hero.name);
            }

            return nameHeroes;
        }

    }
}
