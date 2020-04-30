using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class KickingPlayerModel : PlayerModel
    {
        [DataMember(Name = "fld_goals_made")]
        public string Fld_goals_made { get; set; }

        [DataMember(Name = "fld_goals_att")]
        public string Fld_goals_att { get; set; }
        
        [DataMember(Name = "extra_pt_made")]
        public string Extra_pt_made { get; set; }
        
        [DataMember(Name = "fextra_pt_attum")]
        public string Fextra_pt_attum { get; set; }
    }
}
