using Data.Entities;
using Data.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFCore
{
    public static class DataSeeder
    {
        public static void SeedSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>()
                .HasData(
                    new Setting { Id = Guid.NewGuid(), Key = "REGISTRATION_FEE_PERCENT", Value = "0.05", DataUnit = SettingDataUnit.Percent, Description = "Phần trăm phí đăng ký." },
                    new Setting { Id = Guid.NewGuid(), Key = "DEPOSIT_PERCENT", Value = "10", DataUnit = SettingDataUnit.Percent, Description = "Phần trăm tiền đặt cọc." }
                );
        }
    }
}
