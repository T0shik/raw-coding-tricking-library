using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api.ViewModels
{
    public static class ModerationItemViewModels
    {
        public static readonly Func<ModerationItem, object> Create = Projection.Compile();

        public static Expression<Func<ModerationItem, object>> Projection =>
            modItem => new
            {
                modItem.Id,
                modItem.Current,
                modItem.Target,
                modItem.Reason,
                modItem.Type,
                Updated = modItem.Updated.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
            };

        public static readonly Func<ModerationItem, object> CreateFlat = FlatProjection.Compile();

        public static Expression<Func<ModerationItem, object>> FlatProjection =>
            modItem => new
            {
                modItem.Id,
                modItem.Current,
                modItem.Target,
                modItem.Reason,
                modItem.Type,
                Updated = modItem.Updated.ToLocalTime().ToString("HH:mm dd/MM/yyyy"),
                Reviews = modItem.Reviews.AsQueryable().Select(x => x.Status).ToList(),
            };
    }
}