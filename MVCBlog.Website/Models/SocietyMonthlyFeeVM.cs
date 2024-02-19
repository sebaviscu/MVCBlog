using MVCBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBlog.Website.Models
{
    public class SocietyMonthlyFeeVM
    {
        public Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public AspNetUser User { get; set; }
        public DateTime Period { get; set; }
        public decimal Amount { get; set; }

        [Obsolete]
        public SocietyMonthlyFee ConvertToBusinessEntity(SocietyMonthlyFeeVM vm)
        {
            return new SocietyMonthlyFee
            {
                Id = vm.Id,
                AspNetUserId = vm.AspNetUserId,
                //Period = new DateTime(YearPeriod, MonthPeriod, 1),
                //Amount = vm.Amount,
                //PaymentDate = vm.PaymentDate
            };
        }

        [Obsolete]
        public SocietyMonthlyFeeVM ConvertToViewModel(SocietyMonthlyFee entity)
        {
            return new SocietyMonthlyFeeVM
            {
                Id = entity.Id,
                AspNetUserId = entity.AspNetUserId,
                //YearPeriod = entity.YearPeriod,
                //MonthPeriod = entity.MonthPeriod,
                //Amount = entity.Amount,
                //PaymentDate = entity.PaymentDate
            };
        }
    }

    public class FeeVM
    {
        public int YearPeriod { get; set; }
        public int MonthPeriod { get; set; }
        public decimal Amount { get; set; }
    }
}