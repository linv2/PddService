using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PddService.Common;
using PddService.Common.Controller;
using PddService.Common.Filters;
using PddService.Common.Result;
using PddService.DataAccess;
using PddService.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace PddService.Controllers
{
    /// <summary>
    /// 地址
    /// </summary>
    public class AddressController : BaseController<AddressController>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="siteConfig"></param>
        public AddressController(ILogger<AddressController> logger, PddDbContext dbContext, SiteConfig siteConfig) : base(logger, dbContext, siteConfig)
        {
        }

        /// <summary>
        /// 获取地址列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="name">发货人名字</param>
        /// <param name="mobile">发货人电话</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Result<IEnumerable<Address>>), 200)]
        [HttpGet]
        public IActionResult List(int pageIndex = 1, int pageSize = 10, string name = null, string mobile = null)
        {
            pageIndex--;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            var query = DbContext.Address
                .AsNoTracking();//.
                                // Where(x => x.ProductStatus == ProductStatus.Success);
                                //if (UserType == UserType.Agent) {
            query = query.Where(x => x.UserId == UserInfo.Id && !x.Delete);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(x => x.Mobile.Contains(mobile));
            }

            var data = query.OrderByDescending(x => x.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return SuccessList(data, query.Count());
        }
        /// <summary>
        /// 根据ID获取单个地址信息
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>

        [ProducesResponseType(typeof(Result<Address>), 200)]
        [HttpGet]
        public IActionResult Get(int Id)
        {
            var address = DbContext.Address.FirstOrDefault(x => x.UserId == UserInfo.Id && x.Id == Id && !x.Delete);
            return Success(address);
        }

        /// <summary>
        /// 删除一个已存在的地址
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult Delete(int id)
        {
            var address = DbContext.Address.FirstOrDefault(x => x.UserId == UserInfo.Id && x.Id == id && !x.Delete);
            Assert.IsNull(address, "数据不存在");
            Assert.IsTrue(address.Default, "默认地址不允许删除");
            address.Delete = true;
            DbContext.SaveChanges();
            return OkResult;
        }

        /// <summary>
        /// 添加一个新地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost, ModelValidation]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult Add([FromBody, Bind("Province", "City", "Town", "Name", "Mobile", "Detail")]Address address)
        {
            address.Delete = false;
            address.Default = !DbContext.Address.Any(x => x.UserId == UserInfo.Id);
            address.UserId = UserInfo.Id;
            DbContext.Address.Add(address);
            DbContext.SaveChanges();
            return OkResult;
        }

        /// <summary>
        /// 编辑一个已经存在地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost, ModelValidation]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult Edit([FromBody, Bind("Province", "City", "Town", "Name", "Mobile", "Detail")]Address address)
        {
            var model = DbContext.Address.FirstOrDefault(x => x.Id == address.Id && x.UserId == UserInfo.Id && !x.Delete);
            Assert.IsNull(model, "数据不存在");
            model.Name = address.Name;
            model.Mobile = address.Mobile;
            model.Province = address.Province;
            model.City = address.City;
            model.Town = address.Town;
            model.Detail = address.Detail;
            DbContext.SaveChanges();
            return OkResult;
        }
        /// <summary>
        /// 设置当前地址为默认地址
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result), 200)]
        public IActionResult SetDefault(int id)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {


                try
                {
                    var userId = UserInfo.Id;
                    DbContext.Address.Where(x => x.UserId == userId && x.Default && !x.Delete).Update(x => new Address()
                    {
                        Default = false
                    });
                    DbContext.Address.Where(x => x.UserId == userId && !x.Delete && x.Id == id).Update(x => new Address()
                    {
                        Default = true
                    });
                    transaction.Commit();
                    DbContext.SaveChanges();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return OkResult;
        }
    }

}
