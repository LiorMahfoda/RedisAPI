using Microsoft.Extensions.Caching.Distributed;
using NLog.Fluent;
using RedisAPI.Services;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Models
{
    public class Subscriber
    {
        private readonly IDistributedCache _Cache;
        

        public Subscriber(IDistributedCache distributedCache)
        {
            _Cache = distributedCache;
        }
        public async Task<List<User>> Subscribe(List<User> user)
        {
            //User user = new User();
            //try
            //{
                bool exists = await CacheService.ExistObjectAsync<User>(_Cache, user.ToString());
                if (!string.IsNullOrEmpty(user.ToString()) && !exists)
                {
                    
                    user = await CacheService.GetObjectAsync<List<User>>(_Cache, user.ToString());
                }
                else
                    return user;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return user;

        }
    }

}