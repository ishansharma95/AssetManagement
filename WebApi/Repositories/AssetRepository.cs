using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Interface;
using WebApi.Models;


namespace WebApi.Repositories
{
    public class AssetRepository : Imaintanable<Asset>
    {
        
        public int Create(Asset obj)
        {
            using (var context = new ApplicationDbContext())
            {
                obj.CreatedAt = DateTime.Now;
                context.Asset.Add(obj);
                var assetId = context.SaveChanges();
                return assetId;
            }
        }

        public void Delete(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var asset = context.Asset.Where(c => c.AssetId == id).FirstOrDefault();
                if (asset != null)
                {
                    context.Asset.Remove(asset);
                    context.SaveChanges();
                }
            }
        }

        public Asset Get(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var asset = context.Asset.Where(c => c.AssetId == id).FirstOrDefault();
                return asset;
            }
        }

        public List<Asset> GetAll()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Asset.ToList();
            }
        }

        public string Update(Asset obj)
        {
            using (var context = new ApplicationDbContext())
            {
                var uni = context.Asset.Where(c => c.AssetId == obj.AssetId).FirstOrDefault();
                context.Entry(uni).CurrentValues.SetValues(obj);
                context.SaveChanges();
                return "Asset updated succesfully";
            }
        }

    }
}