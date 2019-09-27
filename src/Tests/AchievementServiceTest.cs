using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects;
using BusinessLayer.Repository;
using BusinessLayer.Services.Generic.Achievement;
using DAL.BaHuEntities;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Tests.Helpers;

namespace Tests
{
    public class AchievementServiceTest : BaseTest<BaHuAchievement, BaHuAchievementDto, AchievementFilterDto>
    {
        private AchievementService<BaHuAchievement, BaHuAchievementDto, UserDto ,AchievementFilterDto> _service { get;
            set;
        }
        
        private List<BaHuAchievementDto> _achievements { get; set; }

        [SetUp]
        public void SetUpTest()
        {
            _service = new AchievementService<BaHuAchievement, BaHuAchievementDto, UserDto, AchievementFilterDto>(
                AutoMapper,
                Repository,
                DbContext,
                ActualTypes,
                QueryBase,
                new Repository<BaHuReward>(AutoMapper,
                    DbContext,
                    ActualTypes));
        }

        [Test]
        public async Task AddAchievementInRepo()
        {
        }
        
    }
}