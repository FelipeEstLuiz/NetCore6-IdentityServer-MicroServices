using MicroServices.Web.Models;
using System.Threading.Tasks;

namespace MicroServices.Web.Services.IServices;

public interface ICouponService
{
    Task<CouponViewModel> GetCoupon(string code, string token);
}
