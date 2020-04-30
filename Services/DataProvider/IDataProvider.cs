using sm_coding_challenge.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataProvider
{
    public interface IDataProvider
    {
        Task<PlayerModel> GetPlayerById(string id);
        Task<LatestPlayersModel> GetLatestPlayers(IEnumerable<string> ids);
    }
}
