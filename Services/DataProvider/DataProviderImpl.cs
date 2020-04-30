using Newtonsoft.Json;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataProvider
{
    public class DataProviderImpl : IDataProvider
    {
        private IDataStore _dataStore;

        public DataProviderImpl(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<LatestPlayersModel> GetLatestPlayers(IEnumerable<string> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            // I am not sure of my understanding of what is needed for GetLatestPlayers. I wanted to confirm my understanding but didn't get a reply on time.
            // My assumption here is that this endpoint will be used to refresh the cache and to return the data in a special format.

            // forceInvalidate is set to True here, to force fetching the latest data from the external source.
            var stringData = await _dataStore.GetDataAsync(forceInvalidate: true);

            var dataResponse = DeserializeRespons(stringData);
            return GetLatestPlayer(ids, dataResponse);
        }

        public async Task<PlayerModel> GetPlayerById(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var stringData = await _dataStore.GetDataAsync();
            var dataResponse = JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return GetPlayer(id, dataResponse);
        }

        private LatestPlayersModel GetLatestPlayer(IEnumerable<string> ids, DataResponseModel dataResponse)
        {
            var rushing = new List<RushingPlayerModel>();
            var passing = new List<PassingPlayerModel>();
            var receiving = new List<ReceivingPlayerModel>();
            var kicking = new List<KickingPlayerModel>();
            foreach (var id in ids)
            {
                AppendPlayerData(id, dataResponse.Rushing, rushing);
                AppendPlayerData(id, dataResponse.Passing, passing);
                AppendPlayerData(id, dataResponse.Receiving, receiving);
                AppendPlayerData(id, dataResponse.Kicking, kicking);
            };

            return new LatestPlayersModel()
            {
                Rushing = NullIfEmpty(rushing),
                Passing = NullIfEmpty(passing),
                Receiving = NullIfEmpty(receiving),
                Kicking = NullIfEmpty(kicking)
            };
        }

        private PlayerModel GetPlayer(string id, DataResponseModel dataResponse)
        {
            var player = GetFirstPlayer(id, dataResponse.Rushing);
            if (player != null)
            {
                return player;
            }

            player = GetFirstPlayer(id, dataResponse.Passing);
            if (player != null)
            {
                return player;
            }

            player = GetFirstPlayer(id, dataResponse.Receiving);
            if (player != null)
            {
                return player;
            }

            return GetFirstPlayer(id, dataResponse.Kicking);
        }

        private PlayerModel GetFirstPlayer<T>(string id, IList<T> playerList)  where T : PlayerModel
        {
            return playerList?.FirstOrDefault(x => string.Compare(x.Id, id, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private void AppendPlayerData<T>(string id, IList<T> playerList, List<T> target) where T : PlayerModel
        {
            var lst = playerList?.Where(x => string.Compare(x.Id, id, StringComparison.OrdinalIgnoreCase) == 0).ToList();
            if (lst.Any())
            {
                target.AddRange(lst);
            }
        }

        private List<T> NullIfEmpty<T>(List<T> lst) where T : PlayerModel
        {
            return lst.Any() ? lst : null;
        }

        private static DataResponseModel DeserializeRespons(string stringData)
        {
            return JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
