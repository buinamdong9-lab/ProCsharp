using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class RecycleBinService : IRecycleBinService
    {
        private readonly IRecycleBinRepository _recycleBinRepository;

        public RecycleBinService(IRecycleBinRepository recycleBinRepository)
        {
            _recycleBinRepository = recycleBinRepository;
        }

        public DataTable Load(string itemType, string keyword) =>
            _recycleBinRepository.Load(itemType, keyword);

        public void Restore(string itemType, int id) =>
            _recycleBinRepository.Restore(itemType, id);

        public void DeleteForever(string itemType, int id) =>
            _recycleBinRepository.DeleteForever(itemType, id);
    }
}
