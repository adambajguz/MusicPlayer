using System;
using System.Linq.Expressions;

namespace MusicPlayer.UWP.Controllers.ZMisc
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> IsSatisfiedBy { get; }
    }
}
