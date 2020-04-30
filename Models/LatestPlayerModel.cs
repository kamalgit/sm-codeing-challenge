using System.Collections.Generic;
using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    public class LatestPlayersModel
    {
        public List<RushingPlayerModel> Rushing { get; set; }
        public List<PassingPlayerModel> Passing { get; set; }
        public List<ReceivingPlayerModel> Receiving { get; set; }
        public List<KickingPlayerModel> Kicking { get; set; }
    }
}
