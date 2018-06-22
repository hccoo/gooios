using Gooios.GoodsService.Applications.DTOs;
using Gooios.GoodsService.Domains.Repositories;
using Gooios.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.GoodsService.Applications.Services
{
    public interface IGoodsTagStatisticsAppService : IApplicationServiceContract
    {
        GoodsTagStatisticsDTO Get(string goodsId);
    }

    public class GoodsTagStatisticsAppService : ApplicationServiceContract, IGoodsTagStatisticsAppService
    {
        readonly ICommentTagRepository _commentTagRepository;
        readonly ITagRepository _tagRepository;
        readonly IDbUnitOfWork _dbUnitOfWork;

        public GoodsTagStatisticsAppService(ICommentTagRepository commentTagRepository, ITagRepository tagRepository, IDbUnitOfWork dbUnitOfWork)
        {
            _tagRepository = tagRepository;
            _commentTagRepository = commentTagRepository;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public GoodsTagStatisticsDTO Get(string goodsId)
        {
            var results = _commentTagRepository.GetFiltered(o => o.GoodsId == goodsId).GroupBy(g => g.TagId).Select(item => new TagStatisticsDTO { TagId = item.Key, Count = item.Count(), TagName= _tagRepository.Get(item.Key)?.Name }).ToList();

            //results.ForEach(item => {
            //    var name = _tagRepository.Get(item.TagId)?.Name;
            //    item.TagName = name;
            //});

            return new GoodsTagStatisticsDTO { GoodsId = goodsId, TagStatistics = results };
        }
    }
}
