using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Adapters
{
    public interface IAdapter<TModel, TSource>
    {
        ICollection<ValidationResult> Errors { get; }

        void Fill(ICollection<TModel> models, ICollection<TSource> sources, Func<TModel> modelBuilder);

        void Fill(TModel model, TSource source);
    }
}