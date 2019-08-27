using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Interface;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class SubAssetRepository : Imaintanable<SubAsset>
    {
        
        public int Create(SubAsset obj)
        {
            using (var context = new ApplicationDbContext())
            {
                context.SubAsset.Add(obj);
                var subAssetId = context.SaveChanges();
                return subAssetId;
            }
            
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public SubAsset Get(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var subAsset = context.SubAsset.Where(c => c.SubAssetId == id).FirstOrDefault();
                return subAsset;
            }
        }

        public List<SubAsset> GetAll()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.SubAsset.ToList();
            }
        }

        public string Update(SubAsset obj)
        {
            using (var context = new ApplicationDbContext())
            {
                var uni = context.SubAsset.Where(c => c.SubAssetId == obj.SubAssetId).FirstOrDefault();
                context.Entry(uni).CurrentValues.SetValues(obj);
                context.SaveChanges();
                return "Asset updated succesfully";
            }
        }
    }
}