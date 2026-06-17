using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class RecycleBinService(IRecycleBinRepository recycleBinRepository) : IRecycleBinService
    {
        public List<RecycleBinItemModel> Load(string itemType, string keyword) =>
            recycleBinRepository.Load(itemType, keyword);

        public void Restore(string itemType, int id) =>
            recycleBinRepository.Restore(itemType, id);

        public void DeleteForever(string itemType, int id) =>
            recycleBinRepository.DeleteForever(itemType, id);
    }
}
