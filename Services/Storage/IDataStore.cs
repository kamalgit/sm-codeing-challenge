using System.Threading.Tasks;

namespace sm_coding_challenge.Services.DataSource
{
    public interface IDataStore
    {
        Task<string> GetDataAsync(bool forceInvalidate = false);
        void Connect();
    }
}
