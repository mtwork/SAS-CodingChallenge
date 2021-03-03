using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

        }


        public SearchResults Search(SearchOptions options)
        {
            var ShirtsCollection = _shirts.ToList();            

            if (options.Colors.Any())
                ShirtsCollection = ShirtsCollection.Where(x => options.Colors.Any(c => c.Id.Equals(x.Color.Id))).ToList();

            if (options.Sizes.Any())
                ShirtsCollection = ShirtsCollection.Where(x => options.Sizes.Any(s => s.Id.Equals(x.Size.Id))).ToList();

            return new SearchResults
            {
                Shirts = ShirtsCollection,
                ColorCounts = GetColorCounts(ShirtsCollection).Result,                
                SizeCounts = GetSizeCounts(ShirtsCollection).Result
            };
        }

        private Task<List<ColorCount>> GetColorCounts(List<Shirt> result)
        {
            var _colorCountSummary = result.GroupBy(x => x.Color)
                .Select(c => new ColorCount()
                {
                    Color = c.Key,
                    Count = c.Count()
                }).ToList();

            var _nonFilterColors = Color.All
                                        .Where(x => !_colorCountSummary.Any(c => c.Color.Id.Equals(x.Id)))
                                        .Select(x => new ColorCount() { Color = x, Count = 0 })
                                        .ToList();

            _colorCountSummary.AddRange(_nonFilterColors);
            return Task.FromResult(_colorCountSummary);
        }
           


        private Task<List<SizeCount>> GetSizeCounts(List<Shirt> result)
        {
            var _sizeCountSummary = result.GroupBy(x => x.Size)
                .Select(c => new SizeCount()
                {
                    Size = c.Key,
                    Count = c.Count()
                }).ToList();

            var _nonFilterSizes = Size.All
                                    .Where(x => !_sizeCountSummary.Any(s => s.Size.Id.Equals(x.Id)))
                                    .Select(x => new SizeCount() { Size  = x, Count = 0 }).ToList();

            _sizeCountSummary.AddRange(_nonFilterSizes);
            return Task.FromResult(_sizeCountSummary);
        }
    }
}