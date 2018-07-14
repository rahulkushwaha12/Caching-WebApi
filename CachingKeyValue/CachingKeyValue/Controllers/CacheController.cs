using CachingKeyValue.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using CachingKeyValue.Entities;
using System.Web.Http.Results;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace CachingKeyValue.Controllers
{
    public class CacheController : ApiController
    {
        private readonly IKeyValueCache _keyValueCache;

        public CacheController()
        {
            _keyValueCache = new KeyValueCache();
        }

        [HttpGet, Route("api/fetch/")]
        public IHttpActionResult Get([FromUri]Input value)
        {
            JObject result = null;
            try
            {
                if (value == null || value.key == null)
                {
                    return BadRequest();
                }
                else
                {
                    var obj = _keyValueCache.GetValue(value.key);
                    if(obj!=null)
                    {
                        result = new JObject(
                                new JProperty("success", true),
                                new JProperty("value", obj.ToString())
                            );
                    }
                    else
                    {
                        result = new JObject(
                                new JProperty("success", false),
                                new JProperty("value", null)
                            );
                    }
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CacheController.Get " + ex);
                return InternalServerError();
            }
        }

        [HttpPost, Route("api/store/")]
        public IHttpActionResult Post([FromBody]Input value)
        {
            double expiryTime;
            try
            {
                if(value == null)
                {
                    return BadRequest();
                }
                else
                {
                    if(value.expiryTime != null)
                    {
                        double.TryParse(value.expiryTime.ToString(), out expiryTime);
                        if (_keyValueCache.Add(value.key, value.value, expiryTime))
                        {
                            long expiresAt = DateTimeOffset.Now.AddSeconds(expiryTime).ToUnixTimeSeconds();
                            return Ok(new JObject(
                                new JProperty("expiresAt", expiresAt)
                            ));
                        }
                        return InternalServerError();
                    }
                    else
                    {
                        return Ok(new JObject(
                                new JProperty("expiresAt", null)
                            ));
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CacheController.Post " + ex);
                return InternalServerError();
            }
        }

        [HttpDelete, Route("api/delete/")]
        public IHttpActionResult Delete([FromBody]Input value)
        {
            try
            {
                if (value == null || value.key == null)
                {
                    return BadRequest();
                }
                else
                {
                    if(_keyValueCache.Delete(value.key))
                    {
                        return Ok(new JObject(
                                new JProperty("success", true)
                            ));
                    }
                    else
                    {
                        return Ok(new JObject(
                               new JProperty("success", false)
                           ));
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CacheController.Delete " + ex);
                return InternalServerError();
            }
        }
    }

}