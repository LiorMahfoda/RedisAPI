using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisAPI.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using RedisAPI.Services;
using ServiceStack.Redis;
using ServiceStack.Messaging.Redis;
using System.Text;
using System.Threading;

namespace RedisAPI.Models
{
    public class Publisher
    {
        private readonly IDistributedCache _Cache;
        ulong num = 1L;

        public Publisher(IDistributedCache distributedCache) =>_Cache = distributedCache;   
       
        public async Task<List<User>> Publish(List<User> users)
        {
            //bool result = false;
            //try
            //{
                if (users.Count > 0) //user != null
                {
                    await CacheService.SetObjectAsync(_Cache, Sequence(num), users);
                    num += 1; // next sequence

                    //result = true;
                }
                else
                    return users;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            return users; //result;

        }

        string Sequence(ulong num)
        {
            return ToBase36(num);
        }
        private string ToBase36(ulong value)
        {
            const string base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var sb = new StringBuilder(9);
            do
            {
                sb.Insert(0, base36[(byte)(value % 36)]);
                value /= 36;
            } while (value != 0);

            var paddedString = "ABC" + sb.ToString().PadLeft(6, '0');
            return paddedString;
        }
    }

    

   
}
