using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gooios.OrderService.Applications.DTOs;
using Gooios.OrderService.Applications.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gooios.OrderService.Controllers
{
    [Route("api/[controller]/v1")]
    public class DeliveryAddressController : BaseApiController
    {
        readonly IDeliveryAddressAppService _deliveryAddressAppService;

        public DeliveryAddressController(IDeliveryAddressAppService deliveryAddressAppService)
        {
            _deliveryAddressAppService = deliveryAddressAppService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<DeliveryAddressDTO> Get()
        {
            return _deliveryAddressAppService.GetMyDeliveryAddresses(UserId);
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]DeliveryAddressDTO value)
        {
            value.UserId = value.CreatedBy = UserId;            
            _deliveryAddressAppService.AddDeliveryAddress(value);
        }

        [HttpDelete]
        public void Delete(string id)
        {
            _deliveryAddressAppService.DeleteDeliveryAddress(id);
        }

        [Route("provinces")]
        public IEnumerable<Province> GetProvinces()
        {
            return new List<Province> { 
                new Province{ 
                    ProvinceName="上海",
                     Cities = new List<City>{ 
                        new City{ 
                            CityName="上海市",
                            Areas = new List<string>{
                                "黄浦",
                                "徐汇",
                                "长宁",
                                "静安",
                                "普陀",
                                "虹口",
                                "杨浦",
                                "闵行",
                                "宝山",
                                "嘉定",
                                "浦东新区",
                                "金山",
                                "松江",
                                "青浦",
                                "奉贤",
                                "崇明"
                            }
                        }
                     }
                }
            };
        }

    }

    public class Province
    {
        public string ProvinceName { get; set; }

        public IEnumerable<City> Cities { get; set; }
    }

    public class City
    {
        public string CityName { get; set; }

        public IEnumerable<string> Areas { get; set; }
    }
}
