using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedisAPI.Services;
using RedisAPI.Models;
using System.Threading;
using ServiceStack.Redis;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;

namespace RedisAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class RedisController : Controller
    {
        private readonly IDistributedCache _Cache;

        public RedisController(IDistributedCache distributedCache) => _Cache = distributedCache;
        
        // GET api/values
        [HttpGet("api/{name}")]
        public async Task<List<User>> GetMessage(List<User> name)
        {
            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);
            mqHost.RegisterHandler<Request>(m =>
                new Response { Result = $"Hello, {m.GetBody().Name}!" });
            mqHost.Start();
            var redis = RedisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("Redis",( channel, message)  =>  { 
                Subscriber subber = new Subscriber(_Cache);
                subber.Subscribe(name);
            });
            
            return name;
        }

        // POST api/values
        [HttpPost("api")]
        public async void PostMessage([FromBody]List<User> users)
        {
            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqServer = new RedisMqServer(redisFactory, retryCount: 2);
            var mqClient = mqServer.CreateMessageQueueClient();
            
            mqServer.Start(); //Starts listening for messages
           
            Publisher pub = new Publisher(_Cache);
            while (true)
            {
                await pub.Publish(users);
                mqClient.Publish(users);
                Thread.Sleep(3);
            }
        }

        public List<User> Index() // IActionResult
        {
            User user1 = new User();
            User user2 = new User("GHI");
            User user3 = new User("KLM", "11/3/2004", 26, "ADV");
            List<User> users = new List<User> { user1, user2, user3 };
            
            GetMessage(users);
            //PostMessage(users);

            /*await PostUser(user2);
            await PostUser(user3);
            await GetUser("GHI"); // User
            await GetUser("are"); //null
            */
           
            return users; 
        }
    }
}