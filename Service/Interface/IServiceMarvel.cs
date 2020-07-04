using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IServiceMarvel
    {
        Task<dynamic> GetAllCharacters();

        Task<dynamic> GetCharacterByName(string name);
    }
}
